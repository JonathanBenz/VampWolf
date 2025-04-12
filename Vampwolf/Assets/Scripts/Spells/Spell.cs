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
        public void Cast(SpellsModel model)
        {
            // Cast the spell
            data.Strategy.Cast(this, model);

            // Remove the cost from the model depending on the character type
            switch (CharacterType)
            {
                case CharacterType.Vampire:
                    model.Blood -= Cost;
                    break;
                case CharacterType.Werewolf:
                    model.Rage -= Cost;
                    break;
            }
        }
    }
}
