using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Models;

namespace YoTennis.Services
{
    public class InMemoryMatchService : IMatchService
    {
        private GameModel _gameModel = new GameModel();

        public async Task AddEvent(GameEvent gameEvent)
        {
            _gameModel.AddEvent(gameEvent);
        }

        public async Task<State> GetState()
        {
            return _gameModel.CurrentState;
        }
    }
}
