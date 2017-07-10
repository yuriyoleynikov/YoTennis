using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class MatchModelView
    {
        public string Id { get; set; }
        public string Date { get; set; }
        public string Players { get; set; }
        public string Score { get; set; }
        public string Status { get; set; }
    }
}
