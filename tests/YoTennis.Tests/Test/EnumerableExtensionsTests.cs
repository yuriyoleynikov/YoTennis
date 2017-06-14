using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using YoTennis.Controllers;
using YoTennis.Helpers;

namespace YoTennis.Tests.Test
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void Test_WithElement_String()
        {
            IEnumerable<string> sequence = new List<string> { "one", "two" };
            sequence.WithElement("three");
            sequence.Should().BeEquivalentTo(new List<string> { "one", "two", "three" });
        }

        [Fact]
        public void Test_WithElement_Int()
        {
            IEnumerable<int> sequence = new List<int> { 1, 2 };
            sequence.WithElement(3);
            sequence.Should().BeEquivalentTo(new List<int> { 1, 2, 3 });
        }

        [Fact]
        public void Test_WithoutElement_String()
        {
            IEnumerable<string> sequence = new List<string> { "one", "two" };
            sequence.WithoutElement("two");
            sequence.Should().BeEquivalentTo(new List<string> { "one" });
        }

        [Fact]
        public void Test_WithoutElement_Int()
        {
            IEnumerable<int> sequence = new List<int> { 1, 2 };
            sequence.WithoutElement(2);
            sequence.Should().BeEquivalentTo(new List<int> { 1 });
        }

        [Fact]
        public void Test_Null_WithElement_String()
        {
            IEnumerable<string> sequence = null;
            sequence.WithElement("one");
            sequence.Should().BeEquivalentTo(new List<string> { "one" });
        }

        [Fact]
        public void Test_Null_WithElement_Int()
        {
            IEnumerable<string> sequence = null;
            sequence.WithElement(1);
            sequence.Should().BeEquivalentTo(new List<int> { 1 });
        }
    }
}
