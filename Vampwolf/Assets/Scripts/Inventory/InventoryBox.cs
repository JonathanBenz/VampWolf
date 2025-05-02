using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Vampwolf.Shop;

namespace Vampwolf.Inventory
{
    public class InventoryBox : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InventorySlot slotPrefab;
        [SerializeField] private Transform slotParent;
        [SerializeField] private Image frame;
        private InventoryView view;
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        private InventorySlotPool slotPool;
        

        [Header("Fields")]
        [SerializeField] private int initialSlotsNum;
        [SerializeField] private Color vampireColor;
        [SerializeField] private Color werewolfColor;
        private List<InventorySlot> slots;

        private void OnDestroy()
        {
            // Deregister the on-click listeners for the slots
            DeregisterOnClickListeners(UpdateInventoryDisplay);
        }

        /// <summary>
        /// Initialize the inventory box
        /// </summary>
        public void Initialize(InventoryView view)
        {
            // Set the view
            this.view = view;

            // Initialize the slots list
            slots = new List<InventorySlot>();

            // Get componnets
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();

            // Initialize the slot pool
            slotPool = new InventorySlotPool(slotPrefab, slotParent);

            // Iterate a number of times equal to the initial slots number
            for (int i = 0; i < initialSlotsNum; i++)
            {
                // Get a slot from the pool
                InventorySlot slot = slotPool.Get();

                // Initialize the slot with its index
                slot.Initialize(i);

                // Add the slot to the list
                slots.Add(slot);
            }

            // Register the on-click listeners for the slots
            RegisterOnClickListeners(UpdateInventoryDisplay);
        }

        /// <summary>
        /// Add an equipment to the inventory box
        /// </summary>
        public void AddEquipment(Equipment equipment)
        {
            // Insert index
            int index = -1;

            // Iterate through each slot
            for(int i = 0; i < slots.Count; i++)
            {
                // Skip if the slot has an equipment
                if (slots[i].Equipment != null) continue;

                // Set the index
                index = i;

                break;
            }

            // Check if a valid index was not found
            if(index == -1)
            {
                // Iterate three times
                for(int i = 0; i < 3; i++)
                {
                    // Get a slot from the pool
                    InventorySlot slot = slotPool.Get();

                    // Initialize the slot with its index
                    slot.Initialize(slots.Count + i);

                    // Add the slot to the list
                    slots.Add(slot);

                    // Skip if not the first slot
                    if (i != 0) continue;

                    // Set the slot
                    slot.Set(equipment);
                }

                return;
            }

            // Set the slot
            slots[index].Set(equipment);
        }

        /// <summary>
        /// Register on-click listeners for the inventory slots
        /// </summary>
        public void RegisterOnClickListeners(Action<Equipment> action)
        {
            // Iterate through each slot
            foreach (InventorySlot slot in slots)
            {
                // Register the listener
                slot.InventorySlotClicked += action;
            }
        }

        /// <summary>
        /// Deregister on-click listeners for the inventory slots
        /// </summary>
        public void DeregisterOnClickListeners(Action<Equipment> action)
        {
            // Iterate through each slot
            foreach (InventorySlot slot in slots)
            {
                // Deregister the listener
                slot.InventorySlotClicked -= action;
            }
        }

        /// <summary>
        /// Show the inventory box
        /// </summary>
        public void Show(RectTransform rectTransform, UserType user)
        {
            // Set the anchored position
            float y = rectTransform.anchoredPosition.y;
            this.rectTransform.anchoredPosition = new Vector2(this.rectTransform.anchoredPosition.x, y);

            // Change the color of the frame based on the user type
            switch (user)
            {
                case UserType.Vampire:
                    frame.color = vampireColor;
                    break;
                case UserType.Werewolf:
                    frame.color = werewolfColor;
                    break;
            }

            // Update the inventory display
            UpdateInventoryDisplay(null);

            // Enable the canvas group
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// Hide the inventory box
        /// </summary>
        public void Hide()
        {
            // Disable the canvas group
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// Update the items shown in the inventory box
        /// </summary>
        public void UpdateInventoryDisplay(Equipment equipment)
        {
            // Filter the slots based on the user
            List<Equipment> matchingSlots = view.Equipments.Where(slot => slot.User == view.CurrentEquipmentSlot.SlotType && !slot.Equipped).ToList();

            // Iterate through each base slot
            for (int i = 0; i < slots.Count; i++)
            {
                // Clear the slot
                slots[i].Clear();
            }

            // Iterate through each matching slot
            for (int i = 0; i < matchingSlots.Count; i++)
            {
                // Set the slot
                slots[i].Set(matchingSlots[i]);
            }
        }
    }
}
