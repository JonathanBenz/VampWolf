using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup inventoryGroup;
        [SerializeField] private InventoryBox inventoryBox;
        [SerializeField] private Button exitButton;
        private InventoryController controller;

        private EquipmentSlot currentEquipmentSlot;
        private List<EquipmentSlot> equipmentSlots;

        [Header("Tweening Variables")]
        [SerializeField] private float fadeDuration;
        private Tween fadeTween;

        public EquipmentSlot CurrentEquipmentSlot => currentEquipmentSlot;
        public IList<Equipment> Equipments => controller.Model.Equipments;

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

        private void OnDestroy()
        {
            // Kill any existing tweens
            fadeTween?.Kill();
        }

        /// <summary>
        /// Initialize the inventory view
        /// </summary>
        public void Initialize(InventoryController controller)
        {
            // Set the controller
            this.controller = controller;

            // Get components
            inventoryGroup = GetComponent<CanvasGroup>();

            // Initialize the inventory box
            inventoryBox.Initialize(this);

            // Initialize the slots
            equipmentSlots = new List<EquipmentSlot>();

            // Store all the slots in the inventory
            GetComponentsInChildren(equipmentSlots);

            // Iterate through each equipment slot
            foreach (EquipmentSlot slot in equipmentSlots)
            {
                // Initialize the equipment slot
                slot.Initialize();

                // Add a listener to the equipment slot
                slot.EmptySlotClicked += ShowInventoryBox;

                slot.FilledSlotClicked += RemoveEquipment;
            }

            // Register listeners to the inventory box slots
            inventoryBox.RegisterOnClickListeners(SetEquipment);

            // Set the functionality for the exit button
            exitButton.onClick.AddListener(() =>
            {
                EventBus<HideInventory>.Raise(new HideInventory());
            });
        }

        /// <summary>
        /// Show the inventory box when an equipment slot is clicked
        /// </summary>
        private void ShowInventoryBox(EquipmentSlot slot, RectTransform rectTransform)
        {
            // Set the equipment slot
            currentEquipmentSlot = slot;

            // Show the inventory box
            inventoryBox.Show(rectTransform, slot.SlotType);
        }

        /// <summary>
        /// Set the equipment in the current slot
        /// </summary>
        public void SetEquipment(Equipment equipment)
        {
            // Set the equipment in the current slot
            currentEquipmentSlot.Set(equipment);

            // Hide the inventory box
            inventoryBox.Hide();
        }

        /// <summary>
        /// Remove the equipment in the current slot
        /// </summary>
        public void RemoveEquipment(EquipmentSlot slot)
        {
            // Set the current slot
            currentEquipmentSlot = slot;

            // Clear the current equipment slot
            currentEquipmentSlot.Clear();

            // Update the inventory display
            inventoryBox.UpdateInventoryDisplay(slot.Equipment);

            // Hide the inventory box
            inventoryBox.Hide();
        }

        /// <summary>
        /// Add an equipment to the inventory
        /// </summary>
        public void AddEquipment(Equipment equipment) => inventoryBox.AddEquipment(equipment);

        /// <summary>
        /// Show the inventory screen
        /// </summary>
        public void Show()
        {
            Fade(1f, fadeDuration, () =>
            {
                inventoryGroup.interactable = true;
                inventoryGroup.blocksRaycasts = true;
            });
        }

        /// <summary>
        /// Hide the inventory screen
        /// </summary>
        public void Hide()
        {
            Fade(0f, fadeDuration, () =>
            {
                inventoryGroup.interactable = false;
                inventoryGroup.blocksRaycasts = false;
            });

            // Hide the inventory box
            inventoryBox.Hide();
        }

        /// <summary>
        /// Handle the fading of the inventory screen
        /// </summary>
        private void Fade(float endValue, float duration, TweenCallback onComplete = null)
        {
            // Kill any existing tweens
            fadeTween?.Kill();

            // Create a new tween
            fadeTween = inventoryGroup.DOFade(endValue, duration);

            // Set easing
            fadeTween.SetEase(Ease.InOutSine);

            // Exit case - if there's no completion action
            if (onComplete == null) return;

            // Set the completion action
            fadeTween.OnComplete(onComplete);
        }
    }
}
