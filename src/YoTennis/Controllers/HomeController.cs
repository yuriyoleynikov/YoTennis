﻿using System;
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

        public static (int count, int skip) CorrectPagination(int totalCount, int count, int skip, int defaultCount = 10)
        {
            if (totalCount < 0)
                throw new ArgumentOutOfRangeException(nameof(totalCount));
            if (defaultCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(defaultCount));

            if (count <= 0)
                count = defaultCount;

            if (skip > totalCount)
            {
                skip = (totalCount / count) * count;                
            }

            if (skip == totalCount)
            {
                if (totalCount !=0)
                    skip = skip - count;
                else
                    skip = 0;
            }
            else if (skip < 0)
            {
                skip = 0;
            }

            return (count, skip);
            /*
                    count > 0
                    skip >= 0
                    skip == 0 || skip < totalCount
                    count == requestedCount || requestedCount <= 0 && count == defaultCount
                    skip == requestedSkip || requestedSkip < 0 || requestedSkip >= totalCount
             */
        }

        public async Task<IActionResult> Index(int count = 10, int skip = 0)
        {
            try
            {
                var totalCount = await _matchListService.GetMatchCount(UserId);
                var (newCount, newSkip) = CorrectPagination(totalCount, count, skip);

                if (count != newCount || skip != newSkip)
                {
                    return RedirectToAction(nameof(Index), new { count = newCount, skip = newSkip });
                }

                var idsForSelectMatches = await _matchListService.GetMatches(UserId, newCount, newSkip);

                var listOfMatchModelView = new List<MatchModelView>();
                foreach (var id in idsForSelectMatches)
                {
                    var match = await _matchListService.GetMatchService(UserId, id);
                    var state = await match.GetStateAsync();
                    listOfMatchModelView.Add(new MatchModelView
                    {
                        Id = id,
                        Name = state.FirstPlayer != null ? state.FirstPlayer + " - " + state.SecondPlayer : "None",
                        Date = state.MatchStartedAt != DateTime.MinValue ? state.MatchStartedAt.ToString() : "None"
                    });
                }

                var containerForMatchModel = new ContainerForMatchModel
                {
                    ListMatchModelView = listOfMatchModelView,
                    TotalCount = totalCount,
                    Count = newCount,
                    Skip = newSkip
                };

                return View(containerForMatchModel);
            }
            catch (FormatException)
            {
                return NotFound();
            }
            
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
