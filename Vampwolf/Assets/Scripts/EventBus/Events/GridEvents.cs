using UnityEngine;
using Vampwolf.EventBus;

namespace Vampwolf.Events
{
    public struct HighlightCells : IEvent
    {
        public Vector3Int GridPosition;
        public int Range;
    }
}
