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
            _context.MatchInfos.Add(new MatchInfo { MatchId = guid });
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

        public async Task<IEnumerable<string>> GetMatches(string userId, int count, int skip)
        {
            var matchIds = await _context.Matches.Where(match => match.UserId == userId)
                .Select(match => match.Id).Skip(skip).Take(count).ToArrayAsync();

            return matchIds.Select(matchId => matchId.ToString());
        }

        public async Task<IEnumerable<string>> GetMatchesWithFilter(string userId, int count, int skip, IEnumerable<string> filterPlayer,
            IEnumerable<MatchState> filterState)
        {
            var matchIds = await _context.Matches.Where(match => match.UserId == userId)
            .Select(match => match.Id).ToArrayAsync();

            var resultIds = new List<string>();

            resultIds = await _context.MatchInfos
                .Where(matchInfo => matchIds.Contains(matchInfo.MatchId))
                .Where(matchInfo => filterPlayer.Contains(matchInfo.FirstPlayer) || filterPlayer.Contains(matchInfo.SecondPlayer) || !filterPlayer.Any())
                .Where(matchInfo => !filterState.Any() || filterState.Contains(matchInfo.State))
                .Select(sate => sate.MatchId.ToString())
                .ToListAsync();

            return resultIds.Skip(skip).Take(count);
        }

        public async Task<IMatchService> GetMatchService(string userId, string matchId) =>
            (Guid.TryParse(matchId, out var guid) && await _context.Matches.Where(match => match.UserId == userId && match.Id == guid).AnyAsync())
            ? new DatabaseMatchService(_context, guid)
            : throw new KeyNotFoundException("Match not found.");

        public async Task<IEnumerable<string>> GetPlayersAsync(string userId)
        {
            var matchIds = await _context.Matches.Where(match => match.UserId == userId)
            .Select(match => match.Id).ToArrayAsync();

            var players = new List<string>();

            foreach (var state in await _context.MatchInfos.Where(matchInfo => matchIds.Contains(matchInfo.MatchId)).ToArrayAsync())            
                if (state.FirstPlayer != null)
                {
                    if (!players.Contains(state.FirstPlayer))
                        players.Add(state.FirstPlayer);
                    if (!players.Contains(state.SecondPlayer))
                        players.Add(state.SecondPlayer);
                }

            return players;
        }

        public async Task RebuildMatchInfosAsync()
        {
            foreach (var match in await _context.Matches.ToArrayAsync())
            {
                var currentMatch = new DatabaseMatchService(_context, match.Id);
                await currentMatch.RebuildMatchInfoAsync();
            }
        }
    }
}
