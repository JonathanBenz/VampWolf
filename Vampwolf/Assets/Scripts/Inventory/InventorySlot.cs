using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Vampwolf.Inventory
{
    public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("References")]
        [SerializeField] private Image highlightBorder;
        [SerializeField] private Image equipmentIcon;
        [SerializeField] private CanvasGroup emptyGroup;
        [SerializeField] private CanvasGroup filledGroup;
        private Button button;

        private Equipment equipment;

        [Header("Tweening Variables")]
        [SerializeField] private float highlightDuration;
        private Tween highlightTween;

        public event Action<Equipment> InventorySlotClicked = delegate { };

        public Equipment Equipment => equipment;

        private void OnDestroy()
        {
            // Kill any existing tweens
            highlightTween?.Kill();
        }

        /// <summary>
        /// Initialize the Inventory Slot
        /// </summary>
        public void Initialize(int index)
        {
            // Get components
            button = GetComponent<Button>();

            // Set the button on-click listener
            button.onClick.AddListener(OnClick);

            // Set the name of the inventory slot
            gameObject.name = $"Inventory Slot {index}";
        }

        /// <summary>
        /// Process logic for when the equipment slot is clicked
        /// </summary>
        private void OnClick()
        {
            // Exit case - there's no equipment attached to this slot
            if (equipment == null) return;

            // Notify that this equipment slot has been clicked
            InventorySlotClicked?.Invoke(equipment);
        }

        /// <summary>
        /// Set the inventory slot
        /// </summary>
        public void Set(Equipment equipment)
        {
            // Set the equipment
            this.equipment = equipment;

            // Show the empty group
            filledGroup.alpha = 1f;
            filledGroup.interactable = true;
            filledGroup.blocksRaycasts = true;

            // Hide the filled group
            emptyGroup.alpha = 0f;
            emptyGroup.interactable = false;
            emptyGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// Clear the inventory slot
        /// </summary>
        public void Clear()
        {
            // Nullify the equipment
            equipment = null;

            // Show the empty group
            emptyGroup.alpha = 1f;
            emptyGroup.interactable = true;
            emptyGroup.blocksRaycasts = true;

            // Hide the filled group
            filledGroup.alpha = 0f;
            filledGroup.interactable = false;
            filledGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// Highlight the inventory slot when the mouse enters
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            Highlight(1f, highlightDuration);
        }

        /// <summary>
        /// Remove the highlight when the mouse exits
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            Highlight(0f, highlightDuration);
        }

        /// <summary>
        /// Handle highlighting the border of the item slot
        /// </summary>
        private void Highlight(float endValue, float duration)
        {
            // Kill the highlight tween if it exists
            highlightTween?.Kill();

            // Create a new tween to highlight the border
            highlightTween = highlightBorder.DOFade(endValue, duration);

            // Set the easing type
            highlightTween.SetEase(Ease.InOutSine);
        }
    }
}
