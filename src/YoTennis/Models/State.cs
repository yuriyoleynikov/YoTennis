using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class State
    {
        public PlayerScore ScoreOnSets { get; set; }
        public List<Set> ScoreInSets { get; set; }
        public Game ScoreInGame { get; set; }

        public DateTime GameTime { get; set; }
        public Player PlayerOnLeft { get; set; }
        public Player PlayerServes { get; set; }
        public ServePositionOnTheCenterLine ServePositionOnTheCenterLine { get; set; }
        public bool SecondServe { get; set; }

        public DateTime MatchDate { get; set; }
        public MatchSettings MatchSettings { get; set; }
        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }

        public MatchState MatchState { get; set; }

        public ServeSpeed ServeSpeed { get; set; }
    }

    public enum ServeSpeed
    {
        Unspecified,
        Slow,
        Medium,
        Fast
    }
    public enum ServePositionOnTheCenterLine { Left, Right }

    public enum ServeFailKind
    {
        Error,
        NetTouch
    }

    public enum MatchState
    {
        NotStarted,
        Drawing,
        BeginingGame,
        PlayingGame,
        ChangingSides,
        BeginTieBreak,
        PlayingTieBreak,
        ChangingSidesOnTiebreak,
        Completed
    }

    public enum Player { First, Second }

    public enum PointKind
    {
        Unspecified,
        Ace,
        Forehand,
        Backhand,
        Error,
        UnforcedError,
        DoubleFaults
    }
}
