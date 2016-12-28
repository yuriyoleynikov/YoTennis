using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class Set
    {
        public PlayerScore Score { get; set; }
        public PlayerScore TieBreakScore { get; set; }
    }
}
