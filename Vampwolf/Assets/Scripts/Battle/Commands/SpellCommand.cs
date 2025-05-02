using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Vampwolf.Spells;
using Vampwolf.Units;

namespace Vampwolf.Battles.Commands
{
    public class SpellCommand : IBattleCommand
    {
        private readonly BattleUnit caster;
        private readonly BattleUnit target;
        private readonly List<BattleUnit> unitsInRange;
        private readonly List<BattleUnit> allUnits;
        private readonly Spell spell;
        private readonly Vector3Int gridPosition;

        public SpellCommand(BattleUnit caster, BattleUnit target, List<BattleUnit> unitsInRange, List<BattleUnit> allUnits, Spell spell, Vector3Int gridPosition)
        {
            // Set the caster, target and spell
            this.caster = caster;
            this.target = target;
            this.unitsInRange = unitsInRange;
            this.allUnits = allUnits;
            this.spell = spell;
            this.gridPosition = gridPosition;
        }

        /// <summary>
        /// Executes the spell command
        /// </summary>
        public UniTask Execute()
        {
            spell.Cast(caster, target, unitsInRange, allUnits, gridPosition);
            return UniTask.CompletedTask;
        }
    }
}
