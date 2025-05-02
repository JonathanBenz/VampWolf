using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Spells;

namespace Vampwolf.Grid
{
    public class GridHighlighter : MonoBehaviour
    {
        [Header("Pool")]
        [SerializeField] private HighlightTile cellHighlightPrefab;
        [SerializeField] private Transform highlightsParent;
        private GridManager gridManager;
        private GridHighlightPool highlightPool;
        [SerializeField] private List<HighlightTile> highlightedCells;

        private EventBinding<ClearHighlights> onClearHighlights;

        public List<HighlightTile> HighlightedCells => highlightedCells;

        private void Awake()
        {
            // Get components
            gridManager = GetComponent<GridManager>();

            // Initialize the highlight list
            highlightedCells = new List<HighlightTile>();
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
            foreach(HighlightTile highlight in highlightedCells)
            {
                // Release the highlight back to the pool
                highlightPool.Release(highlight);
            }

            // Clear the list of highlights
            highlightedCells.Clear();
        }

        public void AddHighlight(Vector3Int cellPosition, HighlightType highlightType)
        {
            // Get the world position of the cell from the grid position
            Vector3 worldPos = gridManager.GetWorldPositionFromGrid(cellPosition);

            // Get a highlight from the pool
            HighlightTile highlight = highlightPool.Get();

            // Set the grid position of the cell
            highlight.GridPosition = cellPosition;

            // Set the world position of the cell
            highlight.transform.position = worldPos;

            // Set the color of the highlight
            highlight.SetColor(highlightType);

            // Add the highlight to the list of highlights
            highlightedCells.Add(highlight);
        }

        /// <summary>
        /// Highlight a list of cells
        /// </summary>
        public void HighlightCells(List<Vector3Int> cellPositions, HighlightType highlightType, bool overlay = false)
        {
            // Check if overlaying
            if(!overlay)
            {
                // If not overlaying, clear the highlights
                ClearHighlights();

                // Iterate through each cell position
                foreach (Vector3Int cellPosition in cellPositions)
                {
                    // Add a highlight to the cell
                    AddHighlight(cellPosition, highlightType);
                }

                return;
            }

            // Iterate through each cell position
            foreach (Vector3Int cellPosition in cellPositions)
            {
                // Set a false-default for if a highlight exists
                bool highlightExistsAtPosition = false;

                // Iterate through each existing highlight
                foreach(HighlightTile highlight in highlightedCells)
                {
                    // Skip if the grid positions are not equal
                    if (highlight.GridPosition != cellPosition) continue;

                    // Set the color of the cell
                    highlight.SetColor(highlightType);

                    // Set the highlight exists to true
                    highlightExistsAtPosition = true;
                }

                // Skip if a highlight already exists at the position
                if (highlightExistsAtPosition) continue;

                // Add a highlight to the cell
                AddHighlight(cellPosition, highlightType);
            }
        }

        /// <summary>
        /// Check if a cell is highlighted
        /// </summary>
        public bool IsCellHighlighted(Vector3Int cellPosition)
        {
            // Iterate through each grid highlight
            foreach (HighlightTile highlight in highlightedCells)
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
        /// Callback function to highlight cells within the movement range of a given grid position
        /// </summary>
        public void HighlightCellsInMovementRange(Vector3Int gridPosition, int range, HighlightType highlightType, GridPredicate predicate = null)
        {
            // Get the cell positions within range of the given grid position
            List<Vector3Int> cellPositions = gridManager.GetReachableCells(gridPosition, range);

            // Highlight the cells
            HighlightCells(cellPositions, highlightType);
        }

        /// <summary>
        /// Callback function to highlight cells within the spell range of a given grid position
        /// </summary>
        public void HighlightCellsInSpellRange(Vector3Int gridPosition, int range, SpellType type, GridPredicate predicate)
        {
            // Get the cell positions within range of the given grid position
            List<Vector3Int> cellPositions = gridManager.GetReachableCells(gridPosition, range, true);

            // Get highlight types
            HighlightType rangeHighlight;
            HighlightType targetHighlight;

            switch(type)
            {
                case SpellType.Attack:
                    rangeHighlight = HighlightType.AttackRange;
                    targetHighlight = HighlightType.AttackTarget;
                    break;

                case SpellType.Heal:
                    rangeHighlight = HighlightType.HealRange;
                    targetHighlight = HighlightType.HealTarget;
                    break;

                case SpellType.Buff:
                    rangeHighlight = HighlightType.BuffRange;
                    targetHighlight = HighlightType.BuffTarget;
                    break;

                case SpellType.Movement:
                    rangeHighlight = HighlightType.MoveRange;
                    targetHighlight = HighlightType.MoveRange;
                    break;

                default:
                    rangeHighlight = HighlightType.None;
                    targetHighlight = HighlightType.None;
                    break;
            }

            // Highlight the cells
            HighlightCells(cellPositions, rangeHighlight);

            // Get a list of the valid cells according to the predicate
            List<Vector3Int> validCells = gridManager.GetValidCells(cellPositions, predicate);

            // Overlay the highlight with a more active highlight
            HighlightCells(validCells, targetHighlight, true);
        }
    }
}
