using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Helpers;
using YoTennis.Models;

namespace YoTennis.Data
{
    public static class Conversion
    {
        public static MatchInfo ToMatchInfo(this MatchModel model)
        {
            var matchInfo = new MatchInfo()
            {
                FirstPlayer = model.FirstPlayer,
                SecondPlayer = model.SecondPlayer,
                State = model.State                 
            };
            
            if (model.State != MatchState.NotStarted)
                matchInfo.MatchStartedAt = model.MatchStartedAt;

            if (model.State != MatchState.NotStarted && model.State != MatchState.Drawing)
                matchInfo.MatchScore = MatchScoreExtensions.ToSeparatedScoreString(model);

            if (model.State == MatchState.Completed)
                matchInfo.Winner = model.MatchScore.FirstPlayer > model.MatchScore.SecondPlayer ? Player.First : Player.Second;

            return matchInfo;
        }
    }
}
