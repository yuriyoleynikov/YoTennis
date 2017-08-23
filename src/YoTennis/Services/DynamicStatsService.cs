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
            string player = null, string playerUserId = null)
        {
            var players = player == null ? null : Enumerable.Repeat(player, 1);

            var count = await _matchListService.GetMatchCount(userId, players);
            var mathes = await _matchListService.GetMatches(userId, count, 0, players);

            var result = new List<PlayerStatsModel>();
            
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
                        result.Add(firstPlayerCurrentStats);
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
                        result.Add(secondPlayerCurrentStats);
                    }
                }
            }

            return result;
        }

        public async Task<IEnumerable<PlayerStatsModel>> GetPlayersStatsModel(string userId) =>
            (await GetPlayersStatsModelInner(userId))
            .GroupBy(stat => stat.Player)
            .Select(group => group.Aggregate((acc, stat) => acc + stat))
            .OrderBy(stat => stat.Player);

        public async Task<PlayerStatsModel> GetPlayerStatsModel(string userId, string player) =>
            (await GetPlayersStatsModelInner(userId, player)).Aggregate(new PlayerStatsModel
            {
                Player = player,
                AggregatedMatchStats = new PlayerStatsMatchModel()
            }, (acc, stat) => acc + stat);

        public async Task<PlayerStatsModel> GetUserStatsModel(string userId) =>
            (await GetPlayersStatsModelInner(userId, null, userId)).Aggregate(new PlayerStatsModel
            {
                Player = userId,
                AggregatedMatchStats = new PlayerStatsMatchModel()
            }, (acc, stat) => acc + stat);        
    }
}
