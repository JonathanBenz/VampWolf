using Vampwolf.EventBus;
using Vampwolf.Units;

namespace Vampwolf
{
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
}
