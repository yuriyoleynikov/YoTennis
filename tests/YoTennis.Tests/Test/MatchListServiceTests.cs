using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YoTennis.Models;
using YoTennis.Models.Events;
using YoTennis.Services;

namespace YoTennis.Tests.Test
{
    public abstract class MatchListServiceTests
    {
        public abstract IMatchListService GetMatchListService();

        [Fact]
        public async Task CreateMatch_Check()
        {
            IMatchListService matchListService = GetMatchListService();

            await matchListService.CreateMatch("user");
            var match = await matchListService.GetMatches("user");
            match.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetMatches_Check()
        {
            IMatchListService matchListService = GetMatchListService();

            await matchListService.CreateMatch("user");
            var match = await matchListService.GetMatches("user");
            match.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetMatches_Check_Empty()
        {
            IMatchListService matchListService = GetMatchListService();

            await matchListService.CreateMatch("user");
            var match = await matchListService.GetMatches("user2");
            match.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task DeleteMatch_Cheсk()
        {
            IMatchListService matchListService = GetMatchListService();

            await matchListService.CreateMatch("user");
            var matchIds = await matchListService.GetMatches("user");
            var matchId = matchIds.SingleOrDefault();

            await matchListService.DeleteMatch("user", matchId);
            var matches = await matchListService.GetMatches("user");
            matches.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task DeleteMatch_Cheсk_Not_This_User()
        {
            IMatchListService matchListService = GetMatchListService();

            await matchListService.CreateMatch("user");
            var matchIds = await matchListService.GetMatches("user");
            var matchId = matchIds.SingleOrDefault();

            new Func<Task>(() => matchListService.DeleteMatch("user1", matchId)).ShouldThrow<KeyNotFoundException>();
        }

        [Fact]
        public async Task DeleteMatch_Cheсk_Not_This_Match()
        {
            IMatchListService matchListService = GetMatchListService();
            
            new Func<Task>(() => matchListService.DeleteMatch("user", "otherId"))
                .ShouldThrow<KeyNotFoundException>();
        }

        [Fact]
        public async Task DeleteMatch_Cheсk_Not_This_User_and_Match()
        {
            IMatchListService matchListService = GetMatchListService();

            await matchListService.CreateMatch("user");
            var matchIds = await matchListService.GetMatches("user");
            var matchId = matchIds.SingleOrDefault();

            new Func<Task>(() => matchListService.DeleteMatch("otherUser", "otherId"))
                .ShouldThrow<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetMatchService_Cheсk()
        {
            IMatchListService matchListService = GetMatchListService();

            await matchListService.CreateMatch("user");
            var matchIds = await matchListService.GetMatches("user");
            var matchId = matchIds.SingleOrDefault();
            var matchService = await matchListService.GetMatchService("user", matchId);
            matchService.Should().NotBeNull();
        }

        [Fact]
        public async Task GetMatchService_Cheсk_Not_This_User()
        {
            IMatchListService matchListService = GetMatchListService();

            await matchListService.CreateMatch("user");
            var matchIds = await matchListService.GetMatches("user");
            var matchId = matchIds.SingleOrDefault();
            new Func<Task>(() => matchListService.GetMatchService("otherUser", matchId))
                .ShouldThrow<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetMatchService_Cheсk_Not_This_Match()
        {
            IMatchListService matchListService = GetMatchListService();

            await matchListService.CreateMatch("user");
            var matchIds = await matchListService.GetMatches("user");
            var matchId = matchIds.SingleOrDefault();
            new Func<Task>(() => matchListService.GetMatchService("user", "otherId"))
                .ShouldThrow<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetMatchService_Cheсk_Not_This_User_and_Match()
        {
            IMatchListService matchListService = GetMatchListService();

            await matchListService.CreateMatch("user");
            var matchIds = await matchListService.GetMatches("user");
            var matchId = matchIds.SingleOrDefault();
            new Func<Task>(() => matchListService.GetMatchService("otherUser", "otherId"))
                .ShouldThrow<KeyNotFoundException>();
        }
    }
}