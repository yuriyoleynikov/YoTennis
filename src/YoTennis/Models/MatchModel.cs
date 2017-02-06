using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class MatchModel
    {
        public Score MatchScore { get; set; }
        public List<SetModel> Sets { get; set; }
        public Score GameScore { get; set; }

        public DateTime GameStratedAt { get; set; }
        public Player PlayerOnLeft { get; set; }
        public Player PlayerServes { get; set; }
        public ServePosition ServePosition { get; set; }
        public bool SecondServe { get; set; }

        public DateTime MatchStartedAt { get; set; }
        public MatchSettings MatchSettings { get; set; }
        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }

        public MatchState State { get; set; }
    }

    public enum ServeSpeed
    {
        Unspecified,
        Slow,
        Medium,
        Fast
    }

    public enum ServePosition
    {
        Right,
        Left
    }

    public enum ServeFailKind
    {
        Error,
        NetTouch
    }

    public enum MatchState
    {
        NotStarted,
        Drawing,
        BeginningGame,
        PlayingGame,
        ChangingSides,
        BeginningTiebreak,
        PlayingTiebreak,
        ChangingSidesOnTiebreak,
        Completed
    }

    public enum Player
    {
        First,
        Second
    }

    public enum PointKind
    {
        Unspecified,
        Ace,
        Forehand,
        Backhand,
        NetPoint,
        Error,
        UnforcedError,
        DoubleFaults
    }
}
