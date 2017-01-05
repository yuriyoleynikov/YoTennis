using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YoTennis.Models;
using YoTennis.Models.Events;

namespace YoTennis.Tests.Test
{
    public class YoTennisTest
    {
        static DateTime _matchDate = new DateTime(1986, 9, 26);
        static DateTime _gameDate = new DateTime(1986, 9, 27);
        static DateTime _gameDate2 = new DateTime(1986, 9, 28);
        static DateTime _gameDate3 = new DateTime(1986, 9, 29);

        [Fact]
        public void Check_NoEvent()
        {
            var myGame = new GameHandler();

            myGame.CurrentState.State.Should().Be(MatchState.NotStarted);
        }

        [Fact]
        public void Check_StartEvent()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
                }
            });

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);
            myGame.CurrentState.State.Should().Be(MatchState.Drawing);
        }

        [Fact]
        public void Check_DrawEvent()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
                }
            });
            myGame.AddEvent(new DrawEvent
            {
                OccuredAt = _matchDate,
                PlayerOnLeft = Player.First,
                PlayerServes = Player.Second
            });

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_matchDate);
            myGame.CurrentState.State.Should().Be(MatchState.BeginningGame);
        }

        [Fact]
        public void Cheсk_StartGameEvent()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
                }
            });
            myGame.AddEvent(new DrawEvent
            {
                OccuredAt = _matchDate,
                PlayerOnLeft = Player.First,
                PlayerServes = Player.Second
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate);
            myGame.CurrentState.State.Should().Be(MatchState.PlayingGame);
        }

        [Fact]
        public void Cheсk_Not_Expected_ChangesSides()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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
                    TiebreakFinal = false
                }
            })).ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Cheсk_2Points_Event()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(2);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate);
            myGame.CurrentState.State.Should().Be(MatchState.PlayingGame);
        }

        [Fact]
        public void Cheсk_4Points_Event()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(1);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate);
            myGame.CurrentState.State.Should().Be(MatchState.ChangingSides);
        }

        [Fact]
        public void Cheсk_4Points_Event_ChangeSidesEvent()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.Second);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(1);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate);
            myGame.CurrentState.State.Should().Be(MatchState.BeginningGame);
        }

        [Fact]
        public void Cheсk_4Points_Event_ChangeSidesEvent_StartGameEvent()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.Second);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(1);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate2);
            myGame.CurrentState.State.Should().Be(MatchState.PlayingGame);
        }

        [Fact]
        public void Cheсk_2GamesEvent()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.Second);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(2);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate2);
            myGame.CurrentState.State.Should().Be(MatchState.BeginningGame);
        }

        [Fact]
        public void Cheсk_FailEvent()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
                }
            });
            myGame.AddEvent(new DrawEvent
            {
                OccuredAt = _matchDate,
                PlayerOnLeft = Player.First,
                PlayerServes = Player.Second
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });

            myGame.AddEvent(new ServeFailEvent { Serve = ServeFailKind.Error });
            
            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(true);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate);
            myGame.CurrentState.State.Should().Be(MatchState.PlayingGame);
        }

        [Fact]
        public void Cheсk_3GamesEvent()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.Second);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(3);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.ChangingSides);
        }

        [Fact]
        public void Cheсk_5GamesEvent()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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
            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.Second);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(5);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.BeginningGame);
        }

        [Fact]
        public void Check_10Games()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.Second);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(5);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(5);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.BeginningGame);
        }

        [Fact]
        public void Check_11Games()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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



            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(5);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.BeginningGame);
        }

        [Fact]
        public void Check_12Games()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.BeginningTiebreak);
        }

        [Fact]
        public void Cheсk_12Games_StartTB()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.AddEvent(new StartTiebreakEvent { OccuredAt = _gameDate3 });

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.PlayingTiebreak);
        }

        [Fact]
        public void Cheсk_12Games_1Point()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.AddEvent(new StartTiebreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 1; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }


            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().TiebreakScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().TiebreakScore.SecondPlayer.Should().Be(1);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Left);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.PlayingTiebreak);
        }

        [Fact]
        public void Cheсk_12Games_2Points()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.AddEvent(new StartTiebreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 2; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }


            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().TiebreakScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().TiebreakScore.SecondPlayer.Should().Be(2);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.PlayingTiebreak);
        }

        [Fact]
        public void Cheсk_12Games_3Points()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.AddEvent(new StartTiebreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 3; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }


            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().TiebreakScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().TiebreakScore.SecondPlayer.Should().Be(3);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Left);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.PlayingTiebreak);
        }

        [Fact]
        public void Cheсk_12Games_6Points()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.AddEvent(new StartTiebreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 6; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }


            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().TiebreakScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().TiebreakScore.SecondPlayer.Should().Be(6);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.ChangingSidesOnTiebreak);
        }

        [Fact]
        public void Cheсk_12Games_6Points_CS()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.AddEvent(new StartTiebreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 6; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }
            myGame.AddEvent(new ChangeSidesOnTiebreakEvent { OccuredAt = _gameDate3 });

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.Second);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().TiebreakScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().TiebreakScore.SecondPlayer.Should().Be(6);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.PlayingTiebreak);
        }

        [Fact]
        public void Check_12Games_12Points()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.AddEvent(new StartTiebreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 6; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }
            myGame.AddEvent(new ChangeSidesOnTiebreakEvent { OccuredAt = _gameDate3 });
            myGame.AddEvent(new StartTiebreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 6; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.First,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }


            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.Second);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().TiebreakScore.FirstPlayer.Should().Be(6);
            myGame.CurrentState.Sets.Last().TiebreakScore.SecondPlayer.Should().Be(6);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.ChangingSidesOnTiebreak);
        }

        [Fact]
        public void Check_12Games_13Points()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.AddEvent(new StartTiebreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 6; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }
            myGame.AddEvent(new ChangeSidesOnTiebreakEvent { OccuredAt = _gameDate3 });
            myGame.AddEvent(new StartTiebreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 6; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.First,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }
            myGame.AddEvent(new ChangeSidesOnTiebreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 1; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.First,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(6);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().TiebreakScore.FirstPlayer.Should().Be(7);
            myGame.CurrentState.Sets.Last().TiebreakScore.SecondPlayer.Should().Be(6);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Left);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.PlayingTiebreak);
        }

        [Fact]
        public void Cheсk_12Games_14Points()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.AddEvent(new StartTiebreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 6; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }
            myGame.AddEvent(new ChangeSidesOnTiebreakEvent { OccuredAt = _gameDate3 });
            myGame.AddEvent(new StartTiebreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 6; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.First,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }
            myGame.AddEvent(new ChangeSidesOnTiebreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 2; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.First,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(1);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(0);

            myGame.CurrentState.Sets[0].Score.FirstPlayer.Should().Be(7);
            myGame.CurrentState.Sets[0].Score.SecondPlayer.Should().Be(6);

            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.ChangingSides);
        }

        [Fact]
        public void Cheсk_12Games_14Points_CS()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.AddEvent(new StartTiebreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 6; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.Second,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }
            myGame.AddEvent(new ChangeSidesOnTiebreakEvent { OccuredAt = _gameDate3 });
            myGame.AddEvent(new StartTiebreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 6; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.First,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }
            myGame.AddEvent(new ChangeSidesOnTiebreakEvent { OccuredAt = _gameDate3 });
            for (int i = 0; i < 2; i++)
            {
                myGame.AddEvent(new PointEvent
                {
                    PlayerPoint = Player.First,
                    ServeSpeed = ServeSpeed.Unspecified,
                    Kind = PointKind.Forehand
                });
            }
            myGame.AddEvent(new ChangeSidesGameEvent());

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.Second);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(1);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(0);

            myGame.CurrentState.Sets[0].Score.FirstPlayer.Should().Be(7);
            myGame.CurrentState.Sets[0].Score.SecondPlayer.Should().Be(6);

            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.BeginningGame);
        }



        [Fact]
        public void Cheсk_1SetEvent()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.Second);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(1);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.BeginningGame);
        }

        [Fact]
        public void Cheсk_1SetEvent_Other1()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.Second);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(1);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.ChangingSides);
        }

        [Fact]
        public void Cheсk_1Set_1ChangeSides()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(1);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.BeginningGame);
        }

        [Fact]
        public void Cheсk_1Set_1Game()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.Second);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(1);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(1);
            myGame.CurrentState.Sets[0].Score.FirstPlayer.Should().Be(1);
            myGame.CurrentState.Sets[0].Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.ChangingSides);
        }

        [Fact]
        public void Cheсk_1Set_5Game()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(1);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(5);

            myGame.CurrentState.Sets[0].Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets[0].Score.SecondPlayer.Should().Be(6);
            myGame.CurrentState.GameScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.GameScore.SecondPlayer.Should().Be(0);
            myGame.CurrentState.SecondServe.Should().Be(false);
            myGame.CurrentState.ServePosition.Should().Be(ServePosition.Right);
            myGame.CurrentState.GameStratedAt.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.BeginningGame);
        }

        [Fact]
        public void Cheсk_2Sets()
        {
            var myGame = new GameHandler();
            myGame.AddEvent(new StartEvent
            {
                OccuredAt = _matchDate,
                FirstPlayer = "Oleynikov",
                SecondPlayer = "Nadal",
                Settings = new MatchSettings
                {
                    SetsForWin = 3,
                    TiebreakFinal = false
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

            myGame.CurrentState.MatchStartedAt.Should().Be(_matchDate);
            myGame.CurrentState.FirstPlayer.Should().Be("Oleynikov");
            myGame.CurrentState.SecondPlayer.Should().Be("Nadal");
            myGame.CurrentState.MatchSettings.SetsForWin.Should().Be(3);
            myGame.CurrentState.MatchSettings.TiebreakFinal.Should().Be(false);

            //myGame.CurrentState.PlayerOnLeft.Should().Be(Player.First);
            //myGame.CurrentState.PlayerServes.Should().Be(Player.First);
            myGame.CurrentState.MatchScore.FirstPlayer.Should().Be(0);
            myGame.CurrentState.MatchScore.SecondPlayer.Should().Be(2);
            myGame.CurrentState.Sets.Last().Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets.Last().Score.SecondPlayer.Should().Be(6);

            myGame.CurrentState.Sets[0].Score.FirstPlayer.Should().Be(0);
            myGame.CurrentState.Sets[0].Score.SecondPlayer.Should().Be(6);
            //myGame.CurrentState.ScoreInGameScore.FirstPlayer.Should().Be(0);
            //myGame.CurrentState.ScoreInGameScore.SecondPlayer.Should().Be(0);
            //myGame.CurrentState.SecondServe.Should().Be(false);
            //myGame.CurrentState.ServePositionOnTheCenterLine.Should().Be(ServePositionOnTheCenterLine.Right);
            //myGame.CurrentState.GameTime.Should().Be(_gameDate3);
            myGame.CurrentState.State.Should().Be(MatchState.Completed);
        }
    }
}