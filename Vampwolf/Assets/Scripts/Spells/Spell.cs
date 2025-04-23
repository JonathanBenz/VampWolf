using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Grid;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    public class Spell
    {
        private readonly SpellsModel model;
        private readonly SpellData data;

        public CharacterType CharacterType => data.CharacterType;
        public SpellType SpellType => data.spellType;
        public string Name => data.Name;
        public string Description => data.Description;
        public float Cost => data.Cost;
        public int Range => data.Range;
        public Sprite Icon => data.Icon;
        public GridPredicate Predicate => data.Strategy.Predicate;

        public Spell(SpellsModel model, SpellData data)
        {
            // Set the model and data
            this.model = model;
            this.data = data;
        }

        /// <summary>
        /// Cast the spell
        /// </summary>
        public void Cast(BattleUnit caster, BattleUnit target)
        {
            // Cast the spell
            data.Strategy.Cast(this, caster, target);

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

            // Raise the event to update data
            EventBus<SpellCast>.Raise(new SpellCast()
            {
                CharacterType = data.CharacterType,
                SpellName = data.Name
            });
        }
    }
}
