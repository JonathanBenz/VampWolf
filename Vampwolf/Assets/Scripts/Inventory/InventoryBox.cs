using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vampwolf.Shop;

namespace Vampwolf.Inventory
{
    public class InventoryBox : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InventorySlot slotPrefab;
        [SerializeField] private Transform slotParent;
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        private InventorySlotPool slotPool;

        [Header("Fields")]
        [SerializeField] private int initialSlotsNum;
        private List<InventorySlot> slots;

        /// <summary>
        /// Initialize the inventory box
        /// </summary>
        public void Initialize()
        {
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
            float x = rectTransform.anchoredPosition.x;
            float y = rectTransform.anchoredPosition.y - (rectTransform.rect.size.y * 0.67f);
            this.rectTransform.anchoredPosition = new Vector2(x, y);

            // Filter the slots based on the user
            List<InventorySlot> matchingSlots = slots.Where(slot => slot.Equipment.User == user).ToList();

            // Iterate through each base slot
            for (int i = 0; i < slots.Count; i++)
            {
                // Clear the slot
                slots[i].Clear();
            }

            // Iterate through each matching slot
            for(int i = 0; i < matchingSlots.Count; i++)
            {
                // Set the slot
                slots[i].Set(matchingSlots[i].Equipment);
            }

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
    }
}
