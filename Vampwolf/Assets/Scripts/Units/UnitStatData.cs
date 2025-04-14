using UnityEngine;

namespace Vampwolf.Units
{
    [CreateAssetMenu(fileName = "Unit Stats", menuName = "Units/Stats")]
    public class UnitStatData : ScriptableObject
    {
        [Header("General")]
        public int minStatValue;
        public int maxStatValue;

        [Header("Speed")]
        public int minMovementRange;
        public int maxMovementRange;
    }
}
