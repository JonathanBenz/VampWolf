using UnityEngine;
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
            // Implement the logic for casting the Eclipse spell
            Debug.Log("Casting Eclipse spell!");
        }
    }
}
