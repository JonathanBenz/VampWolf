using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Grid;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Hemostatis", menuName = "Spells/Strategies/Hemostasis")]
    public class Hemostasis : SpellStrategy
    {
        public override GridPredicate Predicate => new GridPredicate((gridCell) => gridCell.HasPlayerUnit);

        /// <summary>
        /// Heal a small amount yourself or an ally
        /// </summary>
        public override void Cast(Spell spell, BattleUnit caster, BattleUnit target)
        {
            // Heal the unit
            target.DealDamage(-15);

            // Collect data
            EventBus<DamageHealed>.Raise(new DamageHealed()
            {
                CharacterType = CharacterType.Vampire,
                Amount = 15
            });
        }
    }
}
