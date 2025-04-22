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

            // If there are no enemies
            if (manager.NumberOfEnemies <= 0) EventBus<BattleWon>.Raise(new BattleWon());

            // Else, there must be no players left
            else UnityEngine.SceneManagement.SceneManager.LoadScene(5); // EXTREMELY LAZY WAY OF GOING TO LOSE SCREEN
        }
    }
}
