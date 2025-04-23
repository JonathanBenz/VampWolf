using UnityEngine;
using Vampwolf.Grid;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Melee", menuName = "Spells/Strategies/Enemy/Melee")]
    public class Melee : SpellStrategy
    {
        public override GridPredicate Predicate => new GridPredicate((gridCell) => gridCell.HasPlayerUnit);

        /// <summary>
        /// Deal damage to a single target enemy
        /// </summary>
        public override void Cast(Spell spell, BattleUnit caster, BattleUnit target)
        {
            Debug.Log("Enemy is Casting Melee Attack!");

            // PLACEHOLDER: deal 15 damage
            target.DealDamage(20);
        }
    }
}
