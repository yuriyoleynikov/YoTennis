using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Data;
using YoTennis.Models;

namespace YoTennis.Services
{
    public class InNewMemoryMatchService : IMatchService
    {
        private readonly IEnumerable<MatchEvent> _matchEvents;
        private Guid _matchId;

        public InNewMemoryMatchService(IEnumerable<MatchEvent> matchEvents, Guid matchId)
        {
            _matchEvents = matchEvents;
            _matchId = matchId;
        }

        public async Task AddEventAsync(GameEvent gameEvent)
        {
            var _gameHandler = await LoadGameHandler();

            var version = _gameHandler.Events.Count;

            _matchEvents.ToList().Add(
                new MatchEvent
                {
                    Event = JsonConvert.SerializeObject(gameEvent, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects }),
                    MatchId = _matchId,
                    Version = version
                });            
        }
        
        public async Task<MatchModel> GetStateAsync()
        {
            var _gameHandler = await LoadGameHandler();

            return _gameHandler.CurrentState;
        }

        private async Task<GameHandler> LoadGameHandler()
        {
            GameHandler _gameHandler = new GameHandler();

            foreach (var matchEvent in _matchEvents.Where(matchEvent => matchEvent.MatchId == _matchId).OrderBy(matchEvent => matchEvent.Version).ToArray())
                _gameHandler.AddEvent(JsonConvert.DeserializeObject<GameEvent>(matchEvent.Event, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects }));

            return _gameHandler;
        }

        public Task Reset()
        {
            //_matchId = null;

            return Task.FromResult(0);
        }

        public async Task UndoAsync()
        {
            var lastMatchEvent = _matchEvents
                .Where(matchEvent => matchEvent.MatchId == _matchId)
                .OrderByDescending(matchEvent => matchEvent.Version)
                .ToList()
                .FirstOrDefault();

            if (lastMatchEvent != null)
            {
                _matchEvents.ToList().Remove(lastMatchEvent);
            }
        }
    }
}