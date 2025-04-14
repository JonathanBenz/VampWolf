using UnityEngine;

namespace Vampwolf.Units
{
    public class BattleUnit : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] public UnitStatData StatData;

        [Header("Details")]
        [SerializeField] private Vector2Int gridPosition;
        [SerializeField] private int health;
        [SerializeField] private UnitStats stats;

        private void Awake()
        {
            // Initialize the Unit Stats
            stats = new UnitStats(StatData);
        }
    }
}
