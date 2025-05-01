using System.Collections.Generic;
using UnityEngine;

namespace Vampwolf.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InventoryBox inventoryBox;

        private EquipmentSlot currentSlot;
        private List<EquipmentSlot> equipmentSlots;
        private List<InventorySlot> inventorySlots;

        private void OnDisable()
        {
            // Deregister the listeners
            foreach (EquipmentSlot slot in equipmentSlots)
            {
                slot.EmptySlotClicked -= ShowInventoryBox;
            }

            // Deregister the inventory box listeners
            inventoryBox.DeregisterOnClickListeners(SetEquipment);
        }

        /// <summary>
        /// Initialize the inventory view
        /// </summary>
        public void Initialize()
        {
            // Initialize the inventory box
            inventoryBox.Initialize();

            // Initialize the slots
            equipmentSlots = new List<EquipmentSlot>();
            inventorySlots = new List<InventorySlot>();

            // Store all the slots in the inventory
            GetComponentsInChildren(equipmentSlots);
            GetComponentsInChildren(inventorySlots);

            // Iterate through each equipment slot
            foreach (EquipmentSlot slot in equipmentSlots)
            {
                // Initialize the equipment slot
                slot.Initialize();

                // Add a listener to the equipment slot
                slot.EmptySlotClicked += ShowInventoryBox;
            }

            inventoryBox.RegisterOnClickListeners(AddEquipment);
        }

        /// <summary>
        /// Show the inventory box when an equipment slot is clicked
        /// </summary>
        private void ShowInventoryBox(EquipmentSlot slot, RectTransform rectTransform)
        {
            if (slot.Equipment == null) return;

            // Set the equipment slot
            currentSlot = slot;

            // Show the inventory box
            inventoryBox.Show(rectTransform, slot.Equipment.User);
        }

        public void SetEquipment(Equipment equipment)
        {
            // Set the equipment in the current slot
            currentSlot.Set(equipment);

            // Hide the inventory box
            inventoryBox.Hide();
        }

        /// <summary>
        /// Add an equipment to the inventory
        /// </summary>
        public void AddEquipment(Equipment equipment)
        {
            
        }
    }
}
