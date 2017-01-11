using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YoTennis.Services;
using YoTennis.Models.Commands;
using YoTennis.Models;
using YoTennis.Models.Events;

namespace YoTennis.Controllers
{
    public class MatchController : Controller
    {
        private IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        public IActionResult Create()
        {
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index()
        {
            var state = await _matchService.GetState();

            if (state.State == Models.MatchState.NotStarted)
                return View("NotStarted", state);

            if (state.State == Models.MatchState.Drawing)
                return View("Drawing", state);

            if (state.State == Models.MatchState.BeginningGame)
                return View("BeginningGame", state);

            if (state.State == Models.MatchState.PlayingGame)
                return View("PlayingGame", state);

            if (state.State == Models.MatchState.ChangingSides)
                return View("ChangingSides", state);

            if (state.State == Models.MatchState.BeginningTiebreak)
                return View("BeginningTiebreak", state);

            if (state.State == Models.MatchState.PlayingTiebreak)
                return View("PlayingTiebreak", state);

            if (state.State == Models.MatchState.ChangingSidesOnTiebreak)
                return View("ChangingSidesOnTiebreak", state);

            if (state.State == Models.MatchState.Completed)
                return View("Completed", state);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Start(StartCommand strartCommand)
        {
            await _matchService.AddEvent(new StartEvent
            {
                OccuredAt = DateTime.UtcNow,
                Settings = strartCommand.MatchSettings,
                FirstPlayer = strartCommand.FirstPlayer,
                SecondPlayer = strartCommand.SecondPlayer
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Draw(DrawCommand drawCommand)
        {
            await _matchService.AddEvent(new DrawEvent
            {
                OccuredAt = DateTime.UtcNow,
                PlayerOnLeft = drawCommand.PlayerOnLeft,
                PlayerServes = drawCommand.PlayerServes
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> StartGame(StartGameCommand startGameCommand)
        {
            await _matchService.AddEvent(new StartGameEvent
            {
                OccuredAt = DateTime.UtcNow
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> PointToFirst(PointCommand pointCommand)
        {
            await _matchService.AddEvent(new PointEvent
            {
                OccuredAt = DateTime.UtcNow,
                PlayerPoint = Player.First
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> PointToSecond(PointCommand pointCommand)
        {
            await _matchService.AddEvent(new PointEvent
            {
                OccuredAt = DateTime.UtcNow,
                PlayerPoint = Player.Second
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeSides(ChangeSidesCommand changeSidesCommand)
        {
            await _matchService.AddEvent(new ChangeSidesGameEvent
            {
                OccuredAt = DateTime.UtcNow
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> StartTiebreak(StartTiebreakCommand startTiebreakCommand)
        {
            await _matchService.AddEvent(new StartTiebreakEvent
            {
                OccuredAt = DateTime.UtcNow
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeSidesOnTiebreak(ChangeSidesOnTiebreakCommand changeSidesOnTiebreakCommand)
        {
            await _matchService.AddEvent(new ChangeSidesOnTiebreakEvent
            {
                OccuredAt = DateTime.UtcNow
            });

            return RedirectToAction(nameof(Index));
        }
    }
}