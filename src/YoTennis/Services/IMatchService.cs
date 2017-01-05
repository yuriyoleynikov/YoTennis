using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Models;

namespace YoTennis.Services
{
    public interface IMatchService
    {
        Task AddEvent(GameEvent gameEvent);
        Task<MatchModel> GetState();
    }
}
