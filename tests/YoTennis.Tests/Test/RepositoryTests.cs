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
    public abstract class RepositoryTests
    {
        public abstract IMatchListService GetRepository();

        [Fact]
        public async Task Check_MyAsync()
        {
            IMatchListService repository = GetRepository();

            await repository.CreateMatch("user");
            var test = repository.GetMatches("user");
            var t2 = await test;
        }
    }
}