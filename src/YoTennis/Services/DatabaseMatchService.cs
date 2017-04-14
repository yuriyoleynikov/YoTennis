using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Data;
using YoTennis.Models;

namespace YoTennis.Services
{
    public class DatabaseMatchService : IMatchService
    {
        private readonly MyDbContext _context;
        private Guid _matchId;

        public DatabaseMatchService(MyDbContext context, Guid matchId)
        {
            _context = context;
            _matchId = matchId;
        }

        public async Task AddEventAsync(GameEvent gameEvent)
        {
            var _gameHandler = await LoadGameHandler();

            var version = _gameHandler.Events.Count;

            _context.MatchEvents.Add(
                new MatchEvent
                {
                    Event = JsonConvert.SerializeObject(gameEvent, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects }),
                    MatchId = _matchId,
                    Version = version
                });

            await _context.SaveChangesAsync();
        }

        

        public async Task<MatchModel> GetStateAsync()
        {
            var _gameHandler = await LoadGameHandler();

            return _gameHandler.CurrentState;
        }

        private async Task<GameHandler> LoadGameHandler()
        {
            GameHandler _gameHandler = new GameHandler();            

            foreach (var matchEvent in await _context.MatchEvents.Where(matchEvent => matchEvent.MatchId == _matchId).OrderBy(matchEvent => matchEvent.Version).ToArrayAsync())
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
            var lastMatchEvent = await _context.MatchEvents
                .Where(matchEvent => matchEvent.MatchId == _matchId)
                .OrderByDescending(matchEvent => matchEvent.Version)
                .FirstOrDefaultAsync();

            if (lastMatchEvent != null)
            {
                _context.MatchEvents.Remove(lastMatchEvent);
                await _context.SaveChangesAsync();
            }
        }
    }
}