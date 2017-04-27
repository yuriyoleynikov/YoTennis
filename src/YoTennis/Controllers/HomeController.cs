using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YoTennis.Models;
using YoTennis.Data;
using YoTennis.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace YoTennis.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IMatchListService _matchListService;
        private string UserId => User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public HomeController(IMatchListService matchListService)
        {
            _matchListService = matchListService;
        }

        public async Task<IActionResult> Index(int count, int skip)
        {
            count = count == 0 ? 10 : count;

            var idsForAllMatches = await _matchListService.GetMatches(UserId);
            var countMatches = idsForAllMatches.Count();

            var idsForSelectMatches = await _matchListService.GetMatches2(UserId, count, skip);

            var listOfMatchModelView = new List<MatchModelView>();
            foreach (var id in idsForSelectMatches)
            {
                var match = await _matchListService.GetMatchService(UserId, id);
                var state = await match.GetStateAsync();
                listOfMatchModelView.Add(new MatchModelView {
                    Id = id,
                    Name = state.FirstPlayer != null ? state.FirstPlayer + " - " + state.SecondPlayer : "None",
                    Date = state.MatchStartedAt != DateTime.MinValue ? state.MatchStartedAt.ToString() : "None" });
            }

            var containerForMatchModel = new ContainerForMatchModel {
                ListMatchModelView = listOfMatchModelView,
                CountMatches = countMatches,
                Count = count,
                Skip = skip };

            return View(containerForMatchModel);
        }
        
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _matchListService.DeleteMatch(UserId, id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var match = await _matchListService.GetMatchService(UserId, id);
                var matchState = await match.GetStateAsync();
                return View(matchState);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
