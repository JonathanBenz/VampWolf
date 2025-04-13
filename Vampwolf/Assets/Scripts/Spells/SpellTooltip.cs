using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace Vampwolf.Spells
{
    public class SpellTooltip : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasGroup tooltipGroup;
        [SerializeField] private Text spellNameText;
        [SerializeField] private Text spellDescriptionText;
        [SerializeField] private Text spellCostText;
        [SerializeField] private Text spellRangeText;
        private RectTransform canvasRectTransform;
        private RectTransform rectTransform;

        public string Name { set => spellNameText.text = value; }
        public string Description { set => spellDescriptionText.text = value; }
        public string Cost { set => spellCostText.text = value; }
        public string Range { set => spellRangeText.text = value; }
        public string SpellType { set => spellRangeText.text = value; }

        /// <summary>
        /// Initialize the Tooltip
        /// </summary>
        public void Initialize()
        {
            // Get the RectTransform component
            rectTransform = GetComponent<RectTransform>();
            canvasRectTransform = canvas.GetComponent<RectTransform>();

            // Hide the tooltip by default
            Hide();
        }

        /// <summary>
        /// Move the tooltip to a specified position
        /// </summary>
        public void MoveTooltip(Vector2 newPosition)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform,
                Mouse.current.position.ReadValue(),
                canvas.worldCamera,
                out Vector2 localPoint
            );

            // Calculate an offset
            Vector2 offset = new Vector2(1f, 0f);

            // Set the position of the tooltip
            rectTransform.anchoredPosition = localPoint;
        }

        /// <summary>
        /// Show the tooltip
        /// </summary>
        public void Show() => tooltipGroup.alpha = 1f;

        /// <summary>
        /// Hide the tooltip
        /// </summary>
        public void Hide() => tooltipGroup.alpha = 0f;
    }
}
