using System.Collections.Generic;
using System;
using System.Linq;
using Vampwolf.Shop;
using Vampwolf.Utilities.ObservableList;

namespace Vampwolf.Inventory
{
    public class InventoryModel
    {
        private readonly ObservableList<Equipment> equipments;

        public ObservableList<Equipment> Equipments => equipments;

        public InventoryModel()
        {
            // Initialize the list
            equipments = new ObservableList<Equipment>();
        }

        /// <summary>
        /// Add an item to the inventory
        /// </summary>
        public void Add(Item item)
        {
            // Create an equipment from an item
            Equipment equipment = new Equipment(item);

            // Try to find the equipment in the stock
            Equipment equipmentCheck = equipments.FirstOrDefault(check => check.Name == equipment.Name);

            // Exit case - the equipment is already in the inventory
            if (equipmentCheck != null) return;

            // Add the equipment to the inventory
            equipments.Add(equipment);

            // Notify that the inventory has changed
            equipments.Invoke();
        }

        public void Remove(Equipment equipment)
        {
            // Exit case - the equipment is not in the inventory
            if (!equipments.Contains(equipment)) return;

            // Remove the equipment from the inventory
            equipments.Remove(equipment);

            // Notify that the inventory has changed
            equipments.Invoke();
        }

        /// <summary>
        /// Add an event listener to the inventory changed event
        /// </summary>
        public void AddListener(Action<IList<Equipment>> action) => equipments.AnyValueChanged += action;

        /// <summary>
        /// Remove an event listener from the inventory changed event
        /// </summary>
        public void RemoveListener(Action<IList<Equipment>> action) => equipments.AnyValueChanged -= action;
    }
}
