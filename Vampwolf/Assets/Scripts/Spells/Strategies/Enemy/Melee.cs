using UnityEngine;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Melee", menuName = "Spells/Strategies/Enemy/Melee")]
    public class Melee : SpellStrategy
    {
        /// <summary>
        /// Deal damage to a single target enemy
        /// </summary>
        public override void Cast(Spell spell, BattleUnit unit)
        {
            Debug.Log("Enemy is Casting Melee Attack!");

            // PLACEHOLDER: deal 15 damage
            unit.DealDamage(20);
        }
    }
}
