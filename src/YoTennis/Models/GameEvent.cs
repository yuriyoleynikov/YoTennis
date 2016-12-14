using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class GameEvent
    {
        public DateTime OccuredAt;
    }
    public class CancelGame : GameEvent
    {
        //
    }

    public class StartGameEvent : GameEvent
    {
        //empty
    }

    public class EndGameEvent : GameEvent
    {
        //empty
    }
    
    public class StartFirstGameEvent : GameEvent
    {
        //empty
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

    public class StartEvent : GameEvent
    {
        public MatchSettings Settings { get; set; }
        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }
    }
    public class DrawEvent : GameEvent
    {
        public Player PlayerOnLeft { get; set; }
        public Player PlayerServes { get; set; }
    }

    public class PointEvent : GameEvent
    {
        public Player PlayerPoint { get; set; }
        public PointKind Kind { get; set; }
    }
}
