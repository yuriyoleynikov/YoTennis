using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{

    public class GameModel
    {
        public List<GameEvent> Events { get; private set; }
        public State CurrentState { get; private set; } = new State();

        public void AddPoint(Player getPointPlayer)
        {
            var score = CurrentState.ScoreInGame.Score = CurrentState.ScoreInGame.Score + PlayerScore.ForPlayer(getPointPlayer, 1);
            if (score.FirstPlayer >= 4 && score.SecondPlayer + 1 < score.FirstPlayer)
            {
                PlayerWonGame(Player.First);
            }
            else if (score.SecondPlayer >= 4 && score.FirstPlayer + 1 < score.SecondPlayer)
            {
                PlayerWonGame(Player.Second);
            }
            else
            {
                CurrentState.ServePositionOnTheCenter = CurrentState.ServePositionOnTheCenter == ServePositionOnTheCenter.Left ?
                    ServePositionOnTheCenter.Right : ServePositionOnTheCenter.Left;
                CurrentState.SecondServe = false;
            }
        }

        private void PlayerWonGame(Player player)
        {
            var currentSetIndex = CurrentState.ScoreInSets.Count - 1;

            var currentSet = CurrentState.ScoreInSets[currentSetIndex];
            var currentSetScore = currentSet.Score;

            currentSetScore = currentSetScore + PlayerScore.ForPlayer(player, 1);
            currentSet.Score = currentSetScore;

            if (currentSetScore.FirstPlayer >= 6 && currentSetScore.SecondPlayer + 1 < currentSetScore.FirstPlayer)
            {
                PlayerWonSet(Player.First);
            }
            else if (currentSetScore.SecondPlayer >= 6 && currentSetScore.FirstPlayer + 1 < currentSetScore.SecondPlayer)
            {
                PlayerWonSet(Player.Second);
            }
            else
            {
                CurrentState.ServePositionOnTheCenter = ServePositionOnTheCenter.Right;
                CurrentState.SecondServe = false;
                CurrentState.ScoreInGame = new Game();
                CurrentState.PlayerServes = CurrentState.PlayerServes == Player.First ? Player.Second : Player.First;
            }

            if ((currentSetScore.FirstPlayer + currentSetScore.SecondPlayer) % 2 != 0)
            {
                CurrentState.PlayerOnLeft = CurrentState.PlayerOnLeft == Player.First ? Player.Second : Player.First;
                CurrentState.ChangeSides = true;
                CurrentState.MatchState = MatchState.ChangingSides;
            }
        }

        private void PlayerWonSet(Player first)
        {

            CurrentState.ScoreInSets.Add(new Set());

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
            CurrentState.MatchDate = gameEvent.OccuredAt;
            CurrentState.MatchState = MatchState.Drawing;

            Events = new List<GameEvent>();
        }

        private void On(DrawEvent gameEvent)
        {
            CurrentState.PlayerOnLeft = gameEvent.PlayerOnLeft;
            CurrentState.PlayerServes = gameEvent.PlayerServes;
            CurrentState.ScoreInSets = new List<Set> { new Set() };
            CurrentState.ScoreInGame = new Game();
            CurrentState.SecondServe = false;
            CurrentState.ServePositionOnTheCenter = ServePositionOnTheCenter.Right;
            CurrentState.GameTime = gameEvent.OccuredAt;
            CurrentState.MatchState = MatchState.Playing;
        }

        private void On(ChangeSidesGame gameEvent)
        {
            CurrentState.ChangeSides = false;
            CurrentState.MatchState = MatchState.Playing;
        }

        private void On(ChangeServeGame gameEvent)
        {
            /*
            if (CurrentState.PlayerServes == Player.First)
                CurrentState.PlayerServes = Player.Second;
            else
                CurrentState.PlayerServes = Player.First;
                */
        }

        private void On(ServeFailEvent gameEvent)
        {
            if (gameEvent.Serve == ServeFailKind.Error)
                if (CurrentState.SecondServe == false)
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

        private void On(EndGameEvent gameEvent)
        {
            //
        }
        private void On(StartGameEvent gameEvent)
        {
            CurrentState.GameTime = gameEvent.OccuredAt;
        }
        private void On(CancelGame gameEvent)
        {
        }
    }
}
