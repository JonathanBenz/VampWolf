using UnityEngine;
using Vampwolf.Units;

namespace Vampwolf
{
    public class GridCell
    {
        private BattleUnit unit;

        public bool HasEnemyUnit => unit != null && IsEnemyUnit();
        public bool HasPlayerUnit => unit != null && IsPlayerUnit();
        public bool HasDeadUnit => unit != null && IsEnemyUnit() && unit.Dead;
        public bool HasAnyUnit => unit != null;

        public GridCell()
        {
            // Default a null unit
            unit = null;
        }

        /// <summary>
        /// Set a unit to this cell
        /// </summary>
        public void SetUnit(BattleUnit unit) => this.unit = unit;
        
        /// <summary>
        /// Remove a unit from this cell
        /// </summary>
        public void RemoveUnit() => unit = null;

        /// <summary>
        /// Check if the attached unit is an enemy unit
        /// </summary>
        public bool IsEnemyUnit() => unit is not Werewolf && unit is not Vampire;

        /// <summary>
        /// Check if the attached unit is a player unit
        /// </summary>
        public bool IsPlayerUnit() => unit is Werewolf || unit is Vampire;
    }
}
