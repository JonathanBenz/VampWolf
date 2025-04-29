using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Grid;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Maul", menuName = "Spells/Strategies/Maul")]
    public class Maul : SpellStrategy
    {
        public override GridPredicate Predicate => new GridPredicate((gridCell) => gridCell.HasEnemyUnit);

        /// <summary>
        /// Deal damage to a single target enemy
        /// </summary>
        public override void Cast(Spell spell, BattleUnit caster, BattleUnit target)
        {
            // Deal damage to the unit
            target.DealDamage(30);

            // Collect data
            EventBus<DamageDealt>.Raise(new DamageDealt()
            {
                CharacterType = CharacterType.Werewolf,
                Amount = 30
            });
        }
    }
}
