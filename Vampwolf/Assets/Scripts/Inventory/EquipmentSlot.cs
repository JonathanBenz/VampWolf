using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vampwolf.Shop;

namespace Vampwolf.Inventory
{
    public class EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("References")]
        [SerializeField] private Image highlightBorder;
        [SerializeField] private Image equipmentIcon;
        [SerializeField] private CanvasGroup emptyGroup;
        [SerializeField] private CanvasGroup filledGroup;
        private Button button;
        private Equipment equipment;

        [Header("Fields")]
        [SerializeField] private UserType slotType;
        [SerializeField] private bool containsEquipment;

        [Header("Tweening Variables")]
        [SerializeField] private float highlightDuration;
        private Tween highlightTween;

        public event Action<EquipmentSlot> EmptySlotClicked = delegate { };

        public Equipment Equipment => equipment;
        public UserType SlotType => slotType;

        private void OnDestroy()
        {
            // Kill any existing tweens
            highlightTween?.Kill();
        }

        /// <summary>
        /// Initialize the item slot
        /// </summary>
        public void Initialize()
        {
            // Get components
            button = GetComponent<Button>();

            // Set the button on-click listener
            button.onClick.AddListener(OnClick);
        }

        /// <summary>
        /// Process logic for when the equipment slot is clicked
        /// </summary>
        private void OnClick()
        {
            // Check if there's no equipment
            if(equipment == null)
            {
                // Notify that this equipment slot has been clicked
                EmptySlotClicked?.Invoke(this);

                return;
            }

            // Clear the equipment slot
            Clear();
        }

        /// <summary>
        /// Set a piece of equipment for the slot
        /// </summary>
        public void Set(Equipment equipment)
        {
            // Set the equipment
            this.equipment = equipment;

            // Set that the equipment is set
            containsEquipment = true;

            // Set the equipment icon
            equipmentIcon.sprite = equipment.Icon;

            // Set the empty group to invisible
            emptyGroup.alpha = 0f;
            emptyGroup.interactable = false;
            emptyGroup.blocksRaycasts = false;

            // Set the filled group to true
            filledGroup.alpha = 1f;
            filledGroup.interactable = true;
            filledGroup.blocksRaycasts = true;

            // Equip the equipment
            equipment.Equip();

            Debug.Log("Set Equipment");
        }

        /// <summary>
        /// Clear the equipment slot
        /// </summary>
        public void Clear()
        {
            // Unequip the equipment
            equipment.Unequip();

            // Nullify the equipment
            equipment = null;

            // Set that the equipment is not set
            containsEquipment = false;

            // Set the empty group to invisible
            filledGroup.alpha = 0f;
            filledGroup.interactable = false;
            filledGroup.blocksRaycasts = false;

            // Set the filled group to true
            emptyGroup.alpha = 1f;
            emptyGroup.interactable = true;
            emptyGroup.blocksRaycasts = true;

            Debug.Log("Cleared Equipment");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Highlight(1f, highlightDuration);
        }

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
