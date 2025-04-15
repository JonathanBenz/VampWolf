using UnityEngine;
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
            // Implement the logic for casting the Maul spell
            Debug.Log("Casting Maul spell!");

            // PLACEHOLDER: deal 15 damage
            unit.DealDamage(30);
        }
    }
}
