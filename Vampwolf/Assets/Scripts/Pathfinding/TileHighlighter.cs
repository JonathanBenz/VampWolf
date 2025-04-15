using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vampwolf.Events;
using Vampwolf.EventBus;

namespace Vampwolf.Pathfinding
{
    /// <summary>
    /// This component is used to highlight the player's Movement range and Attack ranges.
    /// </summary>
    public class TileHighlighter : MonoBehaviour
    {
        [Header("Tile Maps")]
        [SerializeField] Tilemap groundTilemap;
        [SerializeField] Tilemap obstacleTilemap;
        [SerializeField] Tilemap highlightTilemap;
        [SerializeField] Tilemap hoverTilemap;

        [Header("Specific Tiles")]
        [SerializeField] Tile highlightMovementTile;
        [SerializeField] Tile hoverMovementTile;
        [SerializeField] Tile highlightAttackTile;
        [SerializeField] Tile hoverAttackTile;

        Tile currentHighlightTile;
        Tile currentHoverTile;
        Vector3Int lastHoveredTile = Vector3Int.zero;

        // Track when player switches between Move and Attack
        EventBinding<PlayerStateChangedEvent> onPlayerSwitchedState;
        bool stateHasSwitched;

        public HashSet<Vector3Int> HighlightedTiles { get; private set; } = new HashSet<Vector3Int>();
        public Tilemap GroundTileMap { get { return groundTilemap; } }
        public Tilemap ObstacleTileMap { get { return obstacleTilemap; } }

        private void Awake()
        {
            currentHighlightTile = highlightMovementTile;
            currentHoverTile = hoverMovementTile;
        }

        private void OnEnable()
        {
            onPlayerSwitchedState = new EventBinding<PlayerStateChangedEvent>(ChangeCurrentIcons);
            EventBus<PlayerStateChangedEvent>.Register(onPlayerSwitchedState);
        }

        private void OnDisable()
        {
            EventBus<PlayerStateChangedEvent>.Deregister(onPlayerSwitchedState);
        }

        /// <summary>
        /// If a PlayerStateChangedEvent is called, change the icons to represent the new state (either to Attack icons or Movement icons)
        /// </summary>
        /// <param name="eventData"></param>
        private void ChangeCurrentIcons(PlayerStateChangedEvent eventData)
        {
            if(eventData.AttackState)
            {
                currentHighlightTile = highlightAttackTile;
                currentHoverTile = hoverAttackTile;
            }
            else
            {
                currentHighlightTile = highlightMovementTile;
                currentHoverTile = hoverMovementTile;
            }
            stateHasSwitched = true;
        }

        /// <summary>
        /// Handle displaying hover indicator given the mouse position
        /// </summary>
        /// <param name="mousePos"></param>
        public void HandleHoverIndicator(Vector3 mousePos)
        {
            Vector3Int hoveredCell = groundTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(mousePos));
            CheckIfValid(hoveredCell);
        }

        /// <summary>
        /// Check to see if the tile is valid to be hovered over
        /// </summary>
        /// <param name="hoveredCell">tilemap.WorldToCell() position of the hovered cell.</param>
        private void CheckIfValid(Vector3Int hoveredCell)
        {
                 // Checks if out of bounds            // Checks if hovering over an obstacle    // Checks if outside Highlight range
            if (!groundTilemap.HasTile(hoveredCell) || obstacleTilemap.HasTile(hoveredCell) || !HighlightedTiles.Contains(hoveredCell))
            {
                hoverTilemap.ClearAllTiles();
                lastHoveredTile = Vector3Int.zero;
                return;
            }

            // Only update the hover tile if the hovered tile is different from the last one
            if (hoveredCell != lastHoveredTile || stateHasSwitched)
            {
                hoverTilemap.ClearAllTiles();
                lastHoveredTile = hoveredCell;
                hoverTilemap.SetTile(hoveredCell, currentHoverTile);
                stateHasSwitched = false;
            }
        }

        /// <summary>
        /// Handle displaying highlightable tiles based on range
        /// </summary>
        /// <param name="startCell">The player's cell position</param>
        /// <param name="range"></param>
        public void HighlightMoveableTiles(Vector3Int startCell, int range)
        {
            ClearHighlights();

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
                    highlightTilemap.SetTile(current, currentHighlightTile);
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
            HighlightedTiles.Clear();
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
