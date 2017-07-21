using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Models.Events;

namespace YoTennis.Models
{
    public class PlayersStatsMatchModel
    {
        public PlayerStatsMatchModel FirstPlayer { get; set; } = new PlayerStatsMatchModel();
        public PlayerStatsMatchModel SecondPlayer { get; set; } = new PlayerStatsMatchModel();        
    }
}
