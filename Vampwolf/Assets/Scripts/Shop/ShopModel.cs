using System;
using System.Collections.Generic;
using System.Linq;
using Vampwolf.Utilities.ObservableList;

namespace Vampwolf.Shop
{
    public class ShopModel
    {
        private readonly ObservableList<Item> itemStock;

        public ObservableList<Item> ItemStock => itemStock;

        public ShopModel()
        {
            // Initialize the list
            itemStock = new ObservableList<Item>();
        }

        /// <summary>
        /// Adds an item to the stock
        /// </summary>
        public void Add(ItemData data)
        {
            // Create a new item and add it to the stock
            Item newItem = new Item(data);
            itemStock.Add(newItem);

            // Notify that the item stock has changed
            itemStock.Invoke();
        }

        /// <summary>
        /// Remove an item from the stock
        /// </summary>
        public void Remove(ItemData data)
        {
            // Find the item in the stock
            Item itemToRemove = itemStock.FirstOrDefault(item => item.Name == data.Name);

            // Exit case - if the item is not found
            if (itemToRemove == null) return;

            // Remove the item from the stock
            itemStock.Remove(itemToRemove);

            // Notify that the item stock has changed
            itemStock.Invoke();
        }

        /// <summary>
        /// Add an event listener to the item stock event
        /// </summary>
        public void AddListener(Action<IList<Item>> action) => itemStock.AnyValueChanged += action;

        /// <summary>
        /// Remove an event listener from the item stock event
        /// </summary>
        public void RemoveListener(Action<IList<Item>> action) => itemStock.AnyValueChanged -= action;
    }
}
