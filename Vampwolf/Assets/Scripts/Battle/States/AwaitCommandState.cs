namespace Vampwolf.Battles.States
{
    public class AwaitCommandState : BattleState
    {
        public AwaitCommandState(BattleManager manager) : base(manager) { }

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
    }
}
