using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Eclipse", menuName = "Spells/Strategies/Eclipse")]
    public class Eclipse : SpellStrategy
    {
        /// <summary>
        /// Leap to an enemy and damage them
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
