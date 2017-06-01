using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Data;

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
            await _context.SaveChangesAsync();

            return guid.ToString();
        }

        public async Task DeleteMatch(string userId, string matchId)
        {
            var matchToRemove = Guid.TryParse(matchId, out var guid)
                ? await _context.Matches.Where(match => match.UserId == userId && match.Id == guid)
                    .SingleOrDefaultAsync()
                : null;

            if (matchToRemove == null)
                throw new KeyNotFoundException("Match not found.");

            _context.Matches.Remove(matchToRemove);
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

        public async Task<IEnumerable<string>> GetMatches3(string userId, int count, int skip, IEnumerable<string> filterPlayer)
        {
            if (filterPlayer == Enumerable.Empty<string>())
            {
                var matchIds = await _context.Matches.Where(match => match.UserId == userId)
                .Select(match => match.Id).Skip(skip).Take(count).ToArrayAsync();

                return matchIds.Select(matchId => matchId.ToString());
            }
            else
            {
                var matchIds = await _context.Matches.Where(match => match.UserId == userId)
                .Select(match => match.Id).ToArrayAsync();

                var matchIds2 = new List<string>();

                foreach (var guid in matchIds)
                {
                    var match_ = await GetMatchService(userId, guid.ToString());
                    var state = await match_.GetStateAsync();
                    if (filterPlayer.Contains(state.FirstPlayer) || filterPlayer.Contains(state.SecondPlayer))
                    {
                        matchIds2.Add(guid.ToString());
                    }
                }

                return matchIds2.Skip(skip).Take(count).Select(matchId => matchId.ToString());
            }
        }

        public async Task<IMatchService> GetMatchService(string userId, string matchId) =>
            (Guid.TryParse(matchId, out var guid) && await _context.Matches.Where(match => match.UserId == userId && match.Id == guid).AnyAsync())
            ? new DatabaseMatchService(_context, guid)
            : throw new KeyNotFoundException("Match not found.");
    }
}
