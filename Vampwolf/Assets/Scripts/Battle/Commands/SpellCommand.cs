using Cysharp.Threading.Tasks;
using Vampwolf.Spells;
using Vampwolf.Units;

namespace Vampwolf.Battles.Commands
{
    public class SpellCommand : IBattleCommand
    {
        private readonly BattleUnit target;
        private readonly Spell spell;

        public SpellCommand(BattleUnit target, Spell spell)
        {
            this.target = target;
            this.spell = spell;
        }

        /// <summary>
        /// Executes the spell command
        /// </summary>
        public UniTask Execute()
        {
            spell.Cast(target);
            return UniTask.CompletedTask;
        }
    }
}
