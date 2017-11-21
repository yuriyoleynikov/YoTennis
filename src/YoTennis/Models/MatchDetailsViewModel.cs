using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class MatchDetailsViewModel
    {
        public string Id { get; set; }
        public MatchModel MatchModel { get; set; }
        public string UserId { get; set; }
    }
}
