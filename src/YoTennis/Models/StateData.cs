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

    public struct PlayerScore
    {
        public int FirstPlayer { get; set; }
        public int SecondPlayer { get; set; }

        public static PlayerScore ForPlayer(Player player, int score) =>
            new PlayerScore
            {
                FirstPlayer = player == Player.First ? score : 0,
                SecondPlayer = player == Player.Second ? score : 0
            };
        public static PlayerScore operator +(PlayerScore left, PlayerScore right) =>
            new PlayerScore
            {
                FirstPlayer = left.FirstPlayer + right.FirstPlayer,
                SecondPlayer = left.SecondPlayer + right.SecondPlayer
            };
    }

    public class Set
    {
        public PlayerScore Score { get; set; }
        public PlayerScore TieBreakScore { get; set; }
    }

    public class TieBreak
    {
        public PlayerScore Score { get; set; }
    }

    public class Game
    {
        public PlayerScore Score { get; set; }
    }

    public class ServeFailEvent
    {
        public ServeFailKind Serve { get; set; }
        public ServeSpeed ServeSpeed { get; set; }
    }

    public class MatchSettings
    {
        public int SetsForWin { get; set; }
        public bool TieBreakFinal { get; set; }
        public int GamesInSet { get; set; } = 6;
        public int PointsInGame { get; set; } = 4;
        public int PointsInTieBreak { get; set; } = 7;
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

    public static class PlayerExtentions
    {
        public static Player Other(this Player player) => player == Player.First ? Player.Second : Player.First;
        
    }

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
