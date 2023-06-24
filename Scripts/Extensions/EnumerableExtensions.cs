namespace UniT.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        public static (List<T>, List<T>) Split<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            return enumerable.Aggregate((new List<T>(), new List<T>()), (lists, item) =>
            {
                if (predicate(item)) lists.Item1.Add(item);
                else lists.Item2.Add(item);
                return lists;
            });
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.OrderBy(_ => Guid.NewGuid());
        }

        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> enumerable, int count = -1)
        {
            if (count == 0) yield break;
            var cache = new List<T>();
            foreach (var item in enumerable)
            {
                yield return item;
                cache.Add(item);
            }

            while (--count != 0)
            {
                foreach (var item in cache)
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> Slice<T>(this IEnumerable<T> enumerable, int start, int stop, int step = 1)
        {
            var index = 0;
            foreach (var item in enumerable)
            {
                if (index >= stop) yield break;
                if (index >= start && (index - start) % step == 0) yield return item;
                ++index;
            }
        }
    }
}