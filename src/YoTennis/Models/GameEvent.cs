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
        //dont know
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

    public class ChangeSidesGame : GameEvent
    {
        //empty
    }

    public class ChangeServeGame : GameEvent
    {
        //empty
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
        public ServeSpeed ServeSpeed { get; set; }
    }
}
