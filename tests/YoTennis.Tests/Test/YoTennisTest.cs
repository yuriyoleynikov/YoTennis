using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YoTennis.Models;

namespace YoTennis.Tests.Test
{
    public class YoTennisTest
    {
        static DateTime _matchDate = new DateTime(1986, 9, 26);
        static DateTime _gameDate = new DateTime(1986, 9, 27);

        [Fact]
        public void Chek_NoEvent()
        {
            var myGame = new GameModel();

            myGame.CurrentState.MatchState.Should().Be(MatchState.NotStarted);
        }

        [Fact]
        public void Chek_StartEvent()
        {
            var myGame = new GameModel();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TieBreakFinal = false
                }
            });

            myGame.CurrentState.MatchDate.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TieBreakFinal.Should().Be(false);
            myGame.CurrentState.MatchState.Should().Be(MatchState.Drawing);
        }

        [Fact]
        public void Chek_DrawEvent()
        {
            var myGame = new GameModel();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TieBreakFinal = false
                }
            });
            myGame.AddEvent(new DrawEvent
            {
                OccuredAt = _matchDate,
                PlayerOnLeft = Player.First,
                PlayerServes = Player.Second
            });

            myGame.CurrentState.MatchDate.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TieBreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_matchDate);
            myGame.CurrentState.MatchState.Should().Be(MatchState.BeginingGame);
        }

        [Fact]
        public void Chek_StartGameEvent()
        {
            var myGame = new GameModel();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TieBreakFinal = false
                }
            });
            myGame.AddEvent(new DrawEvent
            {
                OccuredAt = _matchDate,
                PlayerOnLeft = Player.First,
                PlayerServes = Player.Second
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });

            myGame.CurrentState.MatchDate.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TieBreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate);
            myGame.CurrentState.MatchState.Should().Be(MatchState.PlayingGame);
        }
        [Fact]
        public void Chek_Not_Expected_ChangesSides()
        {
            var myGame = new GameModel();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TieBreakFinal = false
                }
            });

            new Action(() => myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TieBreakFinal = false
                }
            })).ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Chek_2Points_Event()
        {
            var myGame = new GameModel();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TieBreakFinal = false
                }
            });
            myGame.AddEvent(new DrawEvent
            {
                OccuredAt = _matchDate,
                PlayerOnLeft = Player.First,
                PlayerServes = Player.Second
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });
            for (int i = 0; i < 2; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.CurrentState.MatchDate.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TieBreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(2);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate);
            myGame.CurrentState.MatchState.Should().Be(MatchState.PlayingGame);
        }

        [Fact]
        public void Chek_4Points_Event()
        {
            var myGame = new GameModel();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TieBreakFinal = false
                }
            });
            myGame.AddEvent(new DrawEvent
            {
                OccuredAt = _matchDate,
                PlayerOnLeft = Player.First,
                PlayerServes = Player.Second
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.CurrentState.MatchDate.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TieBreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.Second);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(1);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate);
            myGame.CurrentState.MatchState.Should().Be(MatchState.ChangingSides);
        }

        [Fact]
        public void Chek_4Points_Event_ChangeSidesEvent()
        {
            var myGame = new GameModel();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TieBreakFinal = false
                }
            });
            myGame.AddEvent(new DrawEvent
            {
                OccuredAt = _matchDate,
                PlayerOnLeft = Player.First,
                PlayerServes = Player.Second
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.AddEvent(new ChangeSidesGame());

            myGame.CurrentState.MatchDate.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TieBreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.Second);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(1);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate);
            myGame.CurrentState.MatchState.Should().Be(MatchState.ChangingSides);
        }

        [Fact]
        public void Chek_4Points_Event_ChangeSidesEvent_StartGameEvent()
        {
            var myGame = new GameModel();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TieBreakFinal = false
                }
            });
            myGame.AddEvent(new DrawEvent
            {
                OccuredAt = _matchDate,
                PlayerOnLeft = Player.First,
                PlayerServes = Player.Second
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.AddEvent(new ChangeSidesGame());
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });

            myGame.CurrentState.MatchDate.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TieBreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.Second);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(1);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate);
            myGame.CurrentState.MatchState.Should().Be(MatchState.PlayingGame);
        }
    }
}