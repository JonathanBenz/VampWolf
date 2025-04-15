using Vampwolf.EventBus;

namespace Vampwolf.Battles.States
{
    public class StartTurnState : BattleState
    {
        public StartTurnState(BattleManager manager) : base(manager) { }

        public override async void OnEnter()
        {
            // Notify that a turn has started
            EventBus<TurnStarted>.Raise(new TurnStarted()
            {
                Unit = manager.ActiveUnit
            });

            // Start the turn for the active unit
            await manager.ActiveUnit.StartTurn();

            // Set to commanding
            manager.Commanding = true;
            manager.SkippingTurn = false;
        }
    }
}
