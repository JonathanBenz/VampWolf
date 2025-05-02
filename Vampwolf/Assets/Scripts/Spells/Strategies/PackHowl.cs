using System.Collections.Generic;
using UnityEngine;
using Vampwolf.Grid;
using Vampwolf.Units;
using Vampwolf.Units.Stats;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Pack Howl", menuName = "Spells/Strategies/Pack Howl")]
    public class PackHowl : SpellStrategy
    {
        public override GridPredicate Predicate => new GridPredicate((gridCell) => gridCell.HasPlayerUnit);

        /// <summary>
        /// Let out a harmonious howl that buffs Agility and Fortitude for both the Werewolf and the Vampire.
        /// </summary>
        public override void Cast(Spell spell, BattleUnit caster, BattleUnit target, List<BattleUnit> unitsInRange, List<BattleUnit> allUnits, Vector3Int gridPosition)
        {
            // Construct the stat modifiers
            AdditiveModifier agilityModifier = new AdditiveModifier(StatType.Agility, 3, 5);
            AdditiveModifier fortitudeModifier = new AdditiveModifier(StatType.Fortitude, 3, 5);

            // Add the modifiers to the caster
            caster.AddStatModifier(agilityModifier);
            caster.AddStatModifier(fortitudeModifier);

            // Add the modifiers to the target
            target.AddStatModifier(agilityModifier);
            target.AddStatModifier(fortitudeModifier);
        }
    }
}
