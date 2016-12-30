using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Models.Events;

namespace YoTennis.Models
{

    public class GameModel
    {
        public List<GameEvent> Events { get; private set; }
        public State CurrentState { get; private set; } = new State();

        private void AddPoint(Player wonPlayer)
        {
            if (CurrentState.MatchState == MatchState.PlayingGame)
            {
                var score = CurrentState.Game.Score = CurrentState.Game.Score + Score.ForPlayer(wonPlayer, 1);

                if ((score.FirstPlayer >= CurrentState.MatchSettings.PointsInGame && score.SecondPlayer + 1 < score.FirstPlayer)
                    || (score.SecondPlayer >= CurrentState.MatchSettings.PointsInGame && score.FirstPlayer + 1 < score.SecondPlayer))
                    PlayerWonGame(wonPlayer);
                else
                {
                    CurrentState.ServePosition = CurrentState.ServePosition.Other();
                    CurrentState.SecondServe = false;
                }
            }
            else if (CurrentState.MatchState == MatchState.PlayingTiebreak)
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
                    CurrentState.MatchState = MatchState.ChangingSides;

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
                        CurrentState.MatchState = MatchState.ChangingSidesOnTiebreak;
                    }
                }
            }
            else
                throw new InvalidOperationException("Not Expected");
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
                if (CurrentState.MatchState != MatchState.Completed)
                {
                    CurrentState.MatchState = MatchState.BeginGame;
                    CurrentState.ServePosition = ServePosition.Right;
                    CurrentState.SecondServe = false;
                    CurrentState.Game = new Game();
                }
            }
            else if (currentSetScore.FirstPlayer == CurrentState.MatchSettings.GamesInSet &&
                currentSetScore.SecondPlayer == CurrentState.MatchSettings.GamesInSet)
            {
                CurrentState.PlayerServes = CurrentState.PlayerServes.Other();
                CurrentState.ServePosition = ServePosition.Right;                
                CurrentState.SecondServe = false;
                CurrentState.Game = new Game();

                CurrentState.MatchState = MatchState.BeginTiebreak;
            }
            else
            {
                CurrentState.MatchState = MatchState.BeginGame;
                CurrentState.ServePosition = ServePosition.Right;
                CurrentState.SecondServe = false;
                CurrentState.Game = new Game();
            }

            if ((CurrentState.MatchState != MatchState.Completed) &&
                (CurrentState.MatchState != MatchState.BeginTiebreak) &&
                (CurrentState.MatchState != MatchState.PlayingTiebreak))
            {
                CurrentState.PlayerServes = CurrentState.PlayerServes.Other();
                if ((currentSetScore.FirstPlayer + currentSetScore.SecondPlayer) % 2 != 0)
                {
                    CurrentState.MatchState = MatchState.ChangingSides;
                }
            }
        }

        private void PlayerWonSet(Player wonPlayer)
        {
            CurrentState.SetScore = CurrentState.SetScore + Score.ForPlayer(wonPlayer, 1);

            if ((CurrentState.MatchSettings.SetsForWin == 3) &&
                ((CurrentState.SetScore.FirstPlayer == 2) || (CurrentState.SetScore.SecondPlayer == 2)))
                CurrentState.MatchState = MatchState.Completed;
            else if ((CurrentState.MatchSettings.SetsForWin == 5) &&
                ((CurrentState.SetScore.FirstPlayer == 3) || (CurrentState.SetScore.SecondPlayer == 3)))
                CurrentState.MatchState = MatchState.Completed;
            else
            {
                CurrentState.Sets.Add(new Set());
                
            }
        }

        public void AddEvent(GameEvent gameEvent)
        {
            ((dynamic)this).On((dynamic)gameEvent);

            Events.Add(gameEvent);
        }

        private void On(StartEvent gameEvent)
        {

            if (CurrentState.MatchState != MatchState.NotStarted)
                throw new InvalidOperationException("Not Expected");


            CurrentState.MatchSettings = gameEvent.Settings;
            CurrentState.FirstPlayer = gameEvent.FirstPlayer;
            CurrentState.SecondPlayer = gameEvent.SecondPlayer;
            CurrentState.MatchStartedAt = gameEvent.OccuredAt;
            CurrentState.MatchState = MatchState.Drawing;

            Events = new List<GameEvent>();
        }

        private void On(DrawEvent gameEvent)
        {
            CurrentState.PlayerOnLeft = gameEvent.PlayerOnLeft;
            CurrentState.PlayerServes = gameEvent.PlayerServes;
            CurrentState.Sets = new List<Set> { new Set() };
            CurrentState.Game = new Game();
            CurrentState.SecondServe = false;
            CurrentState.ServePosition = ServePosition.Right;
            CurrentState.GameStratedAt = gameEvent.OccuredAt;
            CurrentState.MatchState = MatchState.BeginGame;
        }

        private void On(ChangeSidesGameEvent gameEvent)
        {
            CurrentState.PlayerOnLeft = CurrentState.PlayerOnLeft.Other();
            CurrentState.MatchState = MatchState.BeginGame;
        }

        private void On(ChangeSidesOnTiebreakEvent gameEvent)
        {
            CurrentState.PlayerOnLeft = CurrentState.PlayerOnLeft.Other();
            CurrentState.MatchState = MatchState.PlayingTiebreak;
        }

        private void On(ServeFailEvent gameEvent)
        {
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
            AddPoint(gameEvent.PlayerPoint);
        }

        private void On(StartTiebreakEvent gameEvent)
        {
            CurrentState.GameStratedAt = gameEvent.OccuredAt;
            if (CurrentState.MatchState == MatchState.BeginTiebreak)
            {
                CurrentState.MatchState = MatchState.PlayingTiebreak;                
            }
        }

        private void On(StartGameEvent gameEvent)
        {
            if (CurrentState.MatchState != MatchState.BeginGame)
                throw new InvalidOperationException("Not Expected");

            CurrentState.GameStratedAt = gameEvent.OccuredAt;
            CurrentState.MatchState = MatchState.PlayingGame;

        }

        private void On(CancelGameEvent gameEvent)
        {
        }
    }
}
