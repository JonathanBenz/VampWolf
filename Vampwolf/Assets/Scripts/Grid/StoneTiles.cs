using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Vampwolf
{
    public class StoneTiles : MonoBehaviour
    {
        private Tilemap stonePathsTileMap;
        private Dictionary<Vector3Int, bool> tilePositions = new Dictionary<Vector3Int, bool>();

        private void Awake()
        {
            stonePathsTileMap = GetComponent<Tilemap>();
        }

        // Put stone tile positions into dictionary
        void Start()
        {
            BoundsInt bounds = stonePathsTileMap.cellBounds;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);
                    if (stonePathsTileMap.HasTile(position))
                    {
                        tilePositions[position] = true;
                    }
                }
            }
        }

        /// <summary>
        /// Perform quick dictionary lookup to see if the given position is a stone tile position
        /// </summary>
        public bool IsStandingOnStoneTile(Vector3Int pos)
        {
            if (tilePositions.ContainsKey(pos)) return true;
            return false;
        }
    }
}
