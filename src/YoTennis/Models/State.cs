using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class State
    {
        public Score SetScore { get; set; }
        public List<Set> Sets { get; set; }
        public Game GameScore { get; set; }

        public DateTime GameStratedAt { get; set; }
        public Player PlayerOnLeft { get; set; }
        public Player PlayerServes { get; set; }
        public ServePosition ServePosition { get; set; }
        public bool SecondServe { get; set; }

        public DateTime MatchStartedAt { get; set; }
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
    public enum ServePosition { Left, Right }

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
        BeginTiebreak,
        PlayingTiebreak,
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
