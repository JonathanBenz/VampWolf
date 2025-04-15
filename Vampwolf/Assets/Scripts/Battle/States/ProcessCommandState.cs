using Vampwolf.Battle.Commands;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf.Battle.States
{
    public class ProcessCommandState : BattleState
    {
        public ProcessCommandState(BattleManager manager) : base(manager) { }

        public override async void OnEnter()
        {
            // Disable the grid selector
            EventBus<SetGridSelector>.Raise(new SetGridSelector()
            {
                Active = false
            });

            EventBus<ClearHighlights>.Raise(new ClearHighlights());

            // Set processing
            manager.Processing = true;

            // Extract the command from the queue
            IBattleCommand command = manager.CommandQueue.Dequeue();

            // Await it's execution
            await command.Execute();

            manager.Processing = false;

            // Check if the active unit has not moved or casted
            if (!manager.ActiveUnit.HasMoved || !manager.ActiveUnit.HasCasted)
                // Set commanding to true
                manager.Commanding = true;
        }
    }
}
