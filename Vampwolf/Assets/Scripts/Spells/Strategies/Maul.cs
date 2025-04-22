using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Maul", menuName = "Spells/Strategies/Maul")]
    public class Maul : SpellStrategy
    {
        /// <summary>
        /// Deal damage to a single target enemy
        /// </summary>
        public override void Cast(Spell spell, BattleUnit unit)
        {
            // Deal damage to the unit
            unit.DealDamage(30);

            // Collect data
            EventBus<DamageDealt>.Raise(new DamageDealt()
            {
                CharacterType = CharacterType.Werewolf,
                Amount = 30
            });
        }
    }
}
