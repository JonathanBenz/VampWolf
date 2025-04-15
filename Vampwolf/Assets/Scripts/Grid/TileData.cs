using UnityEngine;

namespace Vampwolf.Grid
{
    public class TileData : MonoBehaviour
    {
        [SerializeField] private Vector3Int gridPosition;
        public Vector3Int GridPosition { get => gridPosition; set => gridPosition = value; }
    }
}
