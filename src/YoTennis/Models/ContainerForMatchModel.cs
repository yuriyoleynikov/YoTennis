using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class ContainerForMatchModel
    {
        public List<MatchModelView> ListMatchModelView { get; set; }
        public int CountMatches { get; set; }
        public int Count { get; set; }
        public int Skip { get; set; }
    }
}
