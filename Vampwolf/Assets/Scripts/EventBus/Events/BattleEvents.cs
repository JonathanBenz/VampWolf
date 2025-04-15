using Vampwolf.EventBus;
using Vampwolf.Units;

namespace Vampwolf
{
    public struct InitiativeRegistered : IEvent
    {
        public BattleUnit Unit;
    }

    public struct InitiativeDeregistered : IEvent
    {
        public BattleUnit Unit;
    }

    public struct TurnStarted : IEvent
    {
        public BattleUnit Unit;
    }

    public struct HealthChanged : IEvent
    {
        public BattleUnit Unit;
        public int CurrentHealth;
    }

    public struct SetEndTurnButton : IEvent
    {
        public bool Active;
    }

    public struct SkipTurn : IEvent { }

    public struct RemoveUnit : IEvent
    {
        public BattleUnit Unit;
        public bool IsEnemy;
    }

    public struct BattleWon : IEvent { }
}
