using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Grid;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Blood Lance", menuName = "Spells/Strategies/Blood Lance")]
    public class BloodLance : SpellStrategy
    {
        public override GridPredicate Predicate => new GridPredicate((gridCell) => gridCell.HasEnemyUnit);

        /// <summary>
        /// Hurl a spear of blood to damage a single enemy
        /// </summary>
        public override void Cast(Spell spell, BattleUnit caster, BattleUnit target)
        {
            // Deal damage to the unit
            target.DealDamage(25);

            // Collect data
            EventBus<DamageDealt>.Raise(new DamageDealt()
            {
                CharacterType = CharacterType.Vampire,
                Amount = 25
            });
        }
    }
}
