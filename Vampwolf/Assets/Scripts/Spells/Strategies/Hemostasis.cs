using UnityEngine;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Hemostatis", menuName = "Spells/Strategies/Hemostasis")]
    public class Hemostasis : SpellStrategy
    {
        /// <summary>
        /// Heal a small amount yourself or an ally
        /// </summary>
        public override void Cast(Spell spell, BattleUnit unit)
        {
            // Implement the logic for casting the Hemostasis spell

            // PLACEHOLDER: heal 15 damage
            unit.DealDamage(-15);
        }
    }
}
