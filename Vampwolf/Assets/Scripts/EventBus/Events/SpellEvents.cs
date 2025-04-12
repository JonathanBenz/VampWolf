using Vampwolf.EventBus;
using Vampwolf.Spells;

namespace Vampwolf.Events
{
    public struct ShowSpells : IEvent
    {
        public CharacterType CharacterType;
    }
}
