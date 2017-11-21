using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Models.Events;

namespace YoTennis.Models
{
    public class GameHandler
    {
        public List<GameEvent> Events { get; private set; } = new List<GameEvent>();
        public MatchModel CurrentState { get; private set; } = new MatchModel();
        public PlayersStatsMatchModel PlayersStats { get; private set; } = new PlayersStatsMatchModel();

        private void AddPoint(Player wonPlayer)
        {
            if (CurrentState.State == MatchState.PlayingGame)
            {
                var score = CurrentState.GameScore = CurrentState.GameScore + Score.ForPlayer(wonPlayer, 1);

                if ((score.FirstPlayer >= CurrentState.MatchSettings.PointsInGame && score.SecondPlayer + 1 < score.FirstPlayer)
                    || (score.SecondPlayer >= CurrentState.MatchSettings.PointsInGame && score.FirstPlayer + 1 < score.SecondPlayer))
                    PlayerWonGame(wonPlayer);
                else
                {
                    CurrentState.ServePosition = CurrentState.ServePosition.Other();
                    CurrentState.SecondServe = false;
                }
            }
            else if (CurrentState.State == MatchState.PlayingTiebreak)
            {
                var set = CurrentState.Sets.Count - 1;
                var score = CurrentState.Sets[set].TiebreakScore =
                    CurrentState.Sets[set].TiebreakScore + Score.ForPlayer(wonPlayer, 1);

                if ((score.FirstPlayer >= CurrentState.MatchSettings.PointsInTiebreak && score.SecondPlayer + 1 < score.FirstPlayer) ||
                    (score.SecondPlayer >= CurrentState.MatchSettings.PointsInTiebreak && score.FirstPlayer + 1 < score.SecondPlayer))
                {
                    CurrentState.Sets[set].Score = CurrentState.Sets[set].Score + Score.ForPlayer(wonPlayer, 1);
                    CurrentState.PlayerServes = CurrentState.PlayerServes.Other();
                    CurrentState.ServePosition = ServePosition.Right;
                    CurrentState.State = MatchState.ChangingSides;

                    PlayerWonSet(wonPlayer);
                }
                else
                {
                    CurrentState.ServePosition = CurrentState.ServePosition.Other();
                    CurrentState.SecondServe = false;

                    if ((score.FirstPlayer + score.SecondPlayer) % 2 != 0)
                    {
                        CurrentState.PlayerServes = CurrentState.PlayerServes.Other();
                    }

                    if ((score.FirstPlayer + score.SecondPlayer) % 6 == 0)
                    {
                        CurrentState.State = MatchState.ChangingSidesOnTiebreak;
                    }
                }
            }
            else
                throw new InvalidOperationException("Not Expected");
        }

        public void UndoLastEvent()
        {
            if (Events.Count <= 0)
                throw new InvalidOperationException("No events to undo");

            var events = Events;

            Events = new List<GameEvent>(events.Count);
            CurrentState = new MatchModel();

            foreach (var ev in events.Take(events.Count - 1))
            {
                AddEvent(ev);
            }
        }

        private void PlayerWonGame(Player wonPlayer)
        {
            var currentSetIndex = CurrentState.Sets.Count - 1;

            var currentSet = CurrentState.Sets[currentSetIndex];
            var currentSetScore = currentSet.Score;

            currentSetScore = currentSetScore + Score.ForPlayer(wonPlayer, 1);
            currentSet.Score = currentSetScore;

            if ((currentSetScore.FirstPlayer >= CurrentState.MatchSettings.GamesInSet && currentSetScore.SecondPlayer + 1 < currentSetScore.FirstPlayer)
                || (currentSetScore.SecondPlayer >= CurrentState.MatchSettings.GamesInSet && currentSetScore.FirstPlayer + 1 < currentSetScore.SecondPlayer) ||
                (currentSetScore.FirstPlayer == CurrentState.MatchSettings.GamesInSet + 1) ||
                (currentSetScore.SecondPlayer == CurrentState.MatchSettings.GamesInSet + 1))
            {
                PlayerWonSet(wonPlayer);
                if (CurrentState.State != MatchState.Completed)
                {
                    CurrentState.State = MatchState.BeginningGame;
                    CurrentState.ServePosition = ServePosition.Right;
                    CurrentState.SecondServe = false;
                    CurrentState.GameScore = new Score();
                }
            }
            else if (currentSetScore.FirstPlayer == CurrentState.MatchSettings.GamesInSet &&
                currentSetScore.SecondPlayer == CurrentState.MatchSettings.GamesInSet)
            {
                CurrentState.PlayerServes = CurrentState.PlayerServes.Other();
                CurrentState.ServePosition = ServePosition.Right;
                CurrentState.SecondServe = false;
                CurrentState.GameScore = new Score();

                CurrentState.State = MatchState.BeginningTiebreak;
            }
            else
            {
                CurrentState.State = MatchState.BeginningGame;
                CurrentState.ServePosition = ServePosition.Right;
                CurrentState.SecondServe = false;
                CurrentState.GameScore = new Score();
            }

            if ((CurrentState.State != MatchState.Completed) &&
                (CurrentState.State != MatchState.BeginningTiebreak) &&
                (CurrentState.State != MatchState.PlayingTiebreak))
            {
                CurrentState.PlayerServes = CurrentState.PlayerServes.Other();
                if ((currentSetScore.FirstPlayer + currentSetScore.SecondPlayer) % 2 != 0)
                {
                    CurrentState.State = MatchState.ChangingSides;
                }
            }
        }

