using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballPredictor.Common
{
    public static class EnumerableExtensions
    {
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null || !source.Any()) return;

            foreach (var item in source)
            {
                action(item);
            }
        }

        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source) => source == null || !source.Any();

        /// <summary>
        /// Adds only distinct items to the source. Able to pass in an optional <see cref="IEqualityComparer{T}"/> to configure
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="items"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> AddRangeDistinct<TSource>(
            this IList<TSource> source,
            IEnumerable<TSource> items,
            IEqualityComparer<TSource> comparer = default)
        {
            if (items.IsNullOrEmpty()) return source;
            if (source.IsNullOrEmpty()) source = new List<TSource>(items?.Count() ?? 0);

            foreach (var item in items)
            {
                if (!source.Contains(item, comparer))
                {
                    source.Add(item);
                }
            }
            return source;
        }
    }
}
