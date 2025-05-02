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
        private RectTransform rectTransform;
        private Button button;
        private Equipment equipment;

        [Header("Fields")]
        [SerializeField] private UserType slotType;

        [Header("Tweening Variables")]
        [SerializeField] private float highlightDuration;
        private Tween highlightTween;

        public event Action<EquipmentSlot, RectTransform> EmptySlotClicked = delegate { };
        public event Action<EquipmentSlot> FilledSlotClicked = delegate { };

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
            rectTransform = GetComponent<RectTransform>();

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
                // Notify that this equipment slot - with no data - has been clicked
                EmptySlotClicked?.Invoke(this, rectTransform);

                return;
            }

            // Notify that this equipment slot - with data - has been clicked
            FilledSlotClicked?.Invoke(this);
        }

        /// <summary>
        /// Set a piece of equipment for the slot
        /// </summary>
        public void Set(Equipment equipment)
        {
            // Set the equipment
            this.equipment = equipment;

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

            // Set the empty group to invisible
            filledGroup.alpha = 0f;
            filledGroup.interactable = false;
            filledGroup.blocksRaycasts = false;

            // Set the filled group to true
            emptyGroup.alpha = 1f;
            emptyGroup.interactable = true;
            emptyGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// Handle the selecting of the item slot when the mouse enters
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Highlight the border of the item slot
            Highlight(1f, highlightDuration);
        }

        /// <summary>
        /// Handle the deselecting of the item slot when the mouse exits
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            // Remove the highlight from the border of the item slot
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
