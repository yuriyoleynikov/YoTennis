using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Models.Events;

namespace YoTennis.Models
{
    public class PlayersStatsMatchModel
    {
        public PlayerStatsMatchModel FirstPlayer { get; set; } = new PlayerStatsMatchModel();
        public PlayerStatsMatchModel SecondPlayer { get; set; } = new PlayerStatsMatchModel();

        public void StatsEntry(PointEvent gameEvent, MatchModel state)
        {
            if (gameEvent.PlayerPoint == Player.First)
                FirstPlayer.TotalPoints++;
            if (gameEvent.PlayerPoint == Player.First && gameEvent.Kind == PointKind.Ace)
                FirstPlayer.Ace++;
            if (gameEvent.PlayerPoint == Player.First && gameEvent.Kind == PointKind.Backhand)
                FirstPlayer.Backhand++;
            if (state.PlayerServes == Player.First && gameEvent.Kind == PointKind.DoubleFaults)
                FirstPlayer.DoubleFaults++;
            if (gameEvent.PlayerPoint == Player.Second && gameEvent.Kind == PointKind.Error)
                FirstPlayer.Error++;
            if (gameEvent.PlayerPoint == Player.First && gameEvent.Kind == PointKind.Forehand)
                FirstPlayer.Forehand++;
            if (gameEvent.PlayerPoint == Player.First && gameEvent.Kind == PointKind.NetPoint)
                FirstPlayer.NetPoint++;
            if (gameEvent.PlayerPoint == Player.Second && gameEvent.Kind == PointKind.UnforcedError)
                FirstPlayer.UnforcedError++;

            if (state.PlayerServes == Player.First)
                FirstPlayer.FirstServe++;
            if (state.PlayerServes == Player.First && gameEvent.PlayerPoint == Player.First)
                FirstPlayer.WonOnFirstServe++;
            if (state.PlayerServes == Player.First && gameEvent.Kind != PointKind.DoubleFaults)
                FirstPlayer.FirstServeSuccessful++;

            if (state.SecondServe && state.PlayerServes == Player.First)
                FirstPlayer.SecondServe++;
            if (state.SecondServe && state.PlayerServes == Player.First && gameEvent.PlayerPoint == Player.First)
                FirstPlayer.SecondServeSuccessful++;
            if (state.SecondServe && state.PlayerServes == Player.First && gameEvent.Kind != PointKind.DoubleFaults)
                FirstPlayer.WonOnSecondServe++;


            if (gameEvent.PlayerPoint == Player.Second)
                SecondPlayer.TotalPoints++;
            if (gameEvent.PlayerPoint == Player.Second && gameEvent.Kind == PointKind.Ace)
                SecondPlayer.Ace++;
            if (gameEvent.PlayerPoint == Player.Second && gameEvent.Kind == PointKind.Backhand)
                SecondPlayer.Backhand++;
            if (state.PlayerServes == Player.Second && gameEvent.Kind == PointKind.DoubleFaults)
                SecondPlayer.DoubleFaults++;
            if (gameEvent.PlayerPoint == Player.First && gameEvent.Kind == PointKind.Error)
                SecondPlayer.Error++;
            if (gameEvent.PlayerPoint == Player.Second && gameEvent.Kind == PointKind.Forehand)
                SecondPlayer.Forehand++;
            if (gameEvent.PlayerPoint == Player.Second && gameEvent.Kind == PointKind.NetPoint)
                SecondPlayer.NetPoint++;
            if (gameEvent.PlayerPoint == Player.First && gameEvent.Kind == PointKind.UnforcedError)
                SecondPlayer.UnforcedError++;

            if (state.PlayerServes == Player.Second)
                SecondPlayer.FirstServe++;
            if (state.PlayerServes == Player.Second && gameEvent.PlayerPoint == Player.Second)
                SecondPlayer.WonOnFirstServe++;
            if (state.PlayerServes == Player.Second && gameEvent.Kind != PointKind.DoubleFaults)
                SecondPlayer.FirstServeSuccessful++;

            if (state.SecondServe && state.PlayerServes == Player.Second)
                SecondPlayer.SecondServe++;
            if (state.SecondServe && state.PlayerServes == Player.Second && gameEvent.PlayerPoint == Player.Second)
                SecondPlayer.SecondServeSuccessful++;
            if (state.SecondServe && state.PlayerServes == Player.Second && gameEvent.Kind != PointKind.DoubleFaults)
                SecondPlayer.WonOnSecondServe++;
        }

        public void StatsEntry(ServeFailEvent gameEvent, MatchModel state)
        {
            if (gameEvent.Serve == ServeFailKind.Error && state.PlayerServes == Player.Second && state.SecondServe)
                FirstPlayer.TotalPoints++;            
            if (gameEvent.Serve == ServeFailKind.Error && state.PlayerServes == Player.First && state.SecondServe)
                FirstPlayer.DoubleFaults++;

            if (gameEvent.Serve == ServeFailKind.Error && state.PlayerServes == Player.First && !state.SecondServe)
                FirstPlayer.FirstServe++;
            
            if (state.PlayerServes == Player.First && state.SecondServe)
                FirstPlayer.SecondServe++;


            if (gameEvent.Serve == ServeFailKind.Error && state.PlayerServes == Player.First && state.SecondServe)
                SecondPlayer.TotalPoints++;
            if (gameEvent.Serve == ServeFailKind.Error && state.PlayerServes == Player.Second && state.SecondServe)
                SecondPlayer.DoubleFaults++;

            if (gameEvent.Serve == ServeFailKind.Error && state.PlayerServes == Player.Second && !state.SecondServe)
                SecondPlayer.FirstServe++;

            if (state.PlayerServes == Player.Second && state.SecondServe)
                SecondPlayer.SecondServe++;
        }
    }
}
