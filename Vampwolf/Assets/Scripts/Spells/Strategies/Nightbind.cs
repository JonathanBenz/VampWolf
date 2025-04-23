using UnityEngine;
using Vampwolf.Grid;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Nightbind", menuName = "Spells/Strategies/Nightbind")]
    public class Nightbind : SpellStrategy
    {
        public override GridPredicate Predicate => new GridPredicate((gridCell) => gridCell.HasEnemyUnit);

        /// <summary>
        /// Stun an enemy for 1 turn
        /// </summary>
        public override void Cast(Spell spell, BattleUnit caster, BattleUnit target)
        {
        }
    }
}
