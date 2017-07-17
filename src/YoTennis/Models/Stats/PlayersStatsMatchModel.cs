using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models.Stats
{
    public class PlayersStatsMatchModel
    {
        public PlayerStatsMatchModel FirstPlayer { get; set; }
        public PlayerStatsMatchModel SecondPlayer { get; set; }
    }
}
