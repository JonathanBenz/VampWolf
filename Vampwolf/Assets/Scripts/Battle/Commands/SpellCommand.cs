using Cysharp.Threading.Tasks;
using Vampwolf.Spells;

namespace Vampwolf.Battle.Commands
{
    public class SpellCommand : IBattleCommand
    {
        private readonly Spell spell;
        private readonly SpellsModel model;

        public SpellCommand(Spell spell, SpellsModel model)
        {
            this.spell = spell;
            this.model = model;
        }

        /// <summary>
        /// Executes the spell command
        /// </summary>
        public UniTask Execute()
        {
            spell.Cast(model);
            return UniTask.CompletedTask;
        }
    }
}
