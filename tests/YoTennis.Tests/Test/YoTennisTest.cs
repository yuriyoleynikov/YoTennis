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
        static DateTime _gameDate2 = new DateTime(1986, 9, 28);
        static DateTime _gameDate3 = new DateTime(1986, 9, 29);

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

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
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

            myGame.AddEvent(new ChangeSidesGameEvent());

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
            myGame.CurrentState.MatchState.Should().Be(MatchState.BeginingGame);
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

            myGame.AddEvent(new ChangeSidesGameEvent());
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });

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
            myGame.CurrentState.GameTime.Should().Be(_gameDate2);
            myGame.CurrentState.MatchState.Should().Be(MatchState.PlayingGame);
        }

        [Fact]
        public void Chek_2GamesEvent()
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

            myGame.AddEvent(new ChangeSidesGameEvent());
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
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
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(2);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate2);
            myGame.CurrentState.MatchState.Should().Be(MatchState.BeginingGame);
        }

        [Fact]
        public void Chek_3GamesEvent()
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

            myGame.AddEvent(new ChangeSidesGameEvent());
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
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
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(3);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.ChangingSides);
        }

        [Fact]
        public void Chek_5GamesEvent()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 2; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }
            myGame.CurrentState.MatchDate.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TieBreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.Second);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(5);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.BeginingGame);
        }

        [Fact]
        public void Chek_10Games()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 2; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            for (int i2 = 0; i2 < 2; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.First,
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
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(5);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(5);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.BeginingGame);
        }

        [Fact]
        public void Chek_11Games()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 2; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            for (int i2 = 0; i2 < 3; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }



            myGame.CurrentState.MatchDate.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TieBreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(5);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.BeginingGame);
        }
        
        [Fact]
        public void Chek_12Games()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 2; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            for (int i2 = 0; i2 < 3; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
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

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.BeginTieBreak);
        }
        
        [Fact]
        public void Chek_12Games_StartTB()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 2; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            for (int i2 = 0; i2 < 3; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.AddEvent(new StartTieBreakEvent { OccuredAt = _gameDate3 });

            myGame.CurrentState.MatchDate.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TieBreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.PlayingTieBreak);
        }

        [Fact]
        public void Chek_12Games_1Point()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 2; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            for (int i2 = 0; i2 < 3; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.AddEvent(new StartTieBreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 1; i++)
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
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().TieBreakScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().TieBreakScore.SecondPlayer.Should().Be(1);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Left);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.PlayingTieBreak);
        }

        [Fact]
        public void Chek_12Games_2Points()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 2; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            for (int i2 = 0; i2 < 3; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.AddEvent(new StartTieBreakEvent { OccuredAt = _gameDate3 });
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
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().TieBreakScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().TieBreakScore.SecondPlayer.Should().Be(2);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.PlayingTieBreak);
        }

        [Fact]
        public void Chek_12Games_3Points()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 2; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            for (int i2 = 0; i2 < 3; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.AddEvent(new StartTieBreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 3; i++)
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
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().TieBreakScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().TieBreakScore.SecondPlayer.Should().Be(3);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Left);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.PlayingTieBreak);
        }

        [Fact]
        public void Chek_12Games_6Points()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 2; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            for (int i2 = 0; i2 < 3; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.AddEvent(new StartTieBreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 6; i++)
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
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().TieBreakScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().TieBreakScore.SecondPlayer.Should().Be(6);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.ChangingSidesOnTiebreak);
        }

        [Fact]
        public void Chek_12Games_6Points_CS()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 2; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            for (int i2 = 0; i2 < 3; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.AddEvent(new StartTieBreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 6; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }
            myGame.AddEvent(new ChangeSidesOnTiebreakGame { OccuredAt = _gameDate3 });

            myGame.CurrentState.MatchDate.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TieBreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.Second);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().TieBreakScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().TieBreakScore.SecondPlayer.Should().Be(6);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.PlayingTieBreak);
        }
        
        [Fact]
        public void Chek_12Games_12Points()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 2; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            for (int i2 = 0; i2 < 3; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.AddEvent(new StartTieBreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 6; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }
            myGame.AddEvent(new ChangeSidesOnTiebreakGame { OccuredAt = _gameDate3 });
            myGame.AddEvent(new StartTieBreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 6; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.First,
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
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().TieBreakScore.FirstPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInSets.Last().TieBreakScore.SecondPlayer.Should().Be(6);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.ChangingSidesOnTiebreak);
        }

        [Fact]
        public void Chek_12Games_13Points()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 2; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            for (int i2 = 0; i2 < 3; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.AddEvent(new StartTieBreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 6; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }
            myGame.AddEvent(new ChangeSidesOnTiebreakGame { OccuredAt = _gameDate3 });
            myGame.AddEvent(new StartTieBreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 6; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.First,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }
            myGame.AddEvent(new ChangeSidesOnTiebreakGame { OccuredAt = _gameDate3 });
            for (int i = 0; i < 1; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.First,
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
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().TieBreakScore.FirstPlayer.Should().Be(7);
            myGame.CurrentState.ScoreInSets.Last().TieBreakScore.SecondPlayer.Should().Be(6);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Left);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.PlayingTieBreak);
        }

        [Fact]
        public void Chek_12Games_14Points()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 2; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            for (int i2 = 0; i2 < 3; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.First,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.AddEvent(new StartTieBreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 6; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }
            myGame.AddEvent(new ChangeSidesOnTiebreakGame { OccuredAt = _gameDate3 });
            myGame.AddEvent(new StartTieBreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 6; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.First,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }
            myGame.AddEvent(new ChangeSidesOnTiebreakGame { OccuredAt = _gameDate3 });
            for (int i = 0; i < 2; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.First,
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
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(1);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(0);

            myGame.CurrentState.ScoreInSets[0].Score.FirstPlayer.Should().Be(7);
            myGame.CurrentState.ScoreInSets[0].Score.SecondPlayer.Should().Be(6);

            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Left);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.PlayingTieBreak);
        }

        [Fact]
        public void Chek_1SetEvent()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 2; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
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
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(1);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.BeginingGame);
        }

        [Fact]
        public void Chek_1SetEvent_Other1()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 2; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.First,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
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
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(1);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.ChangingSides);
        }

        [Fact]
        public void Chek_1Set_1ChangeSides()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 2; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.First,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }
            myGame.AddEvent(new ChangeSidesGameEvent());

            myGame.CurrentState.MatchDate.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TieBreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(1);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.BeginingGame);
        }

        [Fact]
        public void Chek_1Set_1Game()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 2; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.First,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 4; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }
            myGame.AddEvent(new ChangeSidesGameEvent());

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
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

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(1);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(1);
            myGame.CurrentState.ScoreInSets[0].Score.FirstPlayer.Should().Be(1);
            myGame.CurrentState.ScoreInSets[0].Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.ChangingSides);
        }
        
        [Fact]
        public void Chek_1Set_5Game()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 5; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }
            
            myGame.CurrentState.MatchDate.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TieBreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(1);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(5);
            
            myGame.CurrentState.ScoreInSets[0].Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets[0].Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.BeginingGame);
        }

        [Fact]
        public void Chek_2Sets()
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

            myGame.AddEvent(new ChangeSidesGameEvent());

            for (int i2 = 0; i2 < 5; i2++)
            {
                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate2 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }

                myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
                for (int i = 0; i < 4; i++)
                {
                    myGame.AddEvent(new PointEvent
                    {
                        PlayerPoint = Player.Second,
                        ServeSpeed = ServeSpeed.Unspecified,
                        Kind = PointKind.Forehand
                    });
                }
                myGame.AddEvent(new ChangeSidesGameEvent());
            }

            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate3 });
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

            //myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            //myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.ScoreOnSets.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreOnSets.SecondPlayer.Should().Be(2);
            myGame.CurrentState.ScoreInSets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets.Last().Score.SecondPlayer.Should().Be(6);

            myGame.CurrentState.ScoreInSets[0].Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.ScoreInSets[0].Score.SecondPlayer.Should().Be(6);
            //myGame.CurrentState.ScoreInGame.Score.FirstPlayer.Should().Be(0);
            //myGame.CurrentState.ScoreInGame.Score.SecondPlayer.Should().Be(0);
            //myGame.CurrentState.SecondServe.Should().Be(false);
            //myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            //myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.MatchState.Should().Be(MatchState.Completed);
        }
    }
}