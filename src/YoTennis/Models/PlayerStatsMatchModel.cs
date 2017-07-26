using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class PlayerStatsMatchModel
    {
        public int TotalPoints { get; set; }
        public int Ace { get; set; }
        public int Backhand { get; set; }
        public int DoubleFaults { get; set; }
        public int Error { get; set; }
        public int Forehand { get; set; }
        public int NetPoint { get; set; }
        public int UnforcedError { get; set; }

        public int FirstServe { get; set; }
        public int FirstServeSuccessful { get; set; }
        public int WonOnFirstServe { get; set; }

        public int SecondServe { get; set; }
        public int SecondServeSuccessful { get; set; }
        public int WonOnSecondServe { get; set; }

        public static PlayerStatsMatchModel operator +(PlayerStatsMatchModel first, PlayerStatsMatchModel second) => new PlayerStatsMatchModel
        {
            TotalPoints = first.TotalPoints + second.TotalPoints,

            Ace = first.Ace + second.Ace,
            Backhand = first.Backhand + second.Backhand,
            DoubleFaults = first.DoubleFaults + second.DoubleFaults,
            Error = first.Error + second.Error,
            Forehand = first.Forehand + second.Forehand,
            NetPoint = first.NetPoint + second.NetPoint,
            UnforcedError = first.UnforcedError + second.UnforcedError,

            FirstServe = first.FirstServe + second.FirstServe,
            FirstServeSuccessful = first.FirstServeSuccessful + second.FirstServeSuccessful,
            WonOnFirstServe = first.WonOnFirstServe + second.WonOnFirstServe,

            SecondServe = first.SecondServe + second.SecondServe,
            SecondServeSuccessful = first.SecondServeSuccessful + second.SecondServeSuccessful,
            WonOnSecondServe = first.WonOnSecondServe + second.WonOnSecondServe
        };
    }
}
