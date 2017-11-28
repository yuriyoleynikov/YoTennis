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
using Microsoft.AspNetCore.Identity;
using YoTennis.Helpers;

namespace YoTennis.Controllers
{
    [Authorize]
    public class MatchController : Controller
    {
        private IMatchListService _matchListService;
        private string UserId => User.FindFirst(ClaimTypes.NameIdentifier).Value;
        private readonly UserManager<ApplicationUser> _userManager;

        public MatchController(UserManager<ApplicationUser> userManager, IMatchListService matchListService)
        {
            _userManager = userManager;
            _matchListService = matchListService;
        }

        public async Task<IActionResult> AddPastMatch()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPastMatch(AddPostMatchViewModel addMatchViewModel)
        {
            var matchScore = new List<SetModel>();
            if (ModelState.IsValid)
            {
                if (addMatchViewModel.FirstPlayerUserId && addMatchViewModel.SecondPlayerUserId)
                {
                    ModelState.AddModelError(nameof(addMatchViewModel.FirstPlayerUserId), "First Player UserId is on");
                    ModelState.AddModelError(nameof(addMatchViewModel.SecondPlayerUserId), "Second Player UserId is on");
                }

                matchScore.Add(new SetModel
                    {
                        Score = new Score {
                            FirstPlayer = addMatchViewModel.FirstPlayerSet1,
                            SecondPlayer = addMatchViewModel.SecondPlayerSet1 }
                    });

                if (addMatchViewModel.FirstPlayerSet2 != null)
                    matchScore.Add(new SetModel
                    {
                        Score = new Score
                        {
                            FirstPlayer = addMatchViewModel.FirstPlayerSet2 ?? 0,
                            SecondPlayer = addMatchViewModel.SecondPlayerSet2 ?? 0
                        }
                    });

                if (addMatchViewModel.FirstPlayerSet3 != null)
                    matchScore.Add(new SetModel
                    {
                        Score = new Score
                        {
                            FirstPlayer = addMatchViewModel.FirstPlayerSet3 ?? 0,
                            SecondPlayer = addMatchViewModel.SecondPlayerSet3 ?? 0
                        }
                    });
            }

            if (!ModelState.IsValid)
                return View(addMatchViewModel);

            var matchId = await _matchListService.CreateMatch(UserId);
            var matchService = await _matchListService.GetMatchService(UserId, matchId);

            await matchService.AddEventAsync(new PastMatchEvent
            {
                OccuredAt = DateTime.UtcNow,
                Date = addMatchViewModel.Date,
                FirstPlayer = addMatchViewModel.FirstPlayer,
                SecondPlayer = addMatchViewModel.SecondPlayer,
                FirstPlayerUserId = addMatchViewModel.FirstPlayerUserId ? UserId : null,
                SecondPlayerUserId = addMatchViewModel.SecondPlayerUserId ? UserId : null,
                MatchScore = matchScore
            });

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var match = await _matchListService.GetMatchService(UserId, id);
                var matchState = await match.GetStateAsync();
                var matchDetailsViewModel = new MatchDetailsViewModel
                {
                    Id = id,
                    MatchModel = matchState,
                    UserId = UserId
                };

                return View(matchDetailsViewModel);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> PlayerToUser(string id, Player player)
        {
            var matchService = await _matchListService.GetMatchService(UserId, id);

            var e = new ChangePlayersEvent()
            {
                OccuredAt = DateTime.UtcNow
            };

            if (player == Player.First)
                e.FirstPlayerUserId = UserId;
            else
                e.SecondPlayerUserId = UserId;

            await matchService.AddEventAsync(e);

            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> PlayerToUserDelete(string id, Player player)
        {
            var matchService = await _matchListService.GetMatchService(UserId, id);

            var e = new DeletePlayersEvent()
            {
                OccuredAt = DateTime.UtcNow
            };

            if (player == Player.First)
                e.FirstPlayerUserId = true;
            else
                e.SecondPlayerUserId = true;

            await matchService.AddEventAsync(e);

            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Share(string id)
        {
            var applicationUser = await _userManager.FindByIdAsync(UserId);
            var date = DateTime.UtcNow.ToBinary().ToString();
            var matchSHA = SHA.GenerateSHA256String(id + date + applicationUser.PasswordHash);
            var link = "/match/shared/" + id + "?date=" + date + "&hash=" + matchSHA;

            var match = await _matchListService.GetMatchService(UserId, id);
            var matchState = await match.GetStateAsync();

            return View(new MatchShareDetailsViewModel { Id = id, MatchModel = matchState, Shared = link });
        }

        public async Task<IActionResult> Shared(string id, long date, string hash)
        {
            var userId = await _matchListService.GetMatchOwner(id);
            var applicationUser = await _userManager.FindByIdAsync(userId);
            var matchSHA = SHA.GenerateSHA256String(id + date.ToString() + applicationUser.PasswordHash);

            if (hash == matchSHA && DateTime.UtcNow - DateTime.FromBinary(date) <= TimeSpan.FromDays(1))
            {
                var match = await _matchListService.GetMatchService(userId, id);

                var matchState = await match.GetStateAsync();
                var matchSharedDetailsViewModel = new MatchSharedDetailsViewModel
                {
                    Id = id,
                    Date = date,
                    Hash = hash,
                    MatchModel = matchState
                };

                return View(matchSharedDetailsViewModel);
            }

            return View();
        }

        public async Task<IActionResult> CopyMatch(string id, string hash, long date)
        {
            var userId = await _matchListService.GetMatchOwner(id);
            var applicationUser = await _userManager.FindByIdAsync(userId);
            var matchSHA = SHA.GenerateSHA256String(id + date.ToString() + applicationUser.PasswordHash);

            if (hash == matchSHA && DateTime.UtcNow - DateTime.FromBinary(date) <= TimeSpan.FromDays(1))
            {
                var matchId = await _matchListService.CopyMatch(UserId, id);

                return RedirectToAction(nameof(Details), new { id = matchId });
            }

            throw new KeyNotFoundException("hash == matchSHA && DateTime.UtcNow - DateTime.FromBinary(date) <= TimeSpan.FromDays(1)");
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
            return RedirectToAction(nameof(Index), new { id });
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

            if (state.State == Models.MatchState.CompletedAndNotFinished)
                return View("CompletedAndNotFinished", new CompletedAndNotFinishedModel { Match = state, Id = id });

            return View();
        }

        public async Task<IActionResult> Stats(string id)
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
                Kind = pointCommand.Kind
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
                Kind = pointCommand.Kind
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

        public async Task<IActionResult> Stop(StopGameCommand stopGameCommand, string id)
        {
            var matchService = await _matchListService.GetMatchService(UserId, id);

            await matchService.AddEventAsync(new StopEvent
            {
                OccuredAt = DateTime.UtcNow
            });

            return RedirectToAction(nameof(Index), new { id });
        }
    }
}