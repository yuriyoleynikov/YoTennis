using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YoTennis.Services;
using YoTennis.Models.Commands;
using YoTennis.Models;
using YoTennis.Models.Events;
using YoTennis.Models.Match;

namespace YoTennis.Controllers
{
    public class MatchController : Controller
    {
        private IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        public async Task<IActionResult> Restart()
        {
            await _matchService.Reset();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index()
        {
            var state = await _matchService.GetState();

            if (state.State == Models.MatchState.NotStarted)
                return View("NotStarted", new NotStartedModel { Match = state });

            if (state.State == Models.MatchState.Drawing)
                return View("Drawing", new DrawingModel { Match = state });

            if (state.State == Models.MatchState.BeginningGame)
                return View("BeginningGame", new BeginningGameModel { Match = state });

            if (state.State == Models.MatchState.PlayingGame)
                return View("PlayingGame", new PlayingGameModel { Match = state });

            if (state.State == Models.MatchState.ChangingSides)
                return View("ChangingSides", new ChangingSidesModel { Match = state });

            if (state.State == Models.MatchState.BeginningTiebreak)
                return View("BeginningTiebreak", new BeginningTiebreakModel { Match = state });

            if (state.State == Models.MatchState.PlayingTiebreak)
                return View("PlayingTiebreak", new PlayingTiebreakModel { Match = state });

            if (state.State == Models.MatchState.ChangingSidesOnTiebreak)
                return View("ChangingSidesOnTiebreak", new ChangingSidesOnTiebreakModel { Match = state });

            if (state.State == Models.MatchState.Completed)
                return View("Completed", new CompletedModel { Match = state });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Start(StartCommand command)
        {
            if (!ModelState.IsValid)
                return View("NotStarted", new NotStartedModel { Match = await _matchService.GetState(), Form = command });

            await _matchService.AddEvent(new StartEvent
            {
                OccuredAt = DateTime.UtcNow,
                Settings = new MatchSettings
                {
                    SetsForWin = command.SetsForWin,
                    GamesInSet = command.GamesInSet,
                    PointsInGame = command.PointsInGame,
                    PointsInTiebreak = command.PointsInTiebreak,
                    TiebreakFinal = command.TiebreakFinal
                },
                FirstPlayer = command.FirstPlayer,
                SecondPlayer = command.SecondPlayer
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
        public async Task<IActionResult> ServeFail(ServeFailCommand serveFailCommand)
        {
            await _matchService.AddEvent(new ServeFailEvent
            {
                OccuredAt = DateTime.UtcNow,
                Serve = ServeFailKind.Error
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> NetTouch(ServeFailCommand serveFailCommand)
        {
            await _matchService.AddEvent(new ServeFailEvent
            {
                OccuredAt = DateTime.UtcNow,
                Serve = ServeFailKind.NetTouch
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Ace(PointCommand pointCommand)
        {
            var state = await _matchService.GetState();

            await _matchService.AddEvent(new PointEvent
            {
                OccuredAt = DateTime.UtcNow,
                PlayerPoint = state.PlayerServes
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