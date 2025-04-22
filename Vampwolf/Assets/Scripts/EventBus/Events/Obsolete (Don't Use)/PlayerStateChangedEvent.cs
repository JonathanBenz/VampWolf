using Vampwolf.EventBus;

namespace Vampwolf.Events
{
    public struct PlayerStateChangedEvent : IEvent
    {
        public bool AttackState;
    }
}
