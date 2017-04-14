using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Data;

namespace YoTennis.Services
{
    public class InMemoryMatchListService : IMatchListService
    {
        private IEnumerable<Match> _matches;
        private IEnumerable<MatchEvent> _matchEvents;

        public async Task<string> CreateMatch(string userId)
        {
            var guid = Guid.NewGuid();

            _matches.ToList().Add(new Match { Id = guid, UserId = userId });

            return guid.ToString();
        }

        public async Task DeleteMatch(string userId, string matchId)
        {
            var guid = Guid.Parse(matchId);

            var matchExists = _matches.Where(match => match.UserId == userId && match.Id == guid).Any();

            if (!matchExists)
                throw new KeyNotFoundException("Match not found.");

            _matches.ToList().Remove(new Match { Id = guid, UserId = userId });
        }

        public async Task<IEnumerable<string>> GetMatches(string userId)
        {
            var matchIds = _matches.Where(match => match.UserId == userId).Select(match => match.Id).ToArray();

            return matchIds.Select(matchId => matchId.ToString());
        }

        public async Task<IMatchService> GetMatchService(string userId, string matchId)
        {
            var guid = Guid.Parse(matchId);

            var matchExists = _matches.Where(match => match.UserId == userId && match.Id == guid).Any();

            if (!matchExists)
                throw new KeyNotFoundException("Match not found.");

            return new InNewMemoryMatchService(_matchEvents, guid);
        }
    }
}
