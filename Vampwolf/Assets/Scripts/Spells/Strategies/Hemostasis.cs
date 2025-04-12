using UnityEngine;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Hemostatis", menuName = "Spells/Strategies/Hemostasis")]
    public class Hemostasis : SpellStrategy
    {
        /// <summary>
        /// Heal a small amount yourself or an ally
        /// </summary>
        public override void Cast(Spell spell, SpellsModel model)
        {
            // Implement the logic for casting the Hemostasis spell
            Debug.Log("Casting Hemostasis spell!");

            model.Blood += 15f;
        }
    }
}
