using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Utilities
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> target, Action<T> invokeThis)
        {
            if (target == null) return;

            foreach (var item in target)
            {
                invokeThis(item);
            }
        }

        public static T Second<T>(this IEnumerable<T> target)
        {
            if (target == null) return default(T);
            if (target.Count() <= 1) new InvalidOperationException("Not enough elements.");

            return target.Skip(1).First();
        }

        public static IEnumerable<List<T>> Partition<T>(this IEnumerable<T> source, Int32 size)
        {
            for (int i = 0; i < Math.Ceiling(source.Count() / (Double)size); i++)
                yield return new List<T>(source.Skip(size * i).Take(size));
        }
    }
}
