using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Models;

namespace YoTennis.Services
{
    public class InMemoryMatchListService : IMatchListService
    {
        private Dictionary<string, Dictionary<string, InMemoryMatchService>> _users =
            new Dictionary<string, Dictionary<string, InMemoryMatchService>>();

        public Task<string> CreateMatch(string userId)
        {
            var matches = GetOrCreateUser(userId);
            var matchId = Guid.NewGuid().ToString();

            matches.Add(matchId, new InMemoryMatchService());

            return Task.FromResult(matchId);
        }

        private Dictionary<string, InMemoryMatchService> GetOrCreateUser(string userId)
        {
            if (_users.TryGetValue(userId, out var userMatches))
                return userMatches;

            userMatches = new Dictionary<string, InMemoryMatchService>();
            _users.Add(userId, userMatches);

            return userMatches;
        }

        public Task DeleteMatch(string userId, string matchId)
        {
            if (!_users.TryGetValue(userId, out var matches) || !matches.ContainsKey(matchId))
                throw new KeyNotFoundException("Match not found.");

            matches.Remove(matchId);
            return Task.FromResult(0);
        }

        public Task<IEnumerable<string>> GetMatches(string userId, int count, int skip) =>
            Task.FromResult(_users.TryGetValue(userId, out var userMatches)
                ? userMatches.Keys.Skip(skip).Take(count)
                : Enumerable.Empty<string>());

        public Task<IMatchService> GetMatchService(string userId, string matchId) =>
            _users.TryGetValue(userId, out var matches) && matches.TryGetValue(matchId, out var matchService)
                ? Task.FromResult((IMatchService)matchService)
                : throw new KeyNotFoundException("Match not found.");

        public Task<int> GetMatchCount(string userId) =>
            Task.FromResult(_users.TryGetValue(userId, out var matches) ? matches.Count : 0);

        public Task<IEnumerable<string>> GetMatches3(string userId, int count, int skip, IEnumerable<string> filterPlayer,
            IEnumerable<MatchState> filterState)
        {
            throw new NotImplementedException();
        }
    }
}
