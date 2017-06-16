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
            if (source != null)
                foreach (T t in source)
                    yield return t;

            yield return element;
        }

        public static IEnumerable<T> WithoutElement<T>(this IEnumerable<T> source, T element, IEqualityComparer<T> comparer = null)
        {
            if (source != null)
            {
                if (comparer == null)
                    comparer = EqualityComparer<T>.Default;
                foreach (T t in source)
                    if (!comparer.Equals(element, t))
                        yield return t;
            }
        }
    }
}
