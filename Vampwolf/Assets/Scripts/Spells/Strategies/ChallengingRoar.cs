using System.Collections.Generic;
using UnityEngine;
using Vampwolf.Grid;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Challenging Roar", menuName = "Spells/Strategies/Challenging Roar")]
    public class ChallengingRoar : SpellStrategy
    {
        public override GridPredicate Predicate => new GridPredicate((gridCell) => gridCell.HasEnemyUnit);

        /// <summary>
        /// Draw all enemy attention, forcing them to focus attacks on you for two rounds.
        /// </summary>
        public override void Cast(Spell spell, BattleUnit caster, BattleUnit target, List<BattleUnit> unitsInRange, List<BattleUnit> allUnits, Vector3Int gridPosition)
        {
            // Iterate through each unit in range
            foreach(BattleUnit unit in unitsInRange)
            {
                // Skip if the unit is not an enemy
                if (unit is not Enemy enemy) return;

                // Taunt the target for two rounds
                enemy.AggroTarget(caster, 2);
            }
        }
    }
}
