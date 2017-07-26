using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class PlayersStatsMatchView
    {
        public PlayersStatsMatchModel PlayersStatsMatchModel { get; set; }
        public string FirstPlayerName { get; set; }
        public string SecondPlayerName { get; set; }
    }
}
