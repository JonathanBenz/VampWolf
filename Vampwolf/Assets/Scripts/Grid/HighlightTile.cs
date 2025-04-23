using UnityEngine;

namespace Vampwolf.Grid
{
    public enum HighlightType
    {
        None,
        Move,
        AttackRange,
        AttackTarget,
        HealRange,
        HealTarget,
        BuffRange,
        BuffTarget
    }

    public class HighlightTile : MonoBehaviour
    {
        [SerializeField] private Vector3Int gridPosition;
        private Color moveColor = new Color(1.0f, 1.0f, 1.0f, 0.1f); // White
        private Color attackRangeColor = new Color(1.0f, 0f, 0f, 0.1f); // Red
        private Color attackTargetColor = new Color(1.0f, 0f, 0f, 0.4f);
        private Color healRangeColor = new Color(0f, 1.0f, 0f, 0.1f); // Green
        private Color healTargetColor = new Color(0f, 1.0f, 0f, 0.4f);
        private Color buffRangeColor = new Color(1.0f, 0.741f, 0.2f, 0.1f); // Orange
        private Color buffTargetColor = new Color(1.0f, 0.741f, 0.2f, 0.4f);
        private SpriteRenderer spriteRenderer;
        public Vector3Int GridPosition { get => gridPosition; set => gridPosition = value; }

        /// <summary>
        /// Initialize the highlight tile
        /// </summary>
        public HighlightTile Initialize()
        {
            // Get components
            spriteRenderer = GetComponent<SpriteRenderer>();

            return this;
        }

        /// <summary>
        /// Set the color of the highlight
        /// </summary>
        public void SetColor(HighlightType highlightType)
        {
            switch (highlightType)
            {
                case HighlightType.None:
                    break;

                case HighlightType.Move:
                    spriteRenderer.color = moveColor;
                    break;

                case HighlightType.AttackRange:
                    spriteRenderer.color = attackRangeColor;
                    break;

                case HighlightType.AttackTarget:
                    spriteRenderer.color = attackTargetColor;
                    break;

                case HighlightType.HealRange:
                    spriteRenderer.color = healRangeColor;
                    break;

                case HighlightType.HealTarget:
                    spriteRenderer.color = healTargetColor;
                    break;

                case HighlightType.BuffRange:
                    spriteRenderer.color = buffRangeColor;
                    break;

                case HighlightType.BuffTarget:
                    spriteRenderer.color = buffTargetColor;
                    break;
            }
        }
    }
}
