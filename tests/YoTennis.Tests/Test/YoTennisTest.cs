using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YoTennis.Data;
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.NotStarted);
            matchInfo.MatchStartedAt.Should().Be(DateTime.MinValue);
            matchInfo.FirstPlayer.Should().Be(null);
            matchInfo.SecondPlayer.Should().Be(null);
            matchInfo.MatchScore.Should().Be(null);
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.Drawing);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be(null);
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.BeginningGame);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("0-0");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.PlayingGame);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("0-0");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.PlayingGame);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("0-0");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.ChangingSides);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("0-1");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.BeginningGame);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("0-1");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.PlayingGame);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("0-1");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.BeginningGame);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("0-2");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.PlayingGame);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("0-0");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.ChangingSides);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("0-3");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.BeginningGame);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("0-5");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.BeginningGame);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("5-5");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.BeginningGame);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("6-5");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.BeginningTiebreak);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("6-6");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.PlayingTiebreak);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("6-6");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.PlayingTiebreak);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("6-6");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.PlayingTiebreak);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("6-6");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.PlayingTiebreak);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("6-6");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.ChangingSidesOnTiebreak);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("6-6");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.PlayingTiebreak);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("6-6");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.ChangingSidesOnTiebreak);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("6-6");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.PlayingTiebreak);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("6-6");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.ChangingSides);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("7-6, 0-0");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.BeginningGame);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("7-6, 0-0");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.BeginningGame);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("0-6, 0-0");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.ChangingSides);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("1-6, 0-0");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.BeginningGame);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("1-6, 0-0");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.ChangingSides);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("1-6, 0-1");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.BeginningGame);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("0-6, 0-5");
            matchInfo.Winner.Should().Be(null);
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

            var matchInfo = myGame.CurrentState.ToMatchInfo();

            matchInfo.State.Should().Be(MatchState.Completed);
            matchInfo.MatchStartedAt.Should().Be(_matchDate);
            matchInfo.FirstPlayer.Should().Be("Oleynikov");
            matchInfo.SecondPlayer.Should().Be("Nadal");
            matchInfo.MatchScore.Should().Be("0-6, 0-6");
            matchInfo.Winner.Should().Be(Player.Second);
        }

        [Fact]
        public void Undo_fails_when_no_events()
        {
            var myGame = new GameHandler();
            new Action(() => myGame.UndoLastEvent()).ShouldThrowExactly<InvalidOperationException>();
        }


        //New test for Stat

        [Fact]
        public void Check_Stat_NoEvent()
        {
            var myGame = new GameHandler();

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Check_Stat_StartEvent()
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

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Check_Stat_DrawEvent()
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

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_StartGameEvent()
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

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        

        [Fact]
        public void Cheсk_Stat_PointEvent_Ace()
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

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Ace
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_PointEvent_Backhand()
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

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Backhand
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_PointEvent_Error()
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

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Error
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_PointEvent_Forehand()
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

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Forehand
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_PointEvent_NetPoint()
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

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.NetPoint
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_PointEvent_UnforcedError()
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

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.UnforcedError
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_PointEvent_Unspecified()
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

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Unspecified
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_ServeFailEvent_PointEvent_Ace()
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

            myGame.AddEvent(new ServeFailEvent
            {
                OccuredAt = _gameDate2,
                Serve = ServeFailKind.Error,
                ServeSpeed = ServeSpeed.Unspecified
            });

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Ace
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(1);
        }

        [Fact]
        public void Cheсk_Stat_ServeFailEvent_PointEvent_Backhand()
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
            myGame.AddEvent(new ServeFailEvent
            {
                OccuredAt = _gameDate2,
                Serve = ServeFailKind.Error,
                ServeSpeed = ServeSpeed.Unspecified
            });
            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Backhand
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(1);
        }

        [Fact]
        public void Cheсk_Stat_ServeFailEvent_PointEvent_Error()
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
            myGame.AddEvent(new ServeFailEvent
            {
                OccuredAt = _gameDate2,
                Serve = ServeFailKind.Error,
                ServeSpeed = ServeSpeed.Unspecified
            });
            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Error
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(1);
        }

        [Fact]
        public void Cheсk_Stat_ServeFailEvent_PointEvent_Forehand()
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
            myGame.AddEvent(new ServeFailEvent
            {
                OccuredAt = _gameDate2,
                Serve = ServeFailKind.Error,
                ServeSpeed = ServeSpeed.Unspecified
            });
            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Forehand
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(1);
        }

        [Fact]
        public void Cheсk_Stat_ServeFailEvent_PointEvent_NetPoint()
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
            myGame.AddEvent(new ServeFailEvent
            {
                OccuredAt = _gameDate2,
                Serve = ServeFailKind.Error,
                ServeSpeed = ServeSpeed.Unspecified
            });
            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.NetPoint
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(1);
        }

        [Fact]
        public void Cheсk_Stat_ServeFailEvent_PointEvent_UnforcedError()
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
            myGame.AddEvent(new ServeFailEvent
            {
                OccuredAt = _gameDate2,
                Serve = ServeFailKind.Error,
                ServeSpeed = ServeSpeed.Unspecified
            });
            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.UnforcedError
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(1);
        }

        [Fact]
        public void Cheсk_Stat_ServeFailEvent_PointEvent_Unspecified()
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
            myGame.AddEvent(new ServeFailEvent
            {
                OccuredAt = _gameDate2,
                Serve = ServeFailKind.Error,
                ServeSpeed = ServeSpeed.Unspecified
            });
            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Unspecified
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(1);
        }

        [Fact]
        public void Cheсk_Stat_ServeFailEvent_DoubleFaults()
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
            myGame.AddEvent(new ServeFailEvent
            {
                OccuredAt = _gameDate2,
                Serve = ServeFailKind.Error,
                ServeSpeed = ServeSpeed.Unspecified
            });
            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.First,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.DoubleFaults
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_ServeFailEvent_ServeFailEvent()
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
            myGame.AddEvent(new ServeFailEvent
            {
                OccuredAt = _gameDate2,
                Serve = ServeFailKind.Error,
                ServeSpeed = ServeSpeed.Unspecified
            });
            myGame.AddEvent(new ServeFailEvent
            {
                OccuredAt = _gameDate3,
                Serve = ServeFailKind.Error,
                ServeSpeed = ServeSpeed.Unspecified
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }
        
        [Fact]
        public void Cheсk_Stat_PointEvent_Ace2()
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
                PlayerServes = Player.First
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.First,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Ace
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_PointEvent_Backhand2()
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
                PlayerServes = Player.First
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.First,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Backhand
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_PointEvent_Error2()
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
                PlayerServes = Player.First
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.First,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Error
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_PointEvent_Forehand2()
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
                PlayerServes = Player.First
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.First,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Forehand
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_PointEvent_NetPoint2()
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
                PlayerServes = Player.First
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.First,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.NetPoint
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_PointEvent_UnforcedError2()
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
                PlayerServes = Player.First
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.First,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.UnforcedError
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_PointEvent_Unspecified2()
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
                PlayerServes = Player.First
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.First,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Unspecified
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }
        
        [Fact]
        public void Cheсk_Stat_PointEvent_Backhand3()
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
                PlayerServes = Player.First
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Backhand
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_PointEvent_Error3()
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
                PlayerServes = Player.First
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Error
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_PointEvent_Forehand3()
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
                PlayerServes = Player.First
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Forehand
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_PointEvent_NetPoint3()
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
                PlayerServes = Player.First
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.NetPoint
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_PointEvent_UnforcedError3()
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
                PlayerServes = Player.First
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.UnforcedError
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }

        [Fact]
        public void Cheсk_Stat_PointEvent_Unspecified3()
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
                PlayerServes = Player.First
            });
            myGame.AddEvent(new StartGameEvent { OccuredAt = _gameDate });

            myGame.AddEvent(new PointEvent
            {
                PlayerPoint = Player.Second,
                ServeSpeed = ServeSpeed.Unspecified,
                Kind = PointKind.Unspecified
            });

            myGame.PlayersStats.FirstPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.Error.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.FirstServe.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.FirstServeSuccessful.Should().Be(1);
            myGame.PlayersStats.FirstPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.TotalPoints.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.FirstPlayer.WonOnSecondServe.Should().Be(0);

            myGame.PlayersStats.SecondPlayer.Ace.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Backhand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.DoubleFaults.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Error.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.FirstServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.Forehand.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.NetPoint.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.SecondServeSuccessful.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.TotalPoints.Should().Be(1);
            myGame.PlayersStats.SecondPlayer.UnforcedError.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnFirstServe.Should().Be(0);
            myGame.PlayersStats.SecondPlayer.WonOnSecondServe.Should().Be(0);
        }
    }
}