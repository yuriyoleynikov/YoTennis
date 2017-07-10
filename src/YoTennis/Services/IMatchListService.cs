using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Models;

namespace YoTennis.Services
{
    public interface IMatchListService
    {
        Task<string> CreateMatch(string userId);
        Task<int> GetMatchCount(string userId);
        Task DeleteMatch(string userId, string matchId);
        Task RebuildMatchInfosAsync();
        Task<IMatchService> GetMatchService(string userId, string matchId);
        Task<IEnumerable<string>> GetPlayers(string userId);
        Task<IEnumerable<MatchInfoModel>> GetMatchesWithFilterAndSort(string userId, int count,
            int skip, IEnumerable<string> filterPlayer, IEnumerable<MatchState> filterState, Sort sort);
    }
}
