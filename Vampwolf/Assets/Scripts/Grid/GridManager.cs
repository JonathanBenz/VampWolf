using System.Collections.Generic;
using UnityEngine;
using Vampwolf.Grid;

namespace Vampwolf
{
    public class GridManager : MonoBehaviour
    {
        public int width = 10;
        public int height = 10;
        public float cellSize = 1f;

        public GridModel Grid { get; private set; }

        private void Awake()
        {
            // Initialize the grid model
            Grid = new GridModel(width, height);
        }

        /// <summary>
        /// Convert grid coordinates to a world position
        /// </summary>
        public Vector3 GetWorldPositionFromGrid(Vector2Int gridPos)
        {
            return new Vector3(gridPos.x * cellSize, 0, gridPos.y * cellSize);
        }

        // Check if a target cell is reachable given a move range.
        public bool IsCellReachable(Vector2Int start, Vector2Int target, int range)
        {
            return (Mathf.Abs(target.x - start.x) + Mathf.Abs(target.y - start.y)) <= range;
        }

        // Get reachable cells fro ma starting position within a given move range
        public List<Vector2Int> GetReachableCells(Vector2Int start, int range)
        {
            // Create a container to store the reachable cells
            List<Vector2Int> reachableCells = new List<Vector2Int>();

            // Iterate through the grid
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Create a position at the current cell
                    Vector2Int targetPos = new Vector2Int(x, y);

                    // Skip if the cell is not walkable
                    if (!Grid.Cells[x, y].IsWalkable) continue;

                    // Skip if the cell is outside the move range
                    if (!IsCellReachable(start, targetPos, range)) continue;

                    // Add the cell to the reachable cells list
                    reachableCells.Add(targetPos);
                }
            }

            return reachableCells;
        }
    }
}
