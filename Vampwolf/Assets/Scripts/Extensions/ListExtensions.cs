using System.Collections.Generic;

namespace Vampwolf.Extensions.List
{
    public static class ListExtensions
    {
        /// <summary>
        /// Refresh the list with a new set of items
        /// </summary>
        public static void RefreshWith<T>(this List<T> list, IEnumerable<T> items)
        {
            // Clear the list
            list.Clear();

            // Add all of the given items
            list.AddRange(items);
        }

        /// <summary>
        /// Move an item from one index to another
        /// </summary>
        public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
        {
            // Exit case - the old index is the same as the new index
            if (oldIndex == newIndex) return;

            // Get the item at the old index
            T item = list[oldIndex];

            // Remove the item from the old index
            list.RemoveAt(oldIndex);

            // Insert the item at the new index
            list.Insert(newIndex, item);
        }
    }
}
