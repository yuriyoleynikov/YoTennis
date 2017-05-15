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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace YoTennis.Controllers
{
    [Authorize]
    public class MatchController : Controller
    {
        private IMatchListService _matchListService;
        private string UserId => User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public MatchController(IMatchListService matchService)
        {
            _matchListService = matchService;
        }

        public Task<IActionResult> Restart()
        {
            return Create();
        }

        public async Task<IActionResult> Cancel(string id)
        {
            var matchService = await _matchListService.GetMatchService(UserId, id);

            await matchService.UndoAsync();
            return RedirectToAction(nameof(Index), new { id });
        }

        public async Task<IActionResult> Create()
        {
            var id = await _matchListService.CreateMatch(UserId);
            return RedirectToAction(nameof(Index), new { id } );
        }

        public async Task<IActionResult> Index(string id)
        {
            var matchService = await _matchListService.GetMatchService(UserId, id);

            var state = await matchService.GetStateAsync();

            if (state.State == Models.MatchState.NotStarted)
                return View("NotStarted", new NotStartedModel { Match = state, Id = id });

            if (state.State == Models.MatchState.Drawing)
                return View("Drawing", new DrawingModel { Match = state, Id = id });

            if (state.State == Models.MatchState.BeginningGame)
                return View("BeginningGame", new BeginningGameModel { Match = state, Id = id });

            if (state.State == Models.MatchState.PlayingGame)
                return View("PlayingGame", new PlayingGameModel { Match = state, Id = id });

            if (state.State == Models.MatchState.ChangingSides)
                return View("ChangingSides", new ChangingSidesModel { Match = state, Id = id });

            if (state.State == Models.MatchState.BeginningTiebreak)
                return View("BeginningTiebreak", new BeginningTiebreakModel { Match = state, Id = id });

            if (state.State == Models.MatchState.PlayingTiebreak)
                return View("PlayingTiebreak", new PlayingTiebreakModel { Match = state, Id = id });

            if (state.State == Models.MatchState.ChangingSidesOnTiebreak)
                return View("ChangingSidesOnTiebreak", new ChangingSidesOnTiebreakModel { Match = state, Id = id });

            if (state.State == Models.MatchState.Completed)
                return View("Completed", new CompletedModel { Match = state, Id = id });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Start(StartCommand command, string id)
        {
            var matchService = await _matchListService.GetMatchService(UserId, id);

            if (!ModelState.IsValid)
                return View("NotStarted", new NotStartedModel { Match = await matchService.GetStateAsync(), Form = command });

            await matchService.AddEventAsync(new StartEvent
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
                        
            return RedirectToAction(nameof(Index), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Draw(DrawCommand drawCommand, string id)
        {
            var matchService = await _matchListService.GetMatchService(UserId, id);

            await matchService.AddEventAsync(new DrawEvent
            {
                OccuredAt = DateTime.UtcNow,
                PlayerOnLeft = drawCommand.PlayerOnLeft,
                PlayerServes = drawCommand.PlayerServes
            });

            return RedirectToAction(nameof(Index), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> StartGame(StartGameCommand startGameCommand, string id)
        {
            var matchService = await _matchListService.GetMatchService(UserId, id);

            await matchService.AddEventAsync(new StartGameEvent
            {
                OccuredAt = DateTime.UtcNow
            });

            return RedirectToAction(nameof(Index), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> ServeFail(ClickCommand serveFailCommand, string id)
        {
            var matchService = await _matchListService.GetMatchService(UserId, id);

            await matchService.AddEventAsync(new ServeFailEvent
            {
                OccuredAt = DateTime.UtcNow,
                Serve = ServeFailKind.Error,
                ServeSpeed = serveFailCommand.ServeSpeed
            });

            return RedirectToAction(nameof(Index), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> NetTouch(ClickCommand serveFailCommand, string id)
        {
            var matchService = await _matchListService.GetMatchService(UserId, id);

            await matchService.AddEventAsync(new ServeFailEvent
            {
                OccuredAt = DateTime.UtcNow,
                Serve = ServeFailKind.NetTouch,
                ServeSpeed = serveFailCommand.ServeSpeed
            });

            return RedirectToAction(nameof(Index), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Ace(ClickCommand pointCommand, string id)
        {
            var matchService = await _matchListService.GetMatchService(UserId, id);

            var state = await matchService.GetStateAsync();

            await matchService.AddEventAsync(new PointEvent
            {
                OccuredAt = DateTime.UtcNow,
                PlayerPoint = state.PlayerServes,
                Kind = PointKind.Ace
            });

            return RedirectToAction(nameof(Index), new { id });
        }
        [HttpPost]
        public async Task<IActionResult> PointToFirst(ClickCommand pointCommand, string id)
        {
            var matchService = await _matchListService.GetMatchService(UserId, id);

            await matchService.AddEventAsync(new PointEvent
            {
                OccuredAt = DateTime.UtcNow,
                PlayerPoint = Player.First,
                Kind = PointKind.Unspecified
            });

            return RedirectToAction(nameof(Index), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> PointToSecond(ClickCommand pointCommand, string id)
        {
            var matchService = await _matchListService.GetMatchService(UserId, id);

            await matchService.AddEventAsync(new PointEvent
            {
                OccuredAt = DateTime.UtcNow,
                PlayerPoint = Player.Second,
                Kind = PointKind.Unspecified
            });

            return RedirectToAction(nameof(Index), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeSides(ChangeSidesCommand changeSidesCommand, string id)
        {
            var matchService = await _matchListService.GetMatchService(UserId, id);

            await matchService.AddEventAsync(new ChangeSidesGameEvent
            {
                OccuredAt = DateTime.UtcNow
            });

            return RedirectToAction(nameof(Index), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> StartTiebreak(StartTiebreakCommand startTiebreakCommand, string id)
        {
            var matchService = await _matchListService.GetMatchService(UserId, id);

            await matchService.AddEventAsync(new StartTiebreakEvent
            {
                OccuredAt = DateTime.UtcNow
            });

            return RedirectToAction(nameof(Index), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeSidesOnTiebreak(ChangeSidesOnTiebreakCommand changeSidesOnTiebreakCommand, string id)
        {
            var matchService = await _matchListService.GetMatchService(UserId, id);

            await matchService.AddEventAsync(new ChangeSidesOnTiebreakEvent
            {
                OccuredAt = DateTime.UtcNow
            });

            return RedirectToAction(nameof(Index), new { id });
        }        
    }
}