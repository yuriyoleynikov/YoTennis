using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Models;

namespace YoTennis.Services
{
    public class InMemoryMatchService : IMatchService
    {        
        private GameHandler _gameHandler = new GameHandler();

        public Task AddEventAsync(GameEvent gameEvent)
        {
            _gameHandler.AddEvent(gameEvent);
            return Task.FromResult(0);
        }

        public Task<PlayersStatsMatchModel> GetPlayersMatchStats()
        {
            throw new NotImplementedException();
        }

        public Task<MatchModel> GetStateAsync()
        {
            return Task.FromResult(_gameHandler.CurrentState);
        }

        public Task RebuildMatchInfoAsync()
        {
            throw new NotImplementedException();
        }

        public Task Reset()
        {
            _gameHandler = new GameHandler();
            return Task.FromResult(0);
        }

        public Task UndoAsync()
        {
            _gameHandler.UndoLastEvent();
            return Task.FromResult(0);
        }
    }
}