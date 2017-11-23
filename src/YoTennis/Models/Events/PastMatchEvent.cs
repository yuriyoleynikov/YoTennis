using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models.Events
{
    public class PastMatchEvent : GameEvent
    {
        public DateTime Date { get; set; }
        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }
        public string FirstPlayerUserId { get; set; }
        public string SecondPlayerUserId { get; set; }
        public List<SetModel> MatchScore { get; set; }
    }
}
