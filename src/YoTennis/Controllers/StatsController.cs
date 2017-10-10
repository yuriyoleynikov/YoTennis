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

        public static (int count, int skip) CorrectPaginationForPlayers(int totalCount, int count, int skip, int defaultCount = 10)
        {
            if (totalCount < 0)
                throw new ArgumentOutOfRangeException(nameof(totalCount), "totalCount < 0");
            if (defaultCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(defaultCount), "defaultCount <= 0");

            if (count <= 0)
                count = defaultCount;

            var skip_initial = skip;

            if (skip > totalCount)
            {
                var skip_for_page0 = skip % count;
                if (skip_for_page0 != 0)
                {
                    skip = skip_for_page0 + ((totalCount - skip_for_page0) / count) * count;
                    if (skip_initial == skip)
                        skip = 0;
                }
                else
                    skip = totalCount / count * count;
            }

            if (skip == totalCount && totalCount != 0)
                skip = skip - count;

            if (skip < 0)
                skip = 0;

            return (count, skip);
        }

        public StatsController(IStatsService statsService)
        {
            _statsService = statsService;
        }

        public async Task<IActionResult> PlayersStats(int count = 10, int skip = 0, SortForPlayerStats sort = SortForPlayerStats.None)
        {
            var totalCount = await _statsService.GetTotalPlayers(UserId);

            var (newCount, newSkip) = CorrectPaginationForPlayers(totalCount, count, skip);

            if (count != newCount || skip != newSkip)
                return RedirectToAction(nameof(PlayersStats), new { count = newCount, skip = newSkip });

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