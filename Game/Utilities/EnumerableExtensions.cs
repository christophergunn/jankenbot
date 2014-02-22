using System;
using System.Collections.Generic;

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
    }
}
