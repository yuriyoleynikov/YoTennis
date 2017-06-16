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
            var sequence = new[] { "one", "two" };
            var result = sequence.WithElement("three");
            result.Should().BeEquivalentTo(new[] { "one", "two", "three" });
        }

        [Fact]
        public void Test_WithElement_Int()
        {
            var sequence = new[] { 1, 2 };
            var result = sequence.WithElement(3);
            result.Should().BeEquivalentTo(new[] { 1, 2, 3 });
        }

        [Fact]
        public void Test_WithoutElement_String()
        {
            var sequence = new[] { "one", "two" };
            var result = sequence.WithoutElement("two");
            result.Should().BeEquivalentTo(new[] { "one" });
        }

        [Fact]
        public void Test_WithoutElement_Int()
        {
            var sequence = new[] { 1, 2 };
            var result = sequence.WithoutElement(2);
            result.Should().BeEquivalentTo(new[] { 1 });
        }

        [Fact]
        public void Test_Null_WithElement_String()
        {
            IEnumerable<string> sequence = null;
            var result = sequence.WithElement("one");
            result.Should().BeEquivalentTo(new[] { "one" });
        }

        [Fact]
        public void Test_Null_WithElement_Int()
        {
            IEnumerable<int> sequence = null;
            var result = sequence.WithElement(1);
            result.Should().BeEquivalentTo(new[] { 1 });
        }

        [Fact]
        public void Test_Null_WithoutElement_String()
        {
            IEnumerable<string> sequence = null;
            var result = sequence.WithoutElement("one");
            result.Should().BeEquivalentTo(new string[] { });
        }

        [Fact]
        public void Test_Null_WithoutElement_Int()
        {
            IEnumerable<int> sequence = null;
            var result = sequence.WithoutElement(1);
            result.Should().BeEquivalentTo(new int[] { });
        }

        [Fact]
        public void Test_WithElement_String_WithNull()
        {
            var sequence = new[] { null, "two" };
            var result = sequence.WithElement("three");
            result.Should().BeEquivalentTo(new[] { null, "two", "three" });
        }

        [Fact]
        public void Test_WithoutElement_String_WithNull_WithCompare()
        {
            var forCompare = StringComparer.OrdinalIgnoreCase;
            var sequence = new[] { null, "two", "three" };
            var result = sequence.WithoutElement("tWo", forCompare);
            result.Should().BeEquivalentTo(new string[] { null, "three" });
        }

        [Fact]
        public void Test_WithoutElement_String_WithNull_WithCompare2()
        {
            var forFirstCompare = new FirstCharEqualityComparer();
            var sequence = new[] { null, "two", "three" };
            var result = sequence.WithoutElement("tWo", forFirstCompare);
            result.Should().BeEquivalentTo(new string[] { null });
        }

        [Fact]
        public void Test_WithoutElement_String_WithCompare()
        {
            var forFirstCompare = new FirstCharEqualityComparer();
            var sequence = new[] { "one", "two", "three" };
            var result = sequence.WithoutElement("five", forFirstCompare);
            result.Should().BeEquivalentTo(new[] { "one", "two", "three" });
        }

        [Fact]
        public void Test_WithoutElement_Int_WithCompare3()
        {
            var compare = new IntEqualityComparer();
            var sequence = new[] { 11, 22, 33 };
            var result = sequence.WithoutElement(20, compare);
            result.Should().BeEquivalentTo(new[] { 11, 33 });
        }

        [Fact]
        public void Test_WithoutElement_Int_WithCompare()
        {
            var compare = new IntEqualityComparer();
            var sequence = new[] { 11, 22, 33 };
            var result = sequence.WithoutElement(44, compare);
            result.Should().BeEquivalentTo(new[] { 11, 22, 33 });
        }

        [Fact]
        public void Test_WithoutElement_Int_WithCompare4()
        {
            var compare = new IntDigitEqualityComparer();
            var sequence = new[] { 1, 11, 111, 1111 };
            var result = sequence.WithoutElement(888, compare);
            result.Should().BeEquivalentTo(new[] { 1, 11, 1111 });
        }

        [Fact]
        public void Test_WithoutElement_Int_WithCompare5()
        {
            var compare = new IntDigitEqualityComparer();
            var sequence = new[] { 1, 11, 111, 1111 };
            var result = sequence.WithoutElement(88888, compare);
            result.Should().BeEquivalentTo(new[] { 1, 11, 111, 1111 });
        }

        [Fact]
        public void Test_WithoutElement_Int_WithCompare6()
        {
            var compare = new IntDigitEqualityComparer();
            var sequence = new[] { 1, -11, 111, 1111 };
            var result = sequence.WithoutElement(22, compare);
            result.Should().BeEquivalentTo(new[] { 1, 111, 1111 });
        }

        class FirstCharEqualityComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                if (x == null)
                    return y == null;
                if (y == null)
                    return false;
                if (x.Length == 0)
                    return y.Length == 0;
                if (y.Length == 0)
                    return false;
                return x[0] == y[0];
            }

            public int GetHashCode(string obj)
            {
                throw new NotImplementedException();
            }
        }

        class IntEqualityComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return x / 10 == y / 10;
            }

            public int GetHashCode(int obj)
            {
                throw new NotImplementedException();
            }
        }

        class IntDigitEqualityComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                if (x > -10 && x < 10 && y > -10 && y < 10)
                    return true;

                while (true)
                {
                    x = x / 10;
                    y = y / 10;

                    if (x == 0 || y == 0)
                    {
                        if (x == y)
                            return true;
                        return false;
                    }
                }
            }

            public int GetHashCode(int obj)
            {
                throw new NotImplementedException();
            }
        }
    }
}
