using System.Collections.Generic;
using UnityEngine;
using Vampwolf.Grid;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Nightform", menuName = "Spells/Strategies/Nightform")]
    public class Nightform : SpellStrategy
    {
        public override GridPredicate Predicate => new GridPredicate((gridCell) => gridCell.HasPlayerUnit);

        /// <summary>
        /// Turn into a bat, becoming untargetable and losing all enemy aggro for one turn.
        /// </summary>
        public override void Cast(Spell spell, BattleUnit caster, BattleUnit target, List<BattleUnit> unitsInRange, List<BattleUnit> allUnits, Vector3Int gridPosition)
        {
            // Iterate through each battle unit
            foreach(BattleUnit unit in allUnits)
            {
                // Skip if not an enemy
                if (unit is not Enemy enemy) continue;

                // Ignore the vampire
                enemy.IgnoreTarget(caster, 1);
            }
        }
    }
}
