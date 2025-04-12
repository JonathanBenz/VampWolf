using UnityEngine;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Blood Lance", menuName = "Spells/Strategies/Blood Lance")]
    public class BloodLance : SpellStrategy
    {
        /// <summary>
        /// Hurl a spear of blood to damage a single enemy
        /// </summary>
        public override void Cast(Spell spell)
        {
            // Implement the logic for casting the Blood Lance spell
            Debug.Log("Casting Blood Lance spell!");
        }
    }
}
