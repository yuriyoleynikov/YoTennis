using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoTennis.Models;

namespace YoTennis.Helpers
{
    public static class MatchScoreExtensions
    {
        public static string ToSeparatedScoreString(MatchModel state)
        {
            StringBuilder score = new StringBuilder();
            bool isFirst = true;

            if (state.Sets != null)
                foreach (var s in state.Sets)
                {
                    if (isFirst)
                        isFirst = false;
                    else
                        score.Append(", ");

                    score.Append(s.Score.FirstPlayer.ToString()).Append("-").Append(s.Score.SecondPlayer.ToString());
                }
            return score.ToString();
        }
    }
}
