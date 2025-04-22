using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Blood Lance", menuName = "Spells/Strategies/Blood Lance")]
    public class BloodLance : SpellStrategy
    {
        /// <summary>
        /// Hurl a spear of blood to damage a single enemy
        /// </summary>
        public override void Cast(Spell spell, BattleUnit unit)
        {
            // Deal damage to the unit
            unit.DealDamage(25);

            // Collect data
            EventBus<DamageDealt>.Raise(new DamageDealt()
            {
                CharacterType = CharacterType.Vampire,
                Amount = 25
            });
        }
    }
}
