﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class PlayersStatsMatchModel
    {
        public PlayerStatsMatchModel FirstPlayer { get; set; } = new PlayerStatsMatchModel();
        public PlayerStatsMatchModel SecondPlayer { get; set; } = new PlayerStatsMatchModel();
    }
}
