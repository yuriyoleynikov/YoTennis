using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Data;
using YoTennis.Models;

namespace YoTennis.Services
{
    public class DatabaseMatchListService : IMatchListService
    {
        private readonly MyDbContext _context;

        public DatabaseMatchListService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateMatch(string userId)
        {
            var guid = Guid.NewGuid();

            _context.Matches.Add(new Match { Id = guid, UserId = userId });
            _context.MatchInfos.Add(new MatchInfo { MatchId = guid, UserId = userId });
            await _context.SaveChangesAsync();

            return guid.ToString();
        }

        public async Task DeleteMatch(string userId, string matchId)
        {
            var matchToRemove = Guid.TryParse(matchId, out var guid)
                ? await _context.Matches.Where(match => match.UserId == userId && match.Id == guid)
                    .SingleOrDefaultAsync()
                : null;

            var matchInfoToRemove = Guid.TryParse(matchId, out var guidInfo)
                ? await _context.MatchInfos.Where(match => match.MatchId == guidInfo)
                    .SingleOrDefaultAsync()
                : null;

            if (matchToRemove == null || matchInfoToRemove == null)
                throw new KeyNotFoundException("Match not found.");

            _context.Matches.Remove(matchToRemove);
            _context.MatchInfos.Remove(matchInfoToRemove);
            await _context.SaveChangesAsync();
        }

        public Task<int> GetMatchCount(string userId) =>
            _context.Matches.Where(match => match.UserId == userId).CountAsync();

        public async Task<int> GetMatchCountWithFilter(string userId, IEnumerable<string> filterPlayer, IEnumerable<MatchState> filterState) =>
            await _context.MatchInfos
                .Where(matchInfo => matchInfo.UserId == userId)
                .ByPlayers(filterPlayer)
                .ByState(filterState)
                .CountAsync();

        public async Task<IEnumerable<MatchInfoModel>> GetMatchesInfosAsync(string userId)
        {
            var matchInfos = await _context.MatchInfos
                .Where(matchInfo => matchInfo.UserId == userId)
                .Select(matchInfo => new MatchInfoModel
                {
                    MatchId = matchInfo.MatchId.ToString(),
                    FirstPlayer = matchInfo.FirstPlayer,
                    SecondPlayer = matchInfo.SecondPlayer,
                    MatchStartedAt = matchInfo.MatchStartedAt,
                    MatchScore = matchInfo.MatchScore,
                    State = matchInfo.State,
                    Winner = matchInfo.Winner
                })
            .ToArrayAsync();

            return matchInfos;
        }

        public async Task<IEnumerable<MatchInfoModel>> GetMatchesWithFilterAndSort(string userId, int count, int skip,
            IEnumerable<string> filterPlayer, IEnumerable<MatchState> filterState, Sort sort)
        {
            var result = await _context.MatchInfos
                .Where(matchInfo => matchInfo.UserId == userId)
                .ByPlayers(filterPlayer)
                .ByState(filterState)
                .BySort(sort)
                .Skip(skip)
                .Take(count).ToArrayAsync();

            return result.Select(matchInfo => new MatchInfoModel
            {
                MatchId = matchInfo.MatchId.ToString(),
                Winner = matchInfo.Winner,
                FirstPlayer = matchInfo.FirstPlayer,
                SecondPlayer = matchInfo.SecondPlayer,
                MatchStartedAt = matchInfo.MatchStartedAt,
                State = matchInfo.State,
                MatchScore = matchInfo.MatchScore
            });
        }

        public async Task<IMatchService> GetMatchService(string userId, string matchId) =>
            (Guid.TryParse(matchId, out var guid) && await _context.Matches.Where(match => match.UserId == userId && match.Id == guid).AnyAsync())
            ? new DatabaseMatchService(_context, guid, userId)
            : throw new KeyNotFoundException("Match not found.");

        public async Task<IEnumerable<string>> GetPlayers(string userId)
        {
            var firstPlayers = await _context.MatchInfos
                .Where(matchInfo => matchInfo.UserId == userId)
                .Where(matchInfo => matchInfo.FirstPlayer != null)
                .Select(matchInfo => matchInfo.FirstPlayer)
                .Distinct()
                .ToArrayAsync();
            var secondPlayers = await _context.MatchInfos
                .Where(matchInfo => matchInfo.UserId == userId)
                .Where(matchInfo => matchInfo.SecondPlayer != null)
                .Select(matchInfo => matchInfo.SecondPlayer)
                .Distinct()
                .ToArrayAsync();

            return new HashSet<string>(firstPlayers.Concat(secondPlayers)).OrderBy(player => player);
        }

        public async Task RebuildMatchInfosAsync()
        {
            var exceptions = new List<Exception>();

            foreach (var match in await _context.Matches.ToArrayAsync())
            {
                var currentMatch = new DatabaseMatchService(_context, match.Id, match.UserId);
                try
                {
                    await currentMatch.RebuildMatchInfoAsync();
                }
                catch (InvalidOperationException ex)
                {
                    exceptions.Add(new Exception("Error during rebuilding MatchInfo of match#" + match.Id, ex) { Data = { ["MatchId"] = match.Id } });
                }
            }

            if (exceptions.Count > 0)
                throw new AggregateException(exceptions);
        }
    }
}
