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
            var guid = Guid.Parse(matchId);

            var matchExists = await _context.Matches.Where(match => match.UserId == userId && match.Id == guid).AnyAsync();

            if (!matchExists)
                throw new KeyNotFoundException("Match not found.");

            _context.Matches.Remove(new Match { Id = guid, UserId = userId });
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetMatches(string userId)
        {
            var matchIds = await _context.Matches.Where(match => match.UserId == userId).Select(match => match.Id).ToArrayAsync();

            return matchIds.Select(matchId => matchId.ToString());
        }

        public async Task<IMatchService> GetMatchService(string userId, string matchId)
        {
            var guid = Guid.Parse(matchId);
            
            var matchExists = await _context.Matches.Where(match=>match.UserId == userId && match.Id == guid).AnyAsync();

            if (!matchExists)
                throw new KeyNotFoundException("Match not found.");

            return new DatabaseMatchService(_context, guid);
        }
    }
}
