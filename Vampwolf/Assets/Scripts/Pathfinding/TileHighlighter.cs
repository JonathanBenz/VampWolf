using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Vampwolf.Pathfinding
{
    public class TileHighlighter : MonoBehaviour
    {
        public Tilemap groundTilemap;
        public Tilemap obstacleTilemap;
        public Tilemap highlightTilemap;
        public Tilemap hoverTilemap;
        public Tile highlightTile;
        public Tile hoverTile;

        private Vector3Int lastHoveredTile = Vector3Int.zero;
        public HashSet<Vector3Int> HighlightedTiles { get; private set; } = new HashSet<Vector3Int>();
        private void Update()
        {
            HandleHoverIndicator();
        }
        void HandleHoverIndicator()
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            Vector3Int hoveredCell = groundTilemap.WorldToCell(mouseWorld);

            if (!groundTilemap.HasTile(hoveredCell) || obstacleTilemap.HasTile(hoveredCell) || !HighlightedTiles.Contains(hoveredCell))
            {
                hoverTilemap.ClearAllTiles();
                lastHoveredTile = new Vector3Int(int.MinValue, int.MinValue, 0);
                return;
            }

            // Only update the hover tile if the hovered tile is different from the last one
            if (hoveredCell != lastHoveredTile)
            {
                hoverTilemap.ClearAllTiles();
                lastHoveredTile = hoveredCell;

                if (groundTilemap.HasTile(hoveredCell) && !obstacleTilemap.HasTile(hoveredCell))
                {
                    hoverTilemap.SetTile(hoveredCell, hoverTile); // Show hover indicator
                }
            }
        }
        public void HighlightMoveableTiles(Vector3Int startCell, int range)
        {
            ClearHighlights();

            HighlightedTiles.Clear();

            HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
            Queue<Vector3Int> frontier = new Queue<Vector3Int>();

            frontier.Enqueue(startCell);
            visited.Add(startCell);

            for (int i = 0; i <= range; i++)
            {
                int frontierCount = frontier.Count;

                for (int j = 0; j < frontierCount; j++)
                {
                    Vector3Int current = frontier.Dequeue();
                    highlightTilemap.SetTile(current, highlightTile);
                    HighlightedTiles.Add(current);

                    foreach (Vector3Int dir in GetDirections())
                    {
                        Vector3Int neighbor = current + dir;

                        if (visited.Contains(neighbor)) continue;
                        if (!groundTilemap.HasTile(neighbor)) continue;
                        if (obstacleTilemap.HasTile(neighbor)) continue;

                        visited.Add(neighbor);
                        frontier.Enqueue(neighbor);
                    }
                }
            }
        }

        public void ClearHighlights()
        {
            highlightTilemap.ClearAllTiles();
        }

        List<Vector3Int> GetDirections()
        {
            return new List<Vector3Int>
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
        };
        }
    }
}
