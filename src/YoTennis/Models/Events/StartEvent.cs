using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models.Events
{
    public class StartEvent : GameEvent
    {
        public MatchSettings Settings { get; set; }
        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }
    }
}
