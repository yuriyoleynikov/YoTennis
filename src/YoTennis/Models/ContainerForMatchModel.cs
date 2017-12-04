using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public List<string> FilterPayers { get; set; } = 
            new List<string>();
        public IEnumerable<string> SelectedPlayers { get; set; }
        public List<MatchState> FilterState { get; set; } = 
            new List<MatchState>() { MatchState.NotStarted, MatchState.Drawing,
                MatchState.Completed, MatchState.CompletedAndNotFinished, MatchState.PastMatchImported };
        public List<DateTime> FilterDate { get; set; }
        public IEnumerable<MatchState> SelectedState { get; set; }       
        [DataType(DataType.Date)]
        public DateTime? BeginningWithDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FinishingBeforeDate { get; set; }
        public Sort Sort { get; set; }
    }
}
