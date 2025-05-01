using System.Collections.Generic;
using UnityEngine;

namespace Vampwolf.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        private EquipmentSlot currentSlot;
        private List<EquipmentSlot> slots;

        /// <summary>
        /// Initialize the inventory view
        /// </summary>
        public void Initialize()
        {
            // Store all the equipemnt slots in the inventory
            GetComponentsInChildren(slots);

            foreach(EquipmentSlot slot in slots)
            {
                // Initialize the equipment slot
                slot.Initialize();

                // Add a listener to the equipment slot
                slot.EmptySlotClicked += ShowEquipment;
            }
        }

        private void ShowEquipment(EquipmentSlot slot)
        {
            // Set the equipment slot
            currentSlot = slot;

            Debug.Log("Showing Equipment Slot");

        }

        /// <summary>
        /// Add an equipment to the inventory
        /// </summary>
        public void AddEquipment(Equipment equipment)
        {

        }
    }
}
