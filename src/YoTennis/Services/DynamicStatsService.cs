using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Data;
using YoTennis.Models;

namespace YoTennis.Services
{
    public class DynamicStatsService : IStatsService
    {
        private IMatchListService _matchListService;

        public DynamicStatsService(IMatchListService matchListService)
        {
            _matchListService = matchListService;
        }

        public async Task<IEnumerable<PlayerStatsModel>> GetPlayersStatsModelInner(string userId,
            int count, int skip, SortForPlayerStats sort, string player = null, string playerUserId = null)
        {
            var players = player == null ? null : Enumerable.Repeat(player, 1);

            var total = await _matchListService.GetMatchCount(userId, players);

            var mathes = await _matchListService.GetMatches(userId, total == 0 ? 1 : total, 0, players);

            var result = new Dictionary<string, PlayerStatsModel>();

            foreach (var matchInfo in mathes)
            {
                var matchService = await _matchListService.GetMatchService(userId, matchInfo.MatchId);
                var matchState = await matchService.GetStateAsync();

                if (matchState.State != MatchState.NotStarted)
                {
                    var playersStats = await matchService.GetPlayersMatchStats();

                    if (((player == null || matchInfo.FirstPlayer == player) && playerUserId == null) ||
                        (playerUserId != null && matchState.FirstPlayerUserId == playerUserId))
                    {
                        var firstPlayerCurrentStats = new PlayerStatsModel
                        {
                            Player = matchInfo.FirstPlayer,
                            Matches = 1,
                            Completed = matchInfo.State == MatchState.Completed ? 1 : 0,
                            Won = matchInfo.Winner == Player.First ? 1 : 0,
                            Lost = matchInfo.Winner == Player.Second ? 1 : 0,
                            AggregatedMatchStats = playersStats.FirstPlayer
                        };
                        if (matchInfo.FirstPlayer != null)
                            result[matchInfo.FirstPlayer] = result.TryGetValue(matchInfo.FirstPlayer, out var firstPlayerOldStats)
                                ? firstPlayerOldStats + firstPlayerCurrentStats
                                : firstPlayerCurrentStats;
                    }

                    if (((player == null || matchInfo.SecondPlayer == player) && playerUserId == null) ||
                            (playerUserId != null && matchState.SecondPlayerUserId == playerUserId))
                    {
                        var secondPlayerCurrentStats = new PlayerStatsModel
                        {
                            Player = matchInfo.SecondPlayer,
                            Matches = 1,
                            Completed = matchInfo.State == MatchState.Completed ? 1 : 0,
                            Won = matchInfo.Winner == Player.Second ? 1 : 0,
                            Lost = matchInfo.Winner == Player.First ? 1 : 0,
                            AggregatedMatchStats = playersStats.SecondPlayer
                        };

                        if (matchInfo.SecondPlayer != null)
                            result[matchInfo.SecondPlayer] = result.TryGetValue(matchInfo.SecondPlayer, out var secondPlayerOldStats)
                                ? secondPlayerOldStats + secondPlayerCurrentStats
                                : secondPlayerCurrentStats;
                    }
                }
            }

            return result.Values.BySortForPlayerStats(sort).Skip(skip).Take(count);
        }

        public Task<IEnumerable<PlayerStatsModel>> GetPlayersStatsModel(string userId, int count, int skip, SortForPlayerStats sort) =>
            GetPlayersStatsModelInner(userId, count, skip, sort);

        public async Task<PlayerStatsModel> GetPlayerStatsModel(string userId, string player) =>
            (await GetPlayersStatsModelInner(userId, 100, 0, SortForPlayerStats.None, player)).SingleOrDefault() ??
                new PlayerStatsModel
                {
                    Player = player,
                    Matches = 0,
                    Completed = 0,
                    Won = 0,
                    Lost = 0,
                    AggregatedMatchStats = new PlayerStatsMatchModel
                    {
                        TotalPoints = 0,

                        Ace = 0,
                        Backhand = 0,
                        DoubleFaults = 0,
                        Error = 0,
                        Forehand = 0,
                        NetPoint = 0,
                        UnforcedError = 0,

                        FirstServe = 0,
                        FirstServeSuccessful = 0,
                        WonOnFirstServe = 0,

                        SecondServe = 0,
                        SecondServeSuccessful = 0,
                        WonOnSecondServe = 0
                    }
                };

        public async Task<int> GetTotalPlayers(string userId)
        {
            var total = await _matchListService.GetMatchCount(userId);
            var mathes = await _matchListService.GetMatches(userId, total == 0 ? 1 : total, 0);

            var result = new HashSet<string>();

            foreach (var matchInfo in mathes)
            {
                result.Add(matchInfo.FirstPlayer);
                result.Add(matchInfo.SecondPlayer);
            }

            return result.Count;
        }

        public async Task<PlayerStatsModel> GetUserStatsModel(string userId) =>
            (await GetPlayersStatsModelInner(userId, 100, 0, SortForPlayerStats.None, null, userId)).Aggregate(new PlayerStatsModel
            {
                Player = userId,
                AggregatedMatchStats = new PlayerStatsMatchModel()
            }, (acc, stat) => acc + stat);
    }
}
