using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Data
{
    public class MatchEvent
    {
        public Guid MatchId { get; set; }
        public string Event { get; set; }
        public int Version { get; set; }
    }
}
