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
using System.Text;
using YoTennis.Helpers;

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

        public async Task<IActionResult> Index(int count = 10, int skip = 0, IEnumerable<string> player = null,
            IEnumerable<MatchState> state = null, Sort sort = Sort.None)
        {
            //await _matchListService.RebuildMatchInfosAsync();

            player = player ?? Enumerable.Empty<string>();
            state = state ?? Enumerable.Empty<MatchState>();

            var totalCount = await _matchListService.GetMatchCountWithFilter(UserId, player, state);

            var (newCount, newSkip) = CorrectPagination(totalCount, count, skip);

            if (count != newCount || skip != newSkip)
                return RedirectToAction(nameof(Index), new { count = newCount, skip = newSkip });

            var matchInfoModels = await _matchListService.GetMatchesWithFilterAndSort(UserId, count, skip, player, state, sort);
            
            var containerForMatchModel = new ContainerForMatchModel
            {
                ListMatchModelView = matchInfoModels.Select(matchInfo => new MatchModelView
                {
                    Id = matchInfo.MatchId,
                    Players = matchInfo.FirstPlayer != null ? matchInfo.FirstPlayer + " - " + matchInfo.SecondPlayer : "None",
                    Date = matchInfo.MatchStartedAt != DateTime.MinValue ? matchInfo.MatchStartedAt.ToString() : "None",
                    Status = matchInfo.State.ToString(),
                    Score = matchInfo.MatchScore
                }).ToList(),
                TotalCount = totalCount,
                Count = newCount,
                Skip = newSkip,
                FilterPayers = (await _matchListService.GetPlayers(UserId)).ToList(),
                SelectedPlayers = player,
                SelectedState = state,
                Sort = sort
            };

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
