using UnityEngine;

namespace Vampwolf.Spells
{
    public abstract class SpellStrategy : ScriptableObject
    {
        /// <summary>
        /// Cast the spell
        /// </summary>
        public abstract void Cast(Spell spell, SpellsModel model);
    }
}
