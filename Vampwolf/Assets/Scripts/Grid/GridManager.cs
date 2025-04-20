using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Units;

namespace Vampwolf.Grid
{
    public class GridManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Tilemap gridTilemap;

        public int Width { get; private set; }
        public int Height { get; private set; }

        private EventBinding<PlaceUnit> onPlaceUnit;

        private void Awake()
        {
            // Get the tilemap's cell bounds
            BoundsInt bounds = gridTilemap.cellBounds;

            // Set the width and height of the grid
            Width = bounds.size.x;
            Height = bounds.size.y;
        }

        private void OnEnable()
        {
            onPlaceUnit = new EventBinding<PlaceUnit>(PlaceUnit);
            EventBus<PlaceUnit>.Register(onPlaceUnit);
        }

        private void OnDisable()
        {
            EventBus<PlaceUnit>.Deregister(onPlaceUnit);
        }

        /// <summary>
        /// Convert grid coordinates to a world position
        /// </summary>
        public Vector3 GetWorldPositionFromGrid(Vector3Int gridPos)
        {
            return gridTilemap.GetCellCenterWorld(gridPos);
        }

        /// <summary>
        /// Convert a world position to grid coordinates
        /// </summary>
        public Vector3Int GetGridPositionFromWorld(Vector3 worldPos)
        {
            // Transform the world position to the tilemap's cell space
            Vector3Int cellPos = gridTilemap.WorldToCell(worldPos);

            // Exit case - the grid position does not exist
            if (!gridTilemap.HasTile(cellPos)) return Vector3Int.zero;

            return cellPos;
        }

        /// <summary>
        /// Check if a grid position exists on the tilemap
        /// </summary>
        public bool GridPositionExists(Vector3Int gridPos) => gridTilemap.HasTile(gridPos);

        /// <summary>
        /// Place a unit on the grid
        /// </summary>
        private void PlaceUnit(PlaceUnit eventData)
        {
            // Extract the data
            BattleUnit unit = eventData.Unit;
            Vector3Int gridPosition = eventData.GridPosition;

            // Set the position of the unit
            unit.transform.position = GetWorldPositionFromGrid(gridPosition);
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

            // Track which cells we've already visited to avoid infinite loops and redundant checking
            HashSet<Vector3Int> visited = new HashSet<Vector3Int>() { start };

            // Due to this being a BFS (breadth-first search), we need to use a queue
            Queue<(Vector3Int pos, int cost)> queue = new Queue<(Vector3Int pos, int cost)>();
            queue.Enqueue((start, 0));

            // Get the four cardinal directions
            Vector3Int[] dirs = {
                new Vector3Int( 1,  0, 0),
                new Vector3Int(-1,  0, 0),
                new Vector3Int( 0,  1, 0),
                new Vector3Int( 0, -1, 0)
            };

            // BFS loop - expand outward one "ring" at a time
            while(queue.Count > 0)
            {
                // Get the current position and cost
                (Vector3Int currentPos, int cost) = queue.Dequeue();

                // Don't want to include the start cell itself
                if (cost > 0)
                    reachableCells.Add(currentPos);

                // Skip if we've already moved the maximum distance
                if (cost == range)
                    continue;

                // Try all four cardinal neighbors
                foreach (Vector3Int d in dirs)
                {
                    // Get the position of the neighbor
                    Vector3Int next = currentPos + d;

                    // Skip if the neighbor has already been visited
                    if (visited.Contains(next))
                        continue;

                    // Skip if there is no tile
                    if (!gridTilemap.HasTile(next))
                        continue;

                    // Track the neighbor
                    visited.Add(next);

                    // Enquue the neighbor with the added cost for further expansion
                    queue.Enqueue((next, cost + 1));
                }
            }

            return reachableCells;
        }

        /// <summary>
        /// Finds a path from the start to the target using A* pathfinding
        /// </summary>
        public List<Vector3Int> FindPath(Vector3Int start, Vector3Int target)
        {
            // Exit case - the target grid position does not exist on the tilemap
            if (!GridPositionExists(target))
                return new List<Vector3Int>();

            // Create a container for the set of discovered nodes that need to be evaluated
            List<Vector3Int> openSet = new List<Vector3Int> { start };

            // Create a dictionary to store edges/relationships between found nodes
            Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();

            // The gScore descibres the currently known cheapest path
            Dictionary<Vector3Int, int> gScore = new Dictionary<Vector3Int, int>
            {
                [start] = 0
            };

            // fScore(n) = gScore(n) + heuristic(n, target) - using Manhattan distance for the heuristic
            Dictionary<Vector3Int, int> fScore = new Dictionary<Vector3Int, int>
            {
                [start] = ManhattanDistance(start, target)
            };

            // Loop while the open set is not empty
            while (openSet.Count > 0)
            {
                // Set the starting grid position and  fScore
                Vector3Int current = openSet[0];
                int currentFScore = fScore.ContainsKey(current) ? fScore[current] : int.MaxValue;

                // Iterate through each position in the open set
                foreach (Vector3Int pos in openSet)
                {
                    // Get the fScoore of the position
                    int score = fScore.ContainsKey(pos) ? fScore[pos] : int.MaxValue;

                    // Check if the current found score is less than the current score
                    if (score < currentFScore)
                    {
                        // Update the current position and score
                        current = pos;
                        currentFScore = score;
                    }
                }

                // If the target is reached, reconstruct the path
                if (current == target)
                    return ReconstructPath(cameFrom, current);

                // Remove the current node from the open set
                openSet.Remove(current);

                // Iterate through each neighbor of the current node
                foreach (Vector3Int neighbor in GetNeighbors(current))
                {
                    // Skip neighbors that do not exist on the tilemap
                    if (!GridPositionExists(neighbor))
                        continue;

                    // Get the gScore of the neighbor (assuming uniform costs)
                    int tentativeGScore = gScore[current] + 1;

                    // Check if the neighbor is not in the gScore dictionary or if the new gScore is less than the current gScore
                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        // Update the cameFrom dictionary with the current node
                        cameFrom[neighbor] = current;

                        // Update the gScore and fScore dictionaries
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = tentativeGScore + ManhattanDistance(neighbor, target);

                        // Check if the neighbor is not already in the open set
                        if (!openSet.Contains(neighbor))
                            // Add the neighbor to the open set
                            openSet.Add(neighbor);
                    }
                }
            }

            // No path found; return an empty list.
            return new List<Vector3Int>();
        }

        /// <summary>
        ///  Get the cardinal neighbors of a cell
        /// </summary>
        private IEnumerable<Vector3Int> GetNeighbors(Vector3Int cell)
        {
            yield return new Vector3Int(cell.x + 1, cell.y, cell.z);
            yield return new Vector3Int(cell.x - 1, cell.y, cell.z);
            yield return new Vector3Int(cell.x, cell.y + 1, cell.z);
            yield return new Vector3Int(cell.x, cell.y - 1, cell.z);
        }

        /// <summary>
        /// Computes the Manhattan distance between two grid positions.
        /// </summary>
        private int ManhattanDistance(Vector3Int a, Vector3Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        /// <summary>
        /// Reconstructs the path from the start node to the given current node.
        /// </summary>
        private List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
        {
            // Create a list to store the path, starting with the current node
            List<Vector3Int> path = new List<Vector3Int> { current };

            // Loop as long as the cameFrom dictionary has a previous node
            while (cameFrom.ContainsKey(current))
            {
                // Set the current node to the previous node
                current = cameFrom[current];

                // Add the path to the list
                path.Add(current);
            }

            // Reverse the path to get the correct order
            path.Reverse();

            return path;
        }
    }
}
