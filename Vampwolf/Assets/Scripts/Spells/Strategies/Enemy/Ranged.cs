using UnityEngine;
using Vampwolf.Grid;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Ranged", menuName = "Spells/Strategies/Enemy/Ranged")]
    public class Ranged : SpellStrategy
    {
        public override GridPredicate Predicate => new GridPredicate((gridCell) => gridCell.HasPlayerUnit);

        /// <summary>
        /// Deal damage to a single target enemy
        /// </summary>
        public override void Cast(Spell spell, BattleUnit caster, BattleUnit target)
        {
            Debug.Log("Enemy is Casting Ranged Attack!");
            target.DealDamage(15);
        }
    }
}
