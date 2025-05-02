using System.Collections.Generic;
using UnityEngine;
using Vampwolf.Grid;
using Vampwolf.Units;
using Vampwolf.Units.Stats;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Bloodbind", menuName = "Spells/Strategies/Bloodbind")]
    public class Bloodbind : SpellStrategy
    {
        public override GridPredicate Predicate => new GridPredicate((gridCell) => gridCell.HasEnemyUnit);

        /// <summary>
        /// Entangle an enemy in pulsing tendrils of blood, preventing their movement for a turn.
        /// </summary>
        public override void Cast(Spell spell, BattleUnit caster, BattleUnit target, List<BattleUnit> unitsInRange, List<BattleUnit> allUnits, Vector3Int gridPosition)
        {
            // Create the agility modifier (go negative so that the target can't move)
            AdditiveModifier agilityModifier = new AdditiveModifier(StatType.Agility, 1, -100);

            // Add the stat modifier to the target
            target.AddStatModifier(agilityModifier);
        }
    }
}
