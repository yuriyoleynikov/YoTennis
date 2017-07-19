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
        private IMatchListService _matchListService;
        private string UserId => User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public StatsController(IMatchListService matchListService)
        {
            _matchListService = matchListService;
        }

        public async Task<IActionResult> Index(string id)
        {
            try
            {
                var match = await _matchListService.GetMatchService(UserId, id);
                var playersMatchStats = await match.GetPlayersMatchStats();
                var matchState = await match.GetStateAsync();

                var playersStatsMatchView = new PlayersStatsMatchView
                {
                    FirstPlayerName = matchState.FirstPlayer ?? "First Player",
                    SecondPlayerName = matchState.SecondPlayer ?? "Second Player",
                    PlayersStatsMatchModel = playersMatchStats
                };

                return View(playersStatsMatchView);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}