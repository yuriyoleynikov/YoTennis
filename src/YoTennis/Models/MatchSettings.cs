using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class MatchSettings
    {
        public int SetsForWin { get; set; } = 2;
        public bool TiebreakFinal { get; set; } = false;
        public int GamesInSet { get; set; } = 6;
        public int PointsInGame { get; set; } = 4;
        public int PointsInTiebreak { get; set; } = 7;
    }
}
