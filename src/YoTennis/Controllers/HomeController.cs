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

        public async Task<IActionResult> Index(int count = 10, int skip = 0)
        {
            var totalCount = await _matchListService.GetMatchCount(UserId);

            var idsForSelectMatches = await _matchListService.GetMatches(UserId, count, skip);

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
                TotalCount = totalCount,
                Count = count,
                Skip = skip };

            return View(containerForMatchModel);
        }
        
        public async Task<IActionResult> Delete(string id, string returnUrl)
        {
            try
            {
                await _matchListService.DeleteMatch(UserId, id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction(nameof(Index));
            return LocalRedirect(returnUrl);
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
