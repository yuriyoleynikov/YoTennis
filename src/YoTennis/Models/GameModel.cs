using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class GameModel
    {
        public GameSettings Settings { get; set; }
        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }
        public DateTime Date { get; set; }
        public Score Score { get; set; }
        public TimeSpan Time { get; set; }
        public State CurrentState { get; set; }
        public IEnumerable<GameEvent> Events { get; set; }

    }

    public class State
    {

    }

    public struct Score
    {
        public Match Match { get; set; }
        public Sets Sets { get; set; }
        public Games Games { get; set; }
        public Game Game { get; set; }


    }
    public struct Match
    {
        public int FirstPlayer { get; set; }
        public int SecondPlayer { get; set; }
    }
    public struct Sets
    {
        public int FirstPlayer { get; set; }
        public int SecondPlayer { get; set; }
    }
    public struct Games
    {
        public int FirstPlayer { get; set; }
        public int SecondPlayer { get; set; }
    }
    public struct Game
    {
        public StructGame FirstPlayer { get; set; }
        public StructGame SecondPlayer { get; set; }


    }

    public enum StructGame { love, fifteen, thirty, forty, advantage, game }

    public class GameSettings
    {
        public int SetsToWin { get; set; }
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
    public class SideExchangeEvent : GameEvent
    {

    }
    public class ServeFailEvent : GameEvent
    {
        public FailKind Kind { get; set; }
    }

    public enum FailKind { Error, TouchNet }

    public class SecondServeFailEvent : GameEvent
    {
        public FailKind Kind { get; set; }
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
