namespace Vampwolf.Battle.States
{
    public class AwaitCommandState : BattleState
    {
        public AwaitCommandState(BattleManager manager) : base(manager) { }

        public override void OnEnter()
        {
            // Start the turn for the active unit
            manager.ActiveUnit.StartTurn();
        }
    }
}
