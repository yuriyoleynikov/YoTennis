using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YoTennis.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using YoTennis.Models;

namespace YoTennis.Controllers
{
    [Authorize]
    public class StatsController : Controller
    {
        private IStatsService _statsService;
        private string UserId => User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public StatsController(IStatsService statsService)
        {
            _statsService = statsService;
        }

        public async Task<IActionResult> PlayersStats(int count = 10, int skip = 0, SortForPlayerStats sort = SortForPlayerStats.None)
        {
            var playersStatsModelList = await _statsService.GetPlayersStatsModel(UserId, count, skip, sort);

            var containerForPlayersStats = new ContainerForPlayersStats
            {
                ListPlayerStatsModelView = playersStatsModelList.Select(model => new PlayerStatsModelView
                {
                    Player = model.Player,
                    Matches = model.Matches,
                    Completed = model.Completed,
                    Won = model.Won,
                    Lost = model.Lost,
                    AggregatedMatchStats = model.AggregatedMatchStats
                }).ToList(),

                Count = count,
                Skip = skip,
                Sort = sort,
                TotalPlayers = await _statsService.GetTotalPlayers(UserId)
            };
            
            return View(containerForPlayersStats);
        }

        public async Task<IActionResult> PlayerStats(string id)
        {
            var playerStatsModel = await _statsService.GetPlayerStatsModel(UserId, id);

            return View(new PlayerStatsModelView
            {
                Player = playerStatsModel.Player,
                Matches = playerStatsModel.Matches,
                Completed = playerStatsModel.Completed,
                Won = playerStatsModel.Won,
                Lost = playerStatsModel.Lost,
                AggregatedMatchStats = playerStatsModel.AggregatedMatchStats
            });
        }
    }
}