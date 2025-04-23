using Cysharp.Threading.Tasks;
using Vampwolf.Spells;
using Vampwolf.Units;

namespace Vampwolf.Battles.Commands
{
    public class SpellCommand : IBattleCommand
    {
        private readonly BattleUnit caster;
        private readonly BattleUnit target;
        private readonly Spell spell;

        public SpellCommand(BattleUnit caster, BattleUnit target, Spell spell)
        {
            // Set the caster, target and spell
            this.caster = caster;
            this.target = target;
            this.spell = spell;
        }

        /// <summary>
        /// Executes the spell command
        /// </summary>
        public UniTask Execute()
        {
            spell.Cast(caster, target);
            return UniTask.CompletedTask;
        }
    }
}
