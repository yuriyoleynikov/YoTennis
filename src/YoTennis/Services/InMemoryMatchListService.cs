using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Data;
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

        public Task<IEnumerable<string>> GetMatchesWithFilter(string userId, int count, int skip, IEnumerable<string> filterPlayer,
            IEnumerable<MatchState> filterState)
        {
            throw new NotImplementedException();
        }

        public Task RebuildMatchInfos()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetPlayers(string userId)
        {
            if (_users.TryGetValue(userId, out var matches))
            {
                var result = new List<string>();

                foreach (var match in matches)
                {
                    var matchState = await match.Value.GetStateAsync();
                    if (matchState.FirstPlayer != null && !result.Contains(matchState.FirstPlayer))
                        result.Add(matchState.FirstPlayer);
                    if (matchState.SecondPlayer != null && !result.Contains(matchState.SecondPlayer))
                        result.Add(matchState.SecondPlayer);
                }
            }

            return await Task.FromResult(Enumerable.Empty<string>());
        }

        public async Task<IEnumerable<MatchInfoModel>> GetMatches(string userId, int count, int skip,
            IEnumerable<string> filterPlayer = null, IEnumerable<MatchState> filterState = null, Sort sort = Sort.None)
        {
            var result = new List<MatchInfoModel>();

            if (_users.TryGetValue(userId, out var matches))
            {
                foreach (var match in matches.Skip(skip).Take(count))
                {
                    var matchState = await match.Value.GetStateAsync();
                    var matchInfo = matchState.ToMatchInfo();
                    result.Add(new MatchInfoModel
                    {
                        MatchId = match.Key,
                        FirstPlayer = matchState.FirstPlayer,
                        MatchScore = matchInfo.MatchScore,
                        MatchStartedAt = matchInfo.MatchStartedAt,
                        SecondPlayer = matchState.SecondPlayer,
                        State = matchState.State,
                        Winner = matchInfo.Winner
                    });
                }
            }
            return result;
        }

        public Task<int> GetMatchCount(string userId, IEnumerable<string> filterPlayer = null, IEnumerable<MatchState> filterState = null)
        {
            if (_users.TryGetValue(userId, out var matches))
            {
                return Task.FromResult(matches.Count());
            }
            return Task.FromResult(0);
        }

        public Task<string> GetMatchOwner(string matchId)
        {
            throw new NotImplementedException();
        }

        public Task<string> CopyMatch(string userId, string matchId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MatchModel>> GetMatchesWithUser(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
