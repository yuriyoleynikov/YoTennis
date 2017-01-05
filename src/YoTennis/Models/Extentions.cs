using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public static class Extentions
    {
        public static Player Other(this Player player) => player == Player.First ? Player.Second : Player.First;
        public static ServePosition Other(this ServePosition position) =>
            position == ServePosition.Left ? ServePosition.Right : ServePosition.Left;
    }
}