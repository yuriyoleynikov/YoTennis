using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using YoTennis.Controllers;

namespace YoTennis.Tests.Test
{
    public class CorrectPaginationTests
    {
        private static void TestCase(
            int totalCount, 
            int requestedCount, 
            int requestedSkip, 
            int defaultCount, 
            int expectedCount, 
            int expectedSkip)
        {
            var (count, skip) = 
                HomeController.CorrectPagination(totalCount, requestedCount, requestedSkip, defaultCount);

            count.Should().BePositive();
            skip.Should().BeGreaterOrEqualTo(0);
            Assert.True(skip == 0 || skip < totalCount);
            Assert.True(count == requestedCount || requestedCount <= 0 && count == defaultCount);
            Assert.True(skip == requestedSkip || requestedSkip < 0 || requestedSkip >= totalCount);
            count.Should().Be(expectedCount);
            skip.Should().Be(expectedSkip);
        }

        [Fact]
        public void Tests()
        {
            //for totalCount
            TestCase(100, 10, 0, 10, 10, 0);
            TestCase(10, 10, 0, 10, 10, 0);
            TestCase(0, 10, 0, 10, 10, 0);
            new Action(() => TestCase(-10, 10, 0, 10, 10, 0))
                .ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("totalCount");
            new Action(() => TestCase(-100, 10, 0, 10, 10, 0))
                .ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("totalCount");
            
            //for requestedCount
            TestCase(100, 1000, 0, 10, 1000, 0);
            TestCase(100, 110, 0, 10, 110, 0);
            TestCase(100, 100, 0, 10, 100, 0);
            TestCase(100, 20, 0, 10, 20, 0);
            TestCase(100, 0, 0, 10, 10, 0);
            TestCase(100, -10, 0, 10, 10, 0);
            TestCase(100, -100, 0, 10, 10, 0);

            TestCase(0, 20, 0, 10, 20, 0);

            //for requestedSkip
            TestCase(100, 10, 1000, 10, 10, 90);
            TestCase(100, 10, 100, 10, 10, 90);
            TestCase(100, 10, 50, 10, 10, 50);
            TestCase(100, 10, 10, 10, 10, 10);
            TestCase(100, 10, 0, 10, 10, 0);
            TestCase(100, 10, -10, 10, 10, 0);
            TestCase(100, 10, -100, 10, 10, 0);
            TestCase(100, 10, -1000, 10, 10, 0);

            TestCase(0, 10, 10, 10, 10, 0);

            //for defaultCount
            TestCase(100, 10, 0, 100, 10, 0);
            TestCase(100, 0, 0, 100, 100, 0);
            new Action(() => TestCase(100, 10, 0, 0, 100, 0))
                .ShouldThrow<ArgumentOutOfRangeException>().And.ParamName.Should().Be("defaultCount");

            //for requestedCount and requestedSkip
            TestCase(100, 0, 1000, 10, 10, 90);
            TestCase(0, 0, 1000, 10, 10, 0);
        }
    }
}
