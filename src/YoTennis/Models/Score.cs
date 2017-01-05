using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public struct Score
    {
        public int FirstPlayer { get; set; }
        public int SecondPlayer { get; set; }

        public static Score ForPlayer(Player player, int score) =>
            new Score
            {
                FirstPlayer = player == Player.First ? score : 0,
                SecondPlayer = player == Player.Second ? score : 0
            };
        public static Score operator +(Score left, Score right) =>
            new Score
            {
                FirstPlayer = left.FirstPlayer + right.FirstPlayer,
                SecondPlayer = left.SecondPlayer + right.SecondPlayer
            };
    }
}
