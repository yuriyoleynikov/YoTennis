﻿using System;
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
        public int WonOnFirstServe { get; set; }
        public int FirstServeSuccessful { get; set; }

        public int SecondServe { get; set; }
        public int SecondServeSuccessful { get; set; }
        public int WonOnSecondServe { get; set; }
    }
}
