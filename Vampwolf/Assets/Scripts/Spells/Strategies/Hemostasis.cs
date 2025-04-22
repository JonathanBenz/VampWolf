using UnityEngine;
using Vampwolf.EventBus;
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
            // Heal the unit
            unit.DealDamage(-15);

            // Collect data
            EventBus<DamageHealed>.Raise(new DamageHealed()
            {
                CharacterType = CharacterType.Vampire,
                Amount = 15
            });
        }
    }
}
