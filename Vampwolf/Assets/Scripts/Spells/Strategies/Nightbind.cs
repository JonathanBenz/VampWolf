using UnityEngine;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Nightbind", menuName = "Spells/Strategies/Nightbind")]
    public class Nightbind : SpellStrategy
    {
        /// <summary>
        /// Stun an enemy for 1 turn
        /// </summary>
        public override void Cast(Spell spell, BattleUnit unit)
        {
        }
    }
}
