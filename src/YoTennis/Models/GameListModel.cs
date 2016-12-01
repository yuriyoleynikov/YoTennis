using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class GameListModel
    {
        public IEnumerable<GameModel> Games { get; set; }
    }
}
