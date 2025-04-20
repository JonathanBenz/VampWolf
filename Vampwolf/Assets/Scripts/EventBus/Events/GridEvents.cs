using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Grid;
using Vampwolf.Spells;
using Vampwolf.Units;

namespace Vampwolf.Events
{
    public struct PlaceUnit : IEvent
    {
        public BattleUnit Unit;
        public Vector3Int GridPosition;
    }

    public struct ClearHighlights : IEvent { }

    public struct SetGridSelector : IEvent
    {
        public bool Active;
        public bool isEnemyTurn;
    }

    public struct SetMovementSelectionMode : IEvent 
    {
        public Vector3Int GridPosition;
        public int Range;
        public int TileColor;
    }

    public struct SetSpellSelectionMode : IEvent
    {
        public Spell Spell;
        public Vector3Int GridPosition;
        public int TileColor;
    }

    public struct MoveCellSelected : IEvent
    {
        public GridManager GridManager;
        public Vector3Int GridPosition;
    }

    public struct TargetCellSelected : IEvent
    {
        public Spell Spell;
        public Vector3Int GridPosition;
    }
}
