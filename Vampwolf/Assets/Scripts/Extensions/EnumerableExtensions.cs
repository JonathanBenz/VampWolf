using System;
using System.Collections.Generic;

namespace Vampwolf.Extensions.Enumerables
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Performs an action on each element in the sequence
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            // Iterate through each item in the sequence
            foreach (T item in sequence)
            {
                // Perform the action on the item
                action(item);
            }
        }
    }
}
