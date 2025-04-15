using Cysharp.Threading.Tasks;
using Vampwolf.Units;

namespace Vampwolf.Battle.States
{
    public class NextTurnState : BattleState
    {
        public NextTurnState(BattleManager manager) : base(manager) { }

        public override async void OnEnter()
        {
            // Wait for the active unit to end their turn
            await manager.ActiveUnit.EndTurn();

            // Set to changing turns
            manager.ChangingTurns = true;

            // Move the previous unit to the end of the queue
            BattleUnit lastUnit = manager.TurnQueue.Dequeue();
            manager.TurnQueue.Enqueue(lastUnit);

            // Set the active unit to the next unit in the queue
            manager.ActiveUnit = manager.TurnQueue.Peek();

            // Wait 1 second
            await UniTask.Delay(1000);

            // Set to start the next turn
            manager.ChangingTurns = false;
            manager.SkippingTurn = false;
        }
    }
}
