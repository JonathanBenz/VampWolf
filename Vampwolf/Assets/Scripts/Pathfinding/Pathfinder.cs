using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Vampwolf.Pathfinding
{
    public class Pathfinder : MonoBehaviour
    {
        public Tilemap groundTilemap;
        public Tilemap obstacleTilemap;

        public List<Vector3Int> FindPath(Vector3Int start, Vector3Int target)
        {
            Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

            List<Node> openList = new List<Node>();
            HashSet<Node> closedList = new HashSet<Node>();

            Node startNode = GetNode(start, allNodes);
            Node targetNode = GetNode(target, allNodes);

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node current = openList[0];
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].FCost < current.FCost ||
                        (openList[i].FCost == current.FCost && openList[i].hCost < current.hCost))
                    {
                        current = openList[i];
                    }
                }

                openList.Remove(current);
                closedList.Add(current);

                if (current.position == target)
                    return RetracePath(startNode, current);

                foreach (Vector3Int direction in GetNeighbors())
                {
                    Vector3Int neighborPos = current.position + direction;
                    Node neighbor = GetNode(neighborPos, allNodes);

                    if (!neighbor.walkable || closedList.Contains(neighbor))
                        continue;

                    int tentativeGCost = current.gCost + 1;

                    if (tentativeGCost < neighbor.gCost || !openList.Contains(neighbor))
                    {
                        neighbor.gCost = tentativeGCost;
                        neighbor.hCost = GetManhattanDistance(neighbor.position, target);
                        neighbor.parent = current;

                        if (!openList.Contains(neighbor))
                            openList.Add(neighbor);
                    }
                }
            }

            return null; // No path found
        }

        List<Vector3Int> RetracePath(Node startNode, Node endNode)
        {
            List<Vector3Int> path = new List<Vector3Int>();
            Node current = endNode;

            while (current != startNode)
            {
                path.Add(current.position);
                current = current.parent;
            }

            path.Reverse();
            return path;
        }

        Node GetNode(Vector3Int pos, Dictionary<Vector3Int, Node> cache)
        {
            if (cache.ContainsKey(pos)) return cache[pos];

            bool walkable = groundTilemap.HasTile(pos) && !obstacleTilemap.HasTile(pos);
            Node node = new Node(pos, walkable) { gCost = int.MaxValue };
            cache[pos] = node;
            return node;
        }

        int GetManhattanDistance(Vector3Int a, Vector3Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        List<Vector3Int> GetNeighbors()
        {
            return new List<Vector3Int> {
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, -1, 0)
        };
        }
    }
}
