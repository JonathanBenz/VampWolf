using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf.Battles.States
{
    public class EndBattleState : BattleState
    {
        public EndBattleState(BattleManager manager) : base(manager) { }

        public override void OnEnter()
        {
            // Clear highlights
            EventBus<ClearHighlights>.Raise(new ClearHighlights());

            // Hide spells
            EventBus<HideSpells>.Raise(new HideSpells());

            // Hide the end turn button
            EventBus<SetEndTurnButton>.Raise(new SetEndTurnButton()
            {
                Active = false
            });

            EventBus<BattleWon>.Raise(new BattleWon());
        }
    }
}
