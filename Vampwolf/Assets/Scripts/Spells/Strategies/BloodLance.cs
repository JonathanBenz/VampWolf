using UnityEngine;
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
            // Implement the logic for casting the Blood Lance spell
            Debug.Log("Casting Blood Lance spell!");

            unit.DealDamage(25);
        }
    }
}
