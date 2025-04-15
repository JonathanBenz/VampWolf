using UnityEngine;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    public abstract class SpellStrategy : ScriptableObject
    {
        /// <summary>
        /// Cast the spell
        /// </summary>
        public abstract void Cast(Spell spell, BattleUnit target);
    }
}
