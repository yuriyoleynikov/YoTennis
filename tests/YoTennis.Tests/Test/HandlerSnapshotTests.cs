using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using Xunit;
using YoTennis.Models;
using YoTennis.Models.Events;

namespace YoTennis.Tests.Test
{
    using YoTennis.Tests.Helpers;

    public class HandlerSnapshotTests
    {/*
        [Fact]
        public void Test1()
        {
            var scenario = new Scenario();

            scenario.AddEvent(new StartEvent
            {
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
                }
            });
            scenario.AddEvent(new DrawEvent
            {
                PlayerOnLeft = Player.First,
                PlayerServes = Player.Second
            });
            scenario.AddEvent(new StartTiebreakEvent());
            scenario.AddEvent(new StartGameEvent());

            scenario.Verify();
        }

        [Fact]
        public void Test2()
        {
            var scenario = new Scenario();

            scenario.AddEvent(new StartEvent
            {
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
                }
            });
            scenario.AddEvent(new DrawEvent
            {
                PlayerOnLeft = Player.First,
                PlayerServes = Player.Second
            });
            scenario.AddEvent(new StartGameEvent());
            for(var i = 0; i < 21; i++)
            {
                scenario.AddEvent(new PointEvent { PlayerPoint = Player.Second, ServeSpeed = (ServeSpeed)(i % 4), Kind = (PointKind)(i % 7) });
                if (i%4 == 3)
                {
                    if (i % 8 == 3)
                        scenario.AddEvent(new ChangeSidesGameEvent());
                    scenario.AddEvent(new StartGameEvent());
                }
            }

            scenario.Verify();
        */
    }
}