using System.Collections.Generic;
using UnityEngine;
using Vampwolf.Grid;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Ashstep", menuName = "Spells/Strategies/Ashstep")]
    public class Ashstep : SpellStrategy
    {
        public override GridPredicate Predicate => new GridPredicate((gridCell) => !gridCell.HasEnemyUnit && !gridCell.HasPlayerUnit && !gridCell.HasDeadUnit);

        /// <summary>
        /// Teleport to a nearby location in a flurry of shadow and ash.
        /// </summary>
        public override void Cast(Spell spell, BattleUnit caster, BattleUnit target, List<BattleUnit> unitsInRange, List<BattleUnit> allUnits, Vector3Int gridPosition)
        {
            // Blink the caster to the target position
            caster.Blink(gridPosition);
        }
    }
}
