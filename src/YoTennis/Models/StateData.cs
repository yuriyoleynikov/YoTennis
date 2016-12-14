using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class State
    {
        public List<Set> ScoreInSets { get; set; }        
        public Game ScoreInGame { get; set; }

        public DateTime TheGameTime { get; set; }
        public Player PlayerOnLeft { get; set; }
        public Player PlayerServes { get; set; }
        public ServePositionOnTheCenter ServePositionOnTheCenter { get; set; }
        public bool SecondServe { get; set; }

        public DateTime MatchDate { get; set; }
        public MatchSettings MatchSettings { get; set; }
        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }

        public MatchState MatchState { get; set; }
    }

    public struct PlayerScore
    {
        public int FirstPlayer { get; set; }
        public int SecondPlayer { get; set; }
    }

    public class Set
    {
        public PlayerScore Score { get; set; }
        public PlayerScore TieBreakScore { get; set; }
    }

    public class TieBreak
    {
        public PlayerScore Player { get; set; }
    }

    public class Game
    {
        public PlayerScore Score { get; set; }
    }

    public class ServeFailEvent
    {
        public ServeFailKind Serve { get; set; }
    }

    public class MatchSettings
    {
        public int SetsForWin { get; set; }
        public bool TieBreakFinal { get; set; }
    }

    public enum ServePositionOnTheCenter { Left, Right }

    public enum ServeFailKind
    {
        Error,
        NetTouch
    }

    public enum MatchState
    {
        NotStarted,
        Drawing,
        Playing,
        Comleted
    }

    public enum Player { First, Second }

    public enum Serve { First, NetTouchOnFirst, Second }

    public enum EnumInTheGame { love, fifteen, thirty, forty, advantage, game }

    public enum PointKind
    {
        Ace,
        Forehand,
        Backhand,
        Error,
        UnforcedError,
        Unspecified
    }
}
