using Vampwolf.StateMachines;

namespace Vampwolf.Battle.States
{
    public class BattleState : IState
    {
        protected readonly BattleManager manager;

        public BattleState(BattleManager manager)
        {
            this.manager = manager;
        }

        public virtual void OnEnter() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void OnExit() { }
    }
}
