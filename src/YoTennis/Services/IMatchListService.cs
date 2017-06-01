using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Services
{
    public interface IMatchListService
    {
        Task<IEnumerable<string>> GetMatches(string userId, int count, int skip);
        Task<IEnumerable<string>> GetMatches3(string userId, int count, int skip, IEnumerable<string> filterPlayer);
        Task<IMatchService> GetMatchService(string userId, string matchId);
        Task<string> CreateMatch(string userId);
        Task DeleteMatch(string userId, string matchId);
        Task<int> GetMatchCount(string userId);
    }
}
