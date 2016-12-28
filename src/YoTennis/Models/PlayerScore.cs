using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public struct PlayerScore
    {
        public int FirstPlayer { get; set; }
        public int SecondPlayer { get; set; }

        public static PlayerScore ForPlayer(Player player, int score) =>
            new PlayerScore
            {
                FirstPlayer = player == Player.First ? score : 0,
                SecondPlayer = player == Player.Second ? score : 0
            };
        public static PlayerScore operator +(PlayerScore left, PlayerScore right) =>
            new PlayerScore
            {
                FirstPlayer = left.FirstPlayer + right.FirstPlayer,
                SecondPlayer = left.SecondPlayer + right.SecondPlayer
            };
    }
}
