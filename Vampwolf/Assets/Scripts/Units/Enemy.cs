using Cysharp.Threading.Tasks;
using Vampwolf.EventBus;

namespace Vampwolf.Units
{
    public class Enemy : BattleUnit
    {
        public override void AwaitCommands()
        {
            // Skip turn for now
            EventBus<SkipTurn>.Raise(new SkipTurn());
        }

        public override async UniTask EndTurn()
        {
            await UniTask.CompletedTask;
        }

        public override async UniTask StartTurn()
        {
            hasMoved = false;
            hasCasted = false;

            await UniTask.CompletedTask;
        }

        protected override void OnDeath()
        {
            // Remove the unit
            EventBus<RemoveUnit>.Raise(new RemoveUnit()
            {
                Unit = this,
                IsEnemy = true
            });

            // Hide the unit
            gameObject.SetActive(false);
        }
    }
}
