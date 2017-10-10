using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class ContainerForPlayersStats
    {
        public List<PlayerStatsModelView> ListPlayerStatsModelView { get; set; }
        public SortForPlayerStats Sort { get; set; }
        public int TotalPlayers { get; set; }
        public int Count { get; set; }
        public int Skip { get; set; }
    }
}
