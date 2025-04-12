using UnityEngine;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Maul", menuName = "Spells/Strategies/Maul")]
    public class Maul : SpellStrategy
    {
        /// <summary>
        /// Deal damage to a single target enemy
        /// </summary>
        public override void Cast(Spell spell, SpellsModel model)
        {
            // Implement the logic for casting the Maul spell
            Debug.Log("Casting Maul spell!");

            model.Rage += 15f;
        }
    }
}
