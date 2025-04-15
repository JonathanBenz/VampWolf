using UnityEngine;
using Cysharp.Threading.Tasks;

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
        [SerializeField] protected int actionsTakenThisTurn;

        public int Initiative => stats.Initiative;

        private void Awake()
        {
            // Initialize the Unit Stats
            stats = new UnitStats(statData);

            // Set to no actions taken this turn
            actionsTakenThisTurn = 0;
        }

        /// <summary>
        /// Trigger behaviour on the start of the turn
        /// </summary>
        public abstract UniTask StartTurn();

        /// <summary>
        /// Move to a specified grid position
        /// </summary>
        public async UniTask MoveTo(Vector3Int targetPosition)
        {
            // Simulate movement with a delay
            await UniTask.Delay(1000);
            gridPosition = targetPosition;
        }
    }
}
