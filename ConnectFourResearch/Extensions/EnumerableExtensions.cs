using System;
using System.Collections.Generic;
using System.Linq;

namespace ConnectFourResearch.Extensions
{
    public static class EnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            var i = 0;
            foreach (var item in items)
            {
                if (predicate(item)) return i;
                i++;
            }

            return -1;
        }
        
        public static T MinBy<T>(this IEnumerable<T> items, Func<T, IComparable> getKey)
        {
            var best = default(T);
            IComparable bestKey = null;
            foreach (var item in items)
                if (bestKey == null || getKey(item).CompareTo(bestKey) < 0)
                {
                    best = item;
                    bestKey = getKey(best);
                }

            return best;
        }
        
        public static T MaxBy<T>(this IEnumerable<T> items, Func<T, IComparable> getKey)
        {
            var best = default(T);
            IComparable bestKey = null;
            foreach (var item in items)
                if (bestKey == null || getKey(item).CompareTo(bestKey) > 0)
                {
                    best = item;
                    bestKey = getKey(best);
                }

            return best;
        }
        
        public static double MaxOrDefault<T>(this IEnumerable<T> items, 
            Func<T, double> getKey, double defaultValue)
        {
            var bestCost = double.NegativeInfinity;
            foreach (var item in items)
            {
                var cost = getKey(item);
                if (cost > bestCost)
                    bestCost = cost;
            }

            return double.IsNegativeInfinity(bestCost) ? defaultValue : bestCost;
        }
        
        public static IEnumerable<T> Times<T>(this int count, Func<int, T> create)
        {
            return Enumerable.Range(0, count).Select(create);
        }

        public static IEnumerable<T> Times<T>(this int count, T item)
        {
            return Enumerable.Repeat(item, count);
        }
        
        public static int IndexOf<T>(this IReadOnlyList<T> readOnlyList, T value)
        {
            var count = readOnlyList.Count;
            var equalityComparer = EqualityComparer<T>.Default;
            for (var i = 0; i < count; i++)
            {
                var current = readOnlyList[i];
                if (equalityComparer.Equals(current, value)) return i;
            }

            return -1;
        }
        
        public static int ElementwiseHashcode<T>(this IEnumerable<T> items)
        {
            unchecked
            {
                return items
                    .Select(t => t?.GetHashCode() ?? 0)
                    .Aggregate((res, next) => (res * 379) ^ next);
            }
        }

        public static List<T> Shuffle<T>(this IEnumerable<T> items, Random random)
        {
            var copy = items.ToList();
            for (var i = 0; i < copy.Count; i++)
            {
                var nextIndex = random.Next(i, copy.Count);
                var t = copy[nextIndex];
                copy[nextIndex] = copy[i];
                copy[i] = t;
            }

            return copy;
        }
    }
}