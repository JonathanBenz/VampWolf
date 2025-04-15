using Cysharp.Threading.Tasks;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf.Units
{
    public class Werewolf : BattleUnit
    {
        /// <summary>
        /// Start the turn by highlighting the cells around the active unit and displaying the werewolf UI
        /// </summary>
        public override async UniTask StartTurn()
        {
            // Highlight the cells around the active unit
            EventBus<HighlightCells>.Raise(new HighlightCells()
            {
                GridPosition = gridPosition,
                Range = stats.MovementRange
            });

            // Display the werewolf UI
            EventBus<ShowSpells>.Raise(new ShowSpells()
            {
                CharacterType = Spells.CharacterType.Werewolf
            });

            await UniTask.CompletedTask;
        }
    }
}
