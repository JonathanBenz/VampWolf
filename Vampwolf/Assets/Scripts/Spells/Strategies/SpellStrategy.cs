using UnityEngine;
using Vampwolf.Grid;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    public abstract class SpellStrategy : ScriptableObject
    {
        /// <summary>
        /// Cast the spell
        /// </summary>
        public abstract void Cast(Spell spell, BattleUnit caster, BattleUnit target);
        public abstract GridPredicate Predicate { get; }
    }
}
