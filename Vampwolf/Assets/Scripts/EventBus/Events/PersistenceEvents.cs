using Vampwolf.EventBus;
using Vampwolf.Spells;

namespace Vampwolf
{
    public struct DamageDealt : IEvent
    {
        public CharacterType CharacterType;
        public int Amount;
    }

    public struct DamageHealed : IEvent
    {
        public CharacterType CharacterType;
        public int Amount;
    }

    public struct DamageTaken : IEvent
    {
        public CharacterType CharacterType;
        public int Amount;
    }

    public struct SpellCast : IEvent
    {
        public CharacterType CharacterType;
        public string SpellName;
        public int TimesCast;
    }
}
