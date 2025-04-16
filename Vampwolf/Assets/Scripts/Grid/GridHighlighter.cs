using System.Collections.Generic;
using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf.Grid
{
    public class GridHighlighter : MonoBehaviour
    {
        [Header("Pool")]
        [SerializeField] private TileData cellHighlightPrefab;
        [SerializeField] private Transform highlightsParent;
        private GridManager gridManager;
        private GridHighlightPool highlightPool;
        [SerializeField] private List<TileData> highlightedCells;

        private EventBinding<ClearHighlights> onClearHighlights;

        public List<TileData> HighlightedCells => highlightedCells;

        private void Awake()
        {
            // Get components
            gridManager = GetComponent<GridManager>();

            // Initialize the highlight list
            highlightedCells = new List<TileData>();
        }

        private void OnEnable()
        {
            onClearHighlights = new EventBinding<ClearHighlights>(ClearHighlights);
            EventBus<ClearHighlights>.Register(onClearHighlights);
        }

        private void OnDisable()
        {
            EventBus<ClearHighlights>.Deregister(onClearHighlights);
        }

        private void Start()
        {
            // Create the highlight pool
            highlightPool = new GridHighlightPool(
                cellHighlightPrefab, 
                highlightsParent, 
                gridManager.Width, 
                gridManager.Height
            );
        }

        /// <summary>
        /// Clear the active grid highlights
        /// </summary>
        public void ClearHighlights()
        {
            // Iterate through each grid highlight
            foreach(TileData highlight in highlightedCells)
            {
                // Release the highlight back to the pool
                highlightPool.Release(highlight);
            }

            // Clear the list of highlights
            highlightedCells.Clear();
        }

        /// <summary>
        /// Highlight a list of cells
        /// </summary>
        public void HighlightCells(List<Vector3Int> cellPositions)
        {
            // Clear the previous highlights
            ClearHighlights();

            // Itearte through each cell position
            foreach(Vector3Int cellPosition in cellPositions)
            {
                // Get the world position of the cell from the grid position
                Vector3 worldPos = gridManager.GetWorldPositionFromGrid(cellPosition);

                // Get a highlight from the pool
                TileData highlight = highlightPool.Get();

                // Set the grid position of the cell
                highlight.GridPosition = cellPosition;

                // Set the world position of the cell
                highlight.transform.position = worldPos;

                // Add the highlight to the list of highlights
                highlightedCells.Add(highlight);
            }
        }

        /// <summary>
        /// Check if a cell is highlighted
        /// </summary>
        public bool IsCellHighlighted(Vector3Int cellPosition)
        {
            // Iterate through each grid highlight
            foreach (TileData highlight in highlightedCells)
            {
                // Check if the grid positions are the same
                if (highlight.GridPosition == cellPosition)
                {
                    // Return true
                    return true;
                }
            }

            // Return false if no grid positions of the same cell were found
            return false;
        }

        /// <summary>
        /// Callback function to highlight cells within the range of a given grid position
        /// </summary>
        public void HighlightCellsInRange(Vector3Int gridPosition, int range)
        {
            // Get the cell positions within range of the given grid position
            List<Vector3Int> cellPositions = gridManager.GetReachableCells(gridPosition, range);

            // Highlight the cells
            HighlightCells(cellPositions);
        }
    }
}
