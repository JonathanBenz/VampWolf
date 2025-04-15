using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Vampwolf.Grid
{
    public class GridManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Tilemap gridTilemap;

        public int Width { get; private set; }
        public int Height { get; private set; }

        private void Awake()
        {
            // Get the tilemap's cell bounds
            BoundsInt bounds = gridTilemap.cellBounds;

            // Set the width and height of the grid
            Width = bounds.size.x;
            Height = bounds.size.y;
        }

        /// <summary>
        /// Convert grid coordinates to a world position
        /// </summary>
        public Vector3 GetWorldPositionFromGrid(Vector3Int gridPos)
        {
            return gridTilemap.GetCellCenterWorld(gridPos);
        }

        /// <summary>
        /// Check if a target cell is reachable given a move range
        /// </summary>
        public bool IsCellReachable(Vector3Int start, Vector3Int target, int range)
        {
            int distance = Mathf.Abs(target.x - start.x) + Mathf.Abs(target.y - start.y);
            return distance <= range;
        }

        /// <summary>
        /// Get reachable cells from a starting position within a given move range
        /// </summary>
        public List<Vector3Int> GetReachableCells(Vector3Int start, int range)
        {
            // Create a container to store the reachable cells
            List<Vector3Int> reachableCells = new List<Vector3Int>();

            // Get the tilemap's cell bounds
            BoundsInt bounds = gridTilemap.cellBounds;

            // Iterate through the grid
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    // Create a position at the current cell
                    Vector3Int targetPos = new Vector3Int(x, y, 0);

                    // Skip if the cell is not walkable
                    if (!gridTilemap.HasTile(targetPos)) continue;

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
