using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Models;

namespace YoTennis.Data
{
    public class MatchInfo
    {
        public Guid MatchId { get; set; }
        public string UserId { get; set; }
        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }
        public DateTime MatchStartedAt { get; set; }
        public MatchState State { get; set; }
        public string MatchScore { get; set; }
        public Player? Winner { get; set; }
    }    
}
