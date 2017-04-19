using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Models;

namespace YoTennis.Services
{
    public class InNewMemoryMatchService : IMatchService
    {
        private Dictionary<string, Dictionary<string, List<GameEvent>>> _users;
        private List<GameEvent> _matchEvents;
        private string _matchId;
        private string _userId;

        public InNewMemoryMatchService(Dictionary<string, Dictionary<string, List<GameEvent>>> users, string userId, string matchId)
        {
            _users = users;
            var matches = _users[userId];
            _matchEvents = matches[matchId];            
            _matchId = matchId;
            _userId = userId;
        }        

        public async Task AddEventAsync(GameEvent gameEvent)
        {
            var _gameHandler = await LoadGameHandler();
            
            _matchEvents.Add(gameEvent);  
        }
        
        public async Task<MatchModel> GetStateAsync()
        {
            var _gameHandler = await LoadGameHandler();

            return _gameHandler.CurrentState;
        }

        private async Task<GameHandler> LoadGameHandler()
        {
            GameHandler _gameHandler = new GameHandler();

            foreach (var matchEvent in _matchEvents)
                _gameHandler.AddEvent(matchEvent);

            return _gameHandler;
        }

        public Task Reset()
        {
            //_matchId = null;

            return Task.FromResult(0);
        }

        public async Task UndoAsync()
        {
            var matches = _users[_userId];
            var matchEvents = matches[_matchId];
            if (matchEvents.Count != 0)
            {
                matchEvents = matchEvents.Take(_matchEvents.Count - 1).ToList();

                matches.Remove(_matchId);
                matches.Add(_matchId, matchEvents);

                _users.Remove(_userId);
                _users.Add(_userId, matches);
            }
        }
    }
}