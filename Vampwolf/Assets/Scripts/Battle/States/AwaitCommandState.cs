using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Input;
using Vampwolf.Units;

namespace Vampwolf.Battles.States
{
    public class AwaitCommandState : BattleState
    {
        private readonly InputReader inputReader;

        public AwaitCommandState(BattleManager manager, InputReader inputReader) : base(manager) 
        {
            this.inputReader = inputReader;
            inputReader.Cancel += Cancel;
        }

        ~AwaitCommandState()
        {
            inputReader.Cancel -= Cancel;
        }

        public override void OnEnter()
        {
            // Set the active unit to await commands
            manager.ActiveUnit.AwaitCommands();
        }

        public override void OnExit()
        {
            // Set not commanding
            manager.Commanding = false;
        }

        /// <summary>
        /// Input handler for canceling a spell
        /// </summary>
        private void Cancel(bool started)
        {
            // Get the active unit
            BattleUnit activeUnit = manager.ActiveUnit;

            EventBus<SetMovementSelectionMode>.Raise(new SetMovementSelectionMode()
            {
                GridPosition = activeUnit.GridPosition,
                Range = activeUnit.MovementLeft,
                HighlightType = Grid.HighlightType.Move
            });
        }
    }
}
