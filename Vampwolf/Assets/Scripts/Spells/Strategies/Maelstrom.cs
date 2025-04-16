using UnityEngine;
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
            // Implement the logic for casting the Maelstrom spell
            Debug.Log("Casting Maelstrom spell!");
        }
    }
}
