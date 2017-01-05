using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class GameListModel
    {
        public IEnumerable<GameHandler> Games { get; set; }
    }
}