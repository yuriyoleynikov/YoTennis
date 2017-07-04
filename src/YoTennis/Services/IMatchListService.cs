using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Models;

namespace YoTennis.Services
{
    public interface IMatchListService
    {
        Task<IEnumerable<string>> GetPlayersAsync(string userId);
        Task<IEnumerable<string>> GetMatches(string userId, int count, int skip);
        Task<IEnumerable<string>> GetMatchesWithFilter(string userId, int count, int skip, IEnumerable<string> filterPlayer, IEnumerable<MatchState> filterState);
        Task<IMatchService> GetMatchService(string userId, string matchId);
        Task<string> CreateMatch(string userId);
        Task DeleteMatch(string userId, string matchId);
        Task<int> GetMatchCount(string userId);
        Task RebuildMatchInfosAsync();
    }
}
