using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class ContainerForMatchModel
    {
        public List<MatchModelView> ListMatchModelView { get; set; }
        public int TotalCount { get; set; }
        public int Count { get; set; }
        public int Skip { get; set; }
        public List<string> FilterPayers { get; set; } = new List<string>();
        public IEnumerable<string> SelectedPlayers { get; set; }
        public List<MatchState> FilterState { get; set; } = new List<MatchState>() { MatchState.NotStarted, MatchState.Drawing, MatchState.Completed };
        public IEnumerable<MatchState> SelectedState { get; set; }
    }
}