        private void PlayerWonSet(Player wonPlayer)
        {
            CurrentState.MatchScore = CurrentState.MatchScore + Score.ForPlayer(wonPlayer, 1);

            if ((CurrentState.MatchSettings.SetsForWin == 3) &&
                ((CurrentState.MatchScore.FirstPlayer == 2) || (CurrentState.MatchScore.SecondPlayer == 2)))
                CurrentState.State = MatchState.Completed;
            else if ((CurrentState.MatchSettings.SetsForWin == 5) &&
                ((CurrentState.MatchScore.FirstPlayer == 3) || (CurrentState.MatchScore.SecondPlayer == 3)))
                CurrentState.State = MatchState.Completed;
            else
            {
                CurrentState.Sets.Add(new SetModel());
            }
        }

        public void AddEvent(GameEvent gameEvent)
        {
            ((dynamic)this).On((dynamic)gameEvent);

            Events.Add(gameEvent);
        }
        
        private void On(StartEvent gameEvent)
        {

            if (CurrentState.State != MatchState.NotStarted)
                throw new InvalidOperationException("Not Expected");


            CurrentState.MatchSettings = gameEvent.Settings;
            CurrentState.FirstPlayer = gameEvent.FirstPlayer;
            CurrentState.SecondPlayer = gameEvent.SecondPlayer;
            CurrentState.MatchStartedAt = gameEvent.OccuredAt;

            CurrentState.FirstPlayerUserId = gameEvent.FirstPlayerUserId;
            CurrentState.SecondPlayerUserId = gameEvent.SecondPlayerUserId;

            CurrentState.State = MatchState.Drawing;
        }

        private void On(DrawEvent gameEvent)
        {
            if (CurrentState.State != MatchState.Drawing)
                throw new InvalidOperationException("Not Expected");

            CurrentState.PlayerOnLeft = gameEvent.PlayerOnLeft;
            CurrentState.PlayerServes = gameEvent.PlayerServes;
            CurrentState.Sets = new List<SetModel> { new SetModel() };
            CurrentState.GameScore = new Score();
            CurrentState.SecondServe = false;
            CurrentState.ServePosition = ServePosition.Right;
            CurrentState.GameStratedAt = gameEvent.OccuredAt;
            CurrentState.State = MatchState.BeginningGame;
        }

        private void On(ChangeSidesGameEvent gameEvent)
        {
            if (CurrentState.State != MatchState.ChangingSides)
                throw new InvalidOperationException("Not Expected");

            CurrentState.PlayerOnLeft = CurrentState.PlayerOnLeft.Other();
            CurrentState.State = MatchState.BeginningGame;
        }

        private void On(ChangeSidesOnTiebreakEvent gameEvent)
        {
            if (CurrentState.State != MatchState.ChangingSidesOnTiebreak)
                throw new InvalidOperationException("Not Expected");

            CurrentState.PlayerOnLeft = CurrentState.PlayerOnLeft.Other();
            CurrentState.State = MatchState.PlayingTiebreak;
        }

        private void On(ServeFailEvent gameEvent)
        {
            AddPointStats(gameEvent);

            if (gameEvent.Serve == ServeFailKind.Error)
                if (!CurrentState.SecondServe)
                    CurrentState.SecondServe = true;
                else
                {
                    var player = CurrentState.PlayerServes;

                    if (player == Player.First)
                        player = Player.Second;
                    else
                        player = Player.First;

                    AddPoint(player);
                }
        }

        private void On(PointEvent gameEvent)
        {
            if (!(CurrentState.State == MatchState.PlayingGame || CurrentState.State == MatchState.PlayingTiebreak))
                throw new InvalidOperationException("Not Expected");

            AddPointStats(gameEvent);

            AddPoint(gameEvent.PlayerPoint);
        }

