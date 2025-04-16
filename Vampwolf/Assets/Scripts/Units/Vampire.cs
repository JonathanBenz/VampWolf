using Cysharp.Threading.Tasks;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf.Units
{
    public class Vampire : BattleUnit
    {
        /// <summary>
        /// Start the turn by highlighting the cells around the active unit and displaying the werewolf UI
        /// </summary>
        public override async UniTask StartTurn()
        {
            // Set that the werewolf has not moved or attacked
            hasMoved = false;
            hasCasted = false;

            // Enable the grid selector
            EventBus<SetGridSelector>.Raise(new SetGridSelector()
            {
                Active = true
            });

            // Set the movement selection mode
            EventBus<SetMovementSelectionMode>.Raise(new SetMovementSelectionMode()
            {
                GridPosition = gridPosition,
                Range = stats.MovementRange
            });

            // Display the werewolf UI
            EventBus<ShowSpells>.Raise(new ShowSpells()
            {
                CharacterType = Spells.CharacterType.Vampire,
                CastingUnit = this
            });

            // Show the end turn button
            EventBus<SetEndTurnButton>.Raise(new SetEndTurnButton()
            {
                Active = true
            });

            await UniTask.CompletedTask;
        }

        /// <summary>
        /// Wait for the player to issue a command
        /// </summary>
        public override void AwaitCommands()
        {
            // Enable the grid selector
            EventBus<SetGridSelector>.Raise(new SetGridSelector()
            {
                Active = true
            });

            // Exit case - has already moved
            if (hasMoved) return;

            // Set the movement selection mode
            EventBus<SetMovementSelectionMode>.Raise(new SetMovementSelectionMode()
            {
                GridPosition = gridPosition,
                Range = stats.MovementRange
            });
        }

        public override async UniTask EndTurn()
        {
            // Hide the end turn button
            EventBus<SetEndTurnButton>.Raise(new SetEndTurnButton()
            {
                Active = false
            });

            // Hide the spell bars
            EventBus<HideSpells>.Raise(new HideSpells());

            // Clear the highlights
            EventBus<ClearHighlights>.Raise(new ClearHighlights());

            await UniTask.CompletedTask;
        }

        protected override void OnDeath()
        {
            EventBus<RemoveUnit>.Raise(new RemoveUnit()
            {
                Unit = this,
                IsEnemy = false
            });

            Destroy(this);
        }
    }
}
