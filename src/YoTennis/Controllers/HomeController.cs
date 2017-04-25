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

        public async Task<IActionResult> Index()
        {
            var ids = await _matchListService.GetMatches(UserId);
            List<MyMatchModel> view = new List<MyMatchModel>();
            foreach (var i in ids)
            {
                var match = await _matchListService.GetMatchService(UserId, i);
                var state = await match.GetStateAsync();
                view.Add(new MyMatchModel { Name = state.FirstPlayer != null ? state.FirstPlayer + " - " + state.SecondPlayer : "None",
                    Date = state.MatchStartedAt != DateTime.MinValue ? state.MatchStartedAt.ToString() : "None" });
            }

            return View(view);
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
