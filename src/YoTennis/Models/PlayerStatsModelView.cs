using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class PlayerStatsModelView
    {
        public string Player { get; set; }
        public int Matches { get; set; }
        public int Completed { get; set; }
        public int Won { get; set; }
        public int Lost { get; set; }
        public PlayerStatsMatchModel AggregatedMatchStats { get; set; }
    }
}
