using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Maelstrom", menuName = "Spells/Strategies/Maelstrom")]
    public class Maelstrom : SpellStrategy
    {
        /// <summary>
        /// Damage all enemies within range
        /// </summary>
        public override void Cast(Spell spell, BattleUnit unit)
        {
            // Collect data
            EventBus<DamageDealt>.Raise(new DamageDealt()
            {
                CharacterType = CharacterType.Werewolf,
                Amount = 25
            });
        }
    }
}
