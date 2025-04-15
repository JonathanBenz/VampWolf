using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Vampwolf.Grid;
using DG.Tweening;

namespace Vampwolf.Units
{
    public abstract class BattleUnit : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] public UnitStatData statData;

        [Header("Details")]
        [SerializeField] protected Vector3Int gridPosition;
        [SerializeField] protected int health;
        [SerializeField] protected UnitStats stats;
        [SerializeField] protected bool hasMoved;
        [SerializeField] protected bool hasCasted;

        public Vector3Int GridPosition => gridPosition;
        public int Initiative => stats.Initiative;
        public bool HasMoved { get => hasMoved; set => hasMoved = value; }
        public bool HasCasted { get => hasCasted; set => hasCasted = value; }

        private void Awake()
        {
            // Initialize the Unit Stats
            stats = new UnitStats(statData);

            // Set to no actions taken this turn
            hasMoved = false;
            hasCasted = false;
        }

        /// <summary>
        /// Trigger behaviour on the start of the turn
        /// </summary>
        public abstract UniTask StartTurn();

        /// <summary>
        /// Trigger behaviour on the end of the turn
        /// </summary>
        public abstract UniTask EndTurn();

        public abstract void AwaitCommands();

        /// <summary>
        /// Move to a specified grid position
        /// </summary>
        public async UniTask MoveThrough(GridManager gridManager, List<Vector3Int> path)
        {
            // Loop through the path
            foreach (Vector3Int position in path)
            {
                // Get the world position
                Vector3 worldPos = gridManager.GetWorldPositionFromGrid(position);

                // Wait for the unit to move to the new position
                await transform.DOMove(worldPos, 0.5f).SetEase(Ease.Linear).ToUniTask();

                // Update the grid position
                gridPosition = position;
            }
        }

        /// <summary>
        /// Deal damage to the unit
        /// </summary>
        public void DealDamage(int damage)
        {
            // Subtract the damage from the health
            health -= damage;

            // Check if the unit is dead
            CheckDeath();
        }

        /// <summary>
        /// Check if the unit is dead
        /// </summary>
        private void CheckDeath()
        {
            // Exit case - the unit is not dead
            if (health > 0) return;

            // Trigger the death behaviour
            OnDeath();
        }

        /// <summary>
        /// Trigger behaviour on death
        /// </summary>
        protected abstract void OnDeath();
    }
}
