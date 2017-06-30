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

        

        public async Task<IActionResult> Index(int count = 10, int skip = 0, IEnumerable<string> player = null, IEnumerable<MatchState> state = null)
        {
            var totalCount = await _matchListService.GetMatchCount(UserId);
            var (newCount, newSkip) = CorrectPagination(totalCount, count, skip);

            if (count != newCount || skip != newSkip)
            {
                return RedirectToAction(nameof(Index), new { count = newCount, skip = newSkip });
            }

            player = player ?? Enumerable.Empty<string>();
            state = state ?? Enumerable.Empty<MatchState>();

            var idsForSelectMatches = await _matchListService.GetMatches3(UserId, newCount, newSkip, player, state);

            var listOfMatchModelView = new List<MatchModelView>();
            foreach (var id in idsForSelectMatches)
            {
                var match = await _matchListService.GetMatchService(UserId, id);
                var cur_state = await match.GetStateAsync();

                listOfMatchModelView.Add(new MatchModelView
                {
                    Id = id,
                    Players = cur_state.FirstPlayer != null ? cur_state.FirstPlayer + " - " + cur_state.SecondPlayer : "None",
                    Date = cur_state.MatchStartedAt != DateTime.MinValue ? cur_state.MatchStartedAt.ToString() : "None",
                    Status = cur_state.State.ToString(),
                    Score = MatchScoreExtensions.ToSeparatedScoreString(cur_state),
                    State = cur_state
                });
            }

            var containerForMatchModel = new ContainerForMatchModel
            {
                ListMatchModelView = listOfMatchModelView,
                TotalCount = totalCount,
                Count = newCount,
                Skip = newSkip,
                FilterPayers = await FilterAsync(),
                SelectedPlayers = player,                
                SelectedState = state
            };

            return View(containerForMatchModel);
        }

        public async Task<List<string>> FilterAsync()
        {
            List<string> FPlayers = new List<string>();
            var idsForAllMatches = await _matchListService.GetMatches(UserId, await _matchListService.GetMatchCount(UserId), 0);
            foreach (var id in idsForAllMatches)
            {
                var match = await _matchListService.GetMatchService(UserId, id);
                var state = await match.GetStateAsync();
                if (state.FirstPlayer != null)
                {
                    if (!FPlayers.Contains(state.FirstPlayer))
                    {
                        FPlayers.Add(state.FirstPlayer);
                    }
                    if (!FPlayers.Contains(state.SecondPlayer))
                    {
                        FPlayers.Add(state.SecondPlayer);
                    }
                }
            }

            return FPlayers;
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
