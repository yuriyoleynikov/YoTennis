using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{

    public class GameModel
    {
        public List<GameEvent> Events { get; private set; }
        public StateData StateData { get; private set; }

        public void AddPoint(bool firstPlayer)
        {
            //AddPoint


        }

        public void AddEvent(GameEvent gameEvent)
        {
            ((dynamic)this).On((dynamic)gameEvent);

            Events.Add(gameEvent);
        }

        private void On(StartGameEvent gameEvent)
        {
            StateData = new StateData
            {
                Settings = gameEvent.Settings,
                FirstPlayer = gameEvent.FirstPlayer,
                SecondPlayer = gameEvent.SecondPlayer,
                Date = gameEvent.OccuredAt,
                State = GameState.Drawing
            };
            Events = new List<GameEvent>();
        }

        private void On(DrawEvent gameEvent)
        {
            StateData.FirstPlayerOnLeft = gameEvent.FirstPlayerOnLeft;
            StateData.FirstPlayerServes = gameEvent.FirstPlayerServes;
            StateData.State = GameState.Playing;
        }

        private void On(TakeSidesEvent gameEvent)
        {
            StateData.Score.InSets.FirstPlayer = 0;
            StateData.Score.InSets.SecondPlayer = 0;

            StateData.Score.InTheSet.FirstPlayer = 0;
            StateData.Score.InTheSet.SecondPlayer = 0;

            StateData.Score.OnTieBreak.FirstPlayer = 0;
            StateData.Score.OnTieBreak.SecondPlayer = 0;

            StateData.Score.InTheGame.FirstPlayer = EnumInTheGame.love;
            StateData.Score.InTheGame.SecondPlayer = EnumInTheGame.love;

            StateData.Score.InTheGame.FirstPlayerServing = StateData.FirstPlayerServes;
            StateData.Score.InTheGame.Serve = Serve.First;

            StateData.Score.InTheGame.FirstPlayerOnLeft = StateData.FirstPlayerOnLeft;
        }

        private void On(ChangeSidesGame gameEvent)
        {
            StateData.Score.InTheGame.FirstPlayerOnLeft = !StateData.Score.InTheGame.FirstPlayerOnLeft;
        }

        private void On(ChangeServeGame gameEvent)
        {
            StateData.Score.InTheGame.FirstPlayerServing = !StateData.Score.InTheGame.FirstPlayerServing;
        }

        private void On(ServeFailEvent gameEvent)
        {
            if (StateData.Score.InTheGame.Serve == Serve.First)
                StateData.Score.InTheGame.Serve = Serve.Second;
            else AddPoint(!StateData.Score.InTheGame.FirstPlayerServing);
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

    }
    public class ServeFailEvent
    {
        // Implementation is not required
    }

    public class TakeSidesEvent : GameEvent
    {
        public Score Score { get; set; }
    }

    public class EndGameEvent : GameEvent
    {
        //
    }

    public class ServeNetTouchEvent : GameEvent
    {

    }

    public class ChangeServeGame : GameEvent
    {

    }

    public class ChangeSidesGame : GameEvent
    {

    }

    public class StartGameEvent : GameEvent
    {
        public GameSettings Settings { get; set; }
        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }
    }

    public class StateData
    {
        public GameSettings Settings { get; set; }
        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }
        public DateTime Date { get; set; }
        public Score Score { get; set; }
        public bool FirstPlayerOnLeft { get; set; }
        public bool FirstPlayerServes { get; set; }
        public GameState State { get; set; }
    }

    public enum GameState
    {
        NotStarted,
        Drawing,
        Playing,
        Comleted
    }

    public class Score
    {
        public InSets InSets { get; set; }
        public OnTieBreak OnTieBreak { get; set; }
        public InTheSet InTheSet { get; set; }
        public InTheGame InTheGame { get; set; }
    }

    public class InSets
    {
        public int FirstPlayer { get; set; }
        public int SecondPlayer { get; set; }
    }

    public enum Serve { First, NetTouchOnFirst, Second }

    public class OnTieBreak
    {
        public int FirstPlayer { get; set; }
        public int SecondPlayer { get; set; }
    }

    public class InTheSet
    {
        public int FirstPlayer { get; set; }
        public int SecondPlayer { get; set; }
    }
    public class InTheGame
    {
        public EnumInTheGame FirstPlayer { get; set; }
        public EnumInTheGame SecondPlayer { get; set; }
        public bool FirstPlayerServing { get; set; }
        public Serve Serve { get; set; }
        public bool FirstPlayerOnLeft { get; set; }
    }

    public enum EnumInTheGame { love, fifteen, thirty, forty, advantage, game }

    public class GameSettings
    {
        public int SetsForWin { get; set; }
        public bool TieBreakFinal { get; set; }
    }

    public class GameEvent
    {
        public DateTime OccuredAt;
    }

    public class DrawEvent : GameEvent
    {
        public bool FirstPlayerOnLeft { get; set; }
        public bool FirstPlayerServes { get; set; }
    }    

    public class PointEvent : GameEvent
    {
        public bool FirstPlayerPoint { get; set; }
        public bool FirstPlayerAction { get; set; }
        public PointKind Kind { get; set; }
    }
    public enum PointKind
    {
        Ace,
        Forehand,
        Backhand,
        Error,
        UnforcedError
    }
}
