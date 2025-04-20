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
            // Set that the werewolf has not moved or attacked
            movementLeft = MovementRange;
            hasCasted = false;
            hasCurrentTurn = true;

            // Enable the grid selector
            EventBus<SetGridSelector>.Raise(new SetGridSelector()
            {
                Active = true
            });

            // Set the movement selection mode
            EventBus<SetMovementSelectionMode>.Raise(new SetMovementSelectionMode()
            {
                GridPosition = gridPosition,
                Range = stats.MovementRange,
                TileColor = 0
            });

            // Display the werewolf UI
            EventBus<ShowSpells>.Raise(new ShowSpells()
            {
                CharacterType = Spells.CharacterType.Werewolf,
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
            if (movementLeft <= 0) return;

            // Set the movement selection mode
            EventBus<SetMovementSelectionMode>.Raise(new SetMovementSelectionMode()
            {
                GridPosition = gridPosition,
                Range = movementLeft,
                TileColor = 0
            });
        }

        public override async UniTask EndTurn()
        {
            hasCurrentTurn = false;

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

            // Display unit death sprite
            spriteRenderer.sprite = statData.deathSprite;
        }

        private void FixedUpdate()
        {
            if (hasCurrentTurn) UpdateCharacterSprite(UnityEngine.Camera.main.ScreenToWorldPoint(UnityEngine.InputSystem.Mouse.current.position.ReadValue()));
        }
    }
}
