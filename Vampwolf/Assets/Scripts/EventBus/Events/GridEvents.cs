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

    public struct PlaceHellPortal : IEvent
    {
        public HellPortal HellPortal;
        public Vector3Int GridPosition;
    }

    public struct PortalOpened : IEvent
    {
    }

    public struct MoveUnit : IEvent
    {
        public BattleUnit Unit;
        public Vector3Int LastPosition;
        public Vector3Int NewPosition;
    }

    public struct RemoveGridCellUnit : IEvent
    {
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
        public HighlightType HighlightType;
    }

    public struct SetSpellSelectionMode : IEvent
    {
        public Spell Spell;
        public Vector3Int GridPosition;
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
