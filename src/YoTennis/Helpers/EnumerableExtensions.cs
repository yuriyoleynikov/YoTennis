using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Helpers
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> WithElement<T>(this IEnumerable<T> source, T element)
        {
            foreach (T t in source)
                yield return t;

            yield return element;
            yield break;
        }

        public static IEnumerable<T> WithoutElement<T>(this IEnumerable<T> source, T element)
        {
            foreach (T t in source)
                if (!element.Equals(t))
                    yield return t;

            yield break;
        }
    }
}
