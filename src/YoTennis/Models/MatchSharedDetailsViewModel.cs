using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class MatchSharedDetailsViewModel
    {
        public string Id { get; set; }
        public string Shared { get; set; }
        public MatchModel MatchModel { get; set; }
        public string Hash { get; set; }
        public long Date { get; set; }
    }
}
