using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Models;

namespace YoTennis.Services
{
    public interface IStatsService
    {
        Task<IEnumerable<PlayerStatsModel>> GetPlayersStatsModel(string userId, int count, int skip, SortForPlayerStats sort);
        Task<int> GetTotalPlayers(string userId);
        Task<PlayerStatsModel> GetPlayerStatsModel(string userId, string player);
        Task<PlayerStatsModel> GetUserStatsModel(string userId);
    }
}
