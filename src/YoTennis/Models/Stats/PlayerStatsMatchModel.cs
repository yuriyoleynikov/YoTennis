using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models.Stats
{
    public class PlayerStatsMatchModel
    {
        public int FirstServe { get; set; }
        public int FirstServeSuccessful { get; set; }
        public int SecondServe { get; set; }
        public int SecondServeSuccessful { get; set; }
    }
}
