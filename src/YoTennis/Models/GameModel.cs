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

        public void AddPoint(bool firstPlayer)
        {
            //AddPoint
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
            CurrentState.MatchState = MatchState.Playing;
            CurrentState.ScoreInSets = new List<Set> { new Set() };
            CurrentState.ScoreInGame = new Game();
            CurrentState.SecondServe = false;
            CurrentState.ServePositionOnTheCenter = ServePositionOnTheCenter.Right;
            CurrentState.TheGameTime = gameEvent.OccuredAt;
        }        

        private void On(ChangeSidesGame gameEvent)
        {
            CurrentState
        }

        private void On(ChangeServeGame gameEvent)
        {
            CurrentState.Score.InTheGame.FirstPlayerServing = !CurrentState.Score.InTheGame.FirstPlayerServing;
        }

        private void On(ServeFailEvent gameEvent)
        {
            if (CurrentState.Score.InTheGame.Serve == Serve.First)
                CurrentState.Score.InTheGame.Serve = Serve.Second;
            else AddPoint(!CurrentState.Score.InTheGame.FirstPlayerServing);
        }

        private void On(ServeNetTouchEvent gameEvent)
        {

        }
        private void On(PointEvent gameEvent)
        {

        }

        private void On(EndGameEvent gameEvent)
        {
        }
        private void On(StartGameEvent gameEvent)
        {

        }
        private void On(CancelGame gameEvent)
        {
        }
    }
}
