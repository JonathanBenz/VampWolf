using UnityEngine;

namespace Vampwolf.Grid
{
    public class TileData : MonoBehaviour
    {
        [SerializeField] private Vector3Int gridPosition;
        private Color moveColor = new Color(1f, 1f, 1f, 0.1f); // White
        private Color targetColor = new Color(1f, 0f, 0f, 0.1f); // Red
        private SpriteRenderer spriteRenderer;
        public Vector3Int GridPosition { get => gridPosition; set => gridPosition = value; }

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetTileColor(int color)
        {
            if (color == 0) spriteRenderer.color = moveColor; // Movement
            if (color == 1) spriteRenderer.color = targetColor; // Attack
        }

    }
}
