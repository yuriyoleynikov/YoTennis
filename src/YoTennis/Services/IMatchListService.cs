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
        Task DeleteMatch(string userId, string matchId);
        Task RebuildMatchInfos();
        Task<IMatchService> GetMatchService(string userId, string matchId);
        Task<IEnumerable<string>> GetPlayers(string userId);
        Task<IEnumerable<DateTime>> GetMatchDates(string userId);
        Task<IEnumerable<MatchInfoModel>> GetMatches(string userId, int count, int skip,
            IEnumerable<string> filterPlayer = null, IEnumerable<MatchState> filterState = null,
            DateTime? beginningWithDate = null, DateTime? finishingBeforeDate = null, Sort sort = Sort.None);
        Task<int> GetMatchCount(string userId, IEnumerable<string> filterPlayer = null, IEnumerable<MatchState> filterState = null,
            DateTime? beginningWithDate = null, DateTime? finishingBeforeDate = null);
        Task<string> GetMatchOwner(string matchId);
        Task<string> CopyMatch(string userId, string matchId);
        Task<IEnumerable<MatchModel>> GetMatchesWithUser(string userId);
    }
}
