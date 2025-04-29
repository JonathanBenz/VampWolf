using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Grid;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    [CreateAssetMenu(fileName = "Maelstrom", menuName = "Spells/Strategies/Maelstrom")]
    public class Maelstrom : SpellStrategy
    {
        public override GridPredicate Predicate => new GridPredicate((gridCell) => gridCell.HasEnemyUnit);

        /// <summary>
        /// Damage all enemies within range
        /// </summary>
        public override void Cast(Spell spell, BattleUnit caster, BattleUnit target)
        {
            // Collect data
            EventBus<DamageDealt>.Raise(new DamageDealt()
            {
                CharacterType = CharacterType.Werewolf,
                Amount = 25
            });
        }
    }
}
