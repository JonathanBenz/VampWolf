using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampwolf.Pathfinding
{
    /// <summary>
    /// Node used for A* pathfinding
    /// </summary>
    public class Node
    {
        public Vector3Int position;
        public bool walkable;
        public int gCost;
        public int hCost;
        public Node parent;

        public int FCost => gCost + hCost;

        public Node(Vector3Int pos, bool isWalkable)
        {
            position = pos;
            walkable = isWalkable;
        }
    }
}
