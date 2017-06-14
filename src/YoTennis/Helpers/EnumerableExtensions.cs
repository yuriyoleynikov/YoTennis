using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Helpers
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> WithElement<T>(this IEnumerable<T> source, T element)
        {
            //
            yield break;
        }

        public static IEnumerable<T> WithoutElement<T>(this IEnumerable<T> source, T element)
        {
            //
            yield break;
        }
    }
}
