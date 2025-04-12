using UnityEngine;

namespace Vampwolf.Spells
{
    public class Spell
    {
        private readonly SpellData data;

        public CharacterType CharacterType => data.characterType;
        public string Name => data.Name;
        public string Description => data.Description;
        public float Cost => data.Cost;
        public int Range => data.Range;
        public Sprite Icon => data.Icon;

        public Spell(SpellData data)
        {
            this.data = data;
        }

        /// <summary>
        /// Cast the spell
        /// </summary>
        public void Cast() => data.Strategy.Cast(this);
    }
}
