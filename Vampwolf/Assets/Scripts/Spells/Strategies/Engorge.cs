using System.Collections.Generic;
using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Grid;
using Vampwolf.Units;
using Vampwolf.Units.Stats;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Engorge", menuName = "Spells/Strategies/Engorge")]
    public class Engorge : SpellStrategy
    {
        public override GridPredicate Predicate => new GridPredicate((gridCell) => gridCell.HasDeadUnit);

        /// <summary>
        /// Devour an enemy to boost the Werewolf's Might and Fortitude by 5
        /// </summary>
        public override void Cast(Spell spell, BattleUnit caster, BattleUnit target, List<BattleUnit> unitsInRange, List<BattleUnit> allUnits, Vector3Int gridPosition)
        {
            // Remove the eaten unit
            EventBus<RemoveGridCellUnit>.Raise(new RemoveGridCellUnit()
            {
                GridPosition = target.GridPosition
            });

            // Construct the stat modifiers
            AdditiveModifier attackModifier = new AdditiveModifier(StatType.Might, 3, 5);
            AdditiveModifier fortitudeModifier = new AdditiveModifier(StatType.Fortitude, 3, 5);

            // Add the modifiers to the caster
            caster.AddStatModifier(attackModifier);
            caster.AddStatModifier(fortitudeModifier);
        }
    }
}
