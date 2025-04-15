using System.Collections.Generic;
using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf.Grid
{
    public class GridHighlighter : MonoBehaviour
    {
        [Header("Pool")]
        [SerializeField] private GameObject cellHighlightPrefab;
        [SerializeField] private Transform highlightsParent;
        private GridManager gridManager;
        private GridHighlightPool highlightPool;
        private List<GameObject> highlightList;

        private EventBinding<HighlightCells> onHighlightCells;

        private void Awake()
        {
            // Get components
            gridManager = GetComponent<GridManager>();

            // Initialize the highlight list
            highlightList = new List<GameObject>();
        }

        private void OnEnable()
        {
            onHighlightCells = new EventBinding<HighlightCells>(HighlightCellsInRange);
            EventBus<HighlightCells>.Register(onHighlightCells);
        }

        private void OnDisable()
        {
            EventBus<HighlightCells>.Deregister(onHighlightCells);
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
            foreach(GameObject highlight in highlightList)
            {
                // Release the highlight back to the pool
                highlightPool.Release(highlight);
            }

            // Clear the list of highlights
            highlightList.Clear();
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
                GameObject highlight = highlightPool.Get();

                // Set its transform to the world position of the cell
                highlight.transform.position = worldPos;
            }
        }

        /// <summary>
        /// Callback function to highlight cells within the range of a given grid position
        /// </summary>
        public void HighlightCellsInRange(HighlightCells eventData)
        {
            // Get the cell positions within range of the given grid position
            List<Vector3Int> cellPositions = gridManager.GetReachableCells(eventData.GridPosition, eventData.Range);

            // Highlight the cells
            HighlightCells(cellPositions);
        }
    }
}
