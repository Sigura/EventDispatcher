namespace Dispatcher.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class Extensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (action == null)
                return enumerable;

            var forEach = enumerable as T[] ?? enumerable.ToArray();

            foreach (var x in forEach)
            {
                action(x);
            }

            return forEach;
        }
    }
}
