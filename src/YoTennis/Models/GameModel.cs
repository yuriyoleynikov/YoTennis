using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{

    public class GameModel
    {
        public List<GameEvent> Events { get; private set; }
        public State CurrentState { get; private set; }

        public void AddPoint(Player GetPointPlayer, PointKind PointKind, ServeSpeed ServeSpeed)
        {
            //AddPoint
            /*
            if (GetPointPlayer == Player.First)
            {
                CurrentState.ScoreInGame.Score.FirstPlayer ++;
            }
            else
            {
                CurrentState.ScoreInGame.Score.SecondPlayer++;
            }

            */
        }

        public void AddEvent(GameEvent gameEvent)
        {
            ((dynamic)this).On((dynamic)gameEvent);

            Events.Add(gameEvent);
        }

        private void On(StartEvent gameEvent)
        {
            CurrentState = new State
            {
                MatchSettings = gameEvent.Settings,
                FirstPlayer = gameEvent.FirstPlayer,
                SecondPlayer = gameEvent.SecondPlayer,
                MatchDate = gameEvent.OccuredAt,
                MatchState = MatchState.Drawing,
            };
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
            /*
            if (CurrentState.PlayerOnLeft == Player.First)
                CurrentState.PlayerOnLeft = Player.Second;
            else
                CurrentState.PlayerOnLeft = Player.First;
                */
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

                    AddPoint(player, PointKind.DoubleFaults, gameEvent.ServeSpeed);
                }
        }

        private void On(PointEvent gameEvent)
        {
            AddPoint(gameEvent.PlayerPoint, gameEvent.Kind, gameEvent.ServeSpeed);
        }

        private void On(EndGameEvent gameEvent)
        {
            //
        }
        private void On(StartGameEvent gameEvent)
        {
            //empty
        }
        private void On(CancelGame gameEvent)
        {
        }
    }
}
