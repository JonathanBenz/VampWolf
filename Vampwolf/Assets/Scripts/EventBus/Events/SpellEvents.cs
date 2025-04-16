using Vampwolf.EventBus;
using Vampwolf.Spells;
using Vampwolf.Units;

namespace Vampwolf.Events
{
    public struct ShowSpells : IEvent
    {
        public CharacterType CharacterType;
        public BattleUnit CastingUnit;
    }

    public struct HideSpells : IEvent { }

    public struct DisableSpells : IEvent { }
}
