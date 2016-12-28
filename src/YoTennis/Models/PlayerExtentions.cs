using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public static class PlayerExtentions
    {
        public static Player Other(this Player player) => player == Player.First ? Player.Second : Player.First;
    }
}
