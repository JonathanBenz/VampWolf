namespace Vampwolf.Battle.States
{
    public class StartTurnState : BattleState
    {
        public StartTurnState(BattleManager manager) : base(manager) { }

        public override async void OnEnter()
        {
            // Start the turn for the active unit
            await manager.ActiveUnit.StartTurn();

            // Set to commanding
            manager.Commanding = true;
            manager.SkippingTurn = false;
        }
    }
}
