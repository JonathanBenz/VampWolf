using System.Collections.Generic;
using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Grid;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Savage Leap", menuName = "Spells/Strategies/Savage Leap")]
    public class SavageLeap : SpellStrategy
    {
        public override GridPredicate Predicate => new GridPredicate((gridCell) => gridCell.HasEnemyUnit);

        /// <summary>
        /// Lunge forward and deal damage to an enemy.
        /// </summary>
        public override void Cast(Spell spell, BattleUnit caster, BattleUnit target, List<BattleUnit> unitsInRange, List<BattleUnit> allUnits, Vector3Int gridPosition)
        {
            // Leap to the target's grid position
            caster.Blink(target.GridPosition);

            // Deal damage to the target
            target.DealDamage(40);

            // Track damage
            EventBus<DamageDealt>.Raise(new DamageDealt()
            {
                CharacterType = CharacterType.Werewolf,
                Amount = 40
            });
        }
    }
}