        private void AddPointStats(ServeFailEvent gameEvent)
        {
            var (playerServes, playerReceiving) = CurrentState.PlayerServes == Player.First
                ? (PlayersStats.FirstPlayer, PlayersStats.SecondPlayer)
                : (PlayersStats.SecondPlayer, PlayersStats.FirstPlayer);

            if (gameEvent.Serve == ServeFailKind.Error)
            {
                if (!CurrentState.SecondServe)
                    playerServes.FirstServe++;
                else
                {
                    playerServes.SecondServe++;
                    playerServes.DoubleFaults++;
                    playerReceiving.TotalPoints++;
                }
            }
        }

        private void AddPointStats(PointEvent gameEvent)
        {
            var (wonPlayer, lostPlayer) = gameEvent.PlayerPoint == Player.First
                ? (PlayersStats.FirstPlayer, PlayersStats.SecondPlayer)
                : (PlayersStats.SecondPlayer, PlayersStats.FirstPlayer);

            var playerServes = CurrentState.PlayerServes == Player.First
                            ? PlayersStats.FirstPlayer
                            : PlayersStats.SecondPlayer;

            wonPlayer.TotalPoints++;

            switch (gameEvent.Kind)
            {
                case PointKind.Ace:
                    wonPlayer.Ace++;
                    break;

                case PointKind.Error:
                    lostPlayer.Error++;
                    break;

                case PointKind.Backhand:
                    wonPlayer.Backhand++;
                    break;

                case PointKind.DoubleFaults:
                    lostPlayer.DoubleFaults++;
                    break;

                case PointKind.Forehand:
                    wonPlayer.Forehand++;
                    break;

                case PointKind.NetPoint:
                    wonPlayer.NetPoint++;
                    break;

                case PointKind.UnforcedError:
                    lostPlayer.UnforcedError++;
                    break;

                case PointKind.Unspecified:
                    break;

                default:
                    break;
            }

            if (!CurrentState.SecondServe)
            {
                playerServes.FirstServe++;

                if (gameEvent.PlayerPoint == CurrentState.PlayerServes)
                    playerServes.WonOnFirstServe++;

                if (gameEvent.Kind != PointKind.DoubleFaults)
                    playerServes.FirstServeSuccessful++;
            }
            else
            {
                playerServes.SecondServe++;

                if (gameEvent.PlayerPoint == CurrentState.PlayerServes)
                    playerServes.WonOnSecondServe++;

                if (gameEvent.Kind != PointKind.DoubleFaults)
                    playerServes.SecondServeSuccessful++;
            }
        }

        private void On(StartTiebreakEvent gameEvent)
        {
            if (CurrentState.State != MatchState.BeginningTiebreak)
                throw new InvalidOperationException("Not Expected");
            
            CurrentState.GameStratedAt = gameEvent.OccuredAt;
            CurrentState.State = MatchState.PlayingTiebreak;
        }

        private void On(StartGameEvent gameEvent)
        {
            if (CurrentState.State != MatchState.BeginningGame)
                throw new InvalidOperationException("Not Expected");

            CurrentState.GameStratedAt = gameEvent.OccuredAt;
            CurrentState.State = MatchState.PlayingGame;

        }

        private void On(StopEvent gameEvent)
        {
            if (CurrentState.State == MatchState.Completed || CurrentState.State == MatchState.CompletedAndNotFinished)
                throw new InvalidOperationException("Not Expected");

            CurrentState.State = MatchState.CompletedAndNotFinished;            
        }

        private void On(ChangePlayersEvent gameEvent)
        {
            if (CurrentState.State == MatchState.NotStarted)
                throw new InvalidOperationException("Not Expected");
            
            if (gameEvent.FirstPlayer != null)
                CurrentState.FirstPlayer = gameEvent.FirstPlayer;

            if (gameEvent.SecondPlayer != null)
                CurrentState.SecondPlayer = gameEvent.SecondPlayer;

            if (gameEvent.FirstPlayerUserId != null)
                    CurrentState.FirstPlayerUserId = gameEvent.FirstPlayerUserId;

            if (gameEvent.SecondPlayerUserId != null)
                    CurrentState.SecondPlayerUserId = gameEvent.SecondPlayerUserId;
        }

        private void On(DeletePlayersEvent gameEvent)
        {
            if (CurrentState.State == MatchState.NotStarted)
                throw new InvalidOperationException("Not Expected");
            
            if (gameEvent.FirstPlayerUserId)
                CurrentState.FirstPlayerUserId = null;

            if (gameEvent.SecondPlayerUserId)
                CurrentState.SecondPlayerUserId = null;
        }
    }
}
