using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Data;

namespace YoTennis.Models
{
    public static class Extentions
    {
        public static Player Other(this Player player) => player == Player.First ? Player.Second : Player.First;
        public static ServePosition Other(this ServePosition position) =>
            position == ServePosition.Left ? ServePosition.Right : ServePosition.Left;

        public static IQueryable<MatchInfo> ByPlayers(this IQueryable<MatchInfo> matchInfoModels, IEnumerable<string> filterPlayer)
        {
            if (filterPlayer == null || !filterPlayer.Any())
                return matchInfoModels;

            return matchInfoModels
                .Where(matchInfo => filterPlayer.Contains(matchInfo.FirstPlayer) || filterPlayer.Contains(matchInfo.SecondPlayer));
        }

        public static IQueryable<MatchInfo> ByState(this IQueryable<MatchInfo> matchInfoModels, IEnumerable<MatchState> filterState)
        {
            if (filterState == null || !filterState.Any())
                return matchInfoModels;

            return matchInfoModels
                .Where(matchInfo => filterState.Contains(matchInfo.State));
        }

        public static IQueryable<MatchInfo> BySort(this IQueryable<MatchInfo> matchInfoModels, Sort sort)
        {
            if (sort == Sort.None)
                return matchInfoModels;

            if (sort == Sort.PlayerNameAscending)
                return matchInfoModels.OrderBy(matchInfo => matchInfo.FirstPlayer)
                    .ThenBy(matchInfo => matchInfo.SecondPlayer);
            if (sort == Sort.PlayerNameDescending)
                return matchInfoModels.OrderByDescending(matchInfo => matchInfo.FirstPlayer)
                    .ThenByDescending(matchInfo => matchInfo.SecondPlayer);
            if (sort == Sort.DateAscending)
                return matchInfoModels.OrderBy(matchInfo => matchInfo.MatchStartedAt);
            if (sort == Sort.DateDescending)
                return matchInfoModels.OrderByDescending(matchInfo => matchInfo.MatchStartedAt);
            if (sort == Sort.StateAscending)
                return matchInfoModels.OrderBy(matchInfo => matchInfo.State);
            if (sort == Sort.StateDescending)
                return matchInfoModels.OrderByDescending(matchInfo => matchInfo.State);

            return matchInfoModels;
        }
    }

    public enum Sort
    {
        None,
        PlayerNameAscending,
        PlayerNameDescending,
        DateAscending,
        DateDescending,
        StateAscending,
        StateDescending
    }
}