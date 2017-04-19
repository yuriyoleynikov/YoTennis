using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Models;

namespace YoTennis.Services
{
    public class InMemoryMatchListService : IMatchListService
    {
        private Dictionary<string, Dictionary<string, List<GameEvent>>> _users = new Dictionary<string, Dictionary<string, List<GameEvent>>>();

        public async Task<string> CreateMatch(string userId)
        {
            var guid = Guid.NewGuid();
            var match = new Dictionary<string, List<GameEvent>>();

            if (_users.ContainsKey(userId))
            {
                match = _users[userId];
            }
            
            match.Add(guid.ToString(), new List<GameEvent>());
            _users[userId] = match;

            return guid.ToString();
        }

        public async Task DeleteMatch(string userId, string matchId)
        {
            var matches = _users[userId];

            if (!matches.ContainsKey(matchId))
                throw new KeyNotFoundException("Match not found.");

            matches.Remove(matchId);
        }

        public async Task<IEnumerable<string>> GetMatches(string userId)
        {
            if (!_users.ContainsKey(userId))
            {
                return new List<string>();
            }
            var matches = _users[userId];

            return matches.Keys;
        }

        public async Task<IMatchService> GetMatchService(string userId, string matchId)
        {
            var matches = _users[userId];

            if (!matches.ContainsKey(matchId))
                throw new KeyNotFoundException("Match not found.");

            return new InNewMemoryMatchService(_users, userId, matchId);
        }
    }
}
