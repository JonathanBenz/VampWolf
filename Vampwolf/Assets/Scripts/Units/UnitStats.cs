using UnityEngine;

namespace Vampwolf.Units
{
    public class UnitStats
    {
        public int Might;
        public int Fortitude;
        public int Speed;

        private int minMovementRange;
        private int maxMovementRange;
        private int minStatValue;
        private int maxStatValue;

        public int MovementRange
        {
            get
            {
                return (int)Mathf.Lerp(minMovementRange, maxMovementRange, (Speed - minStatValue) / maxStatValue - 1f);
            }
        }

        public int Initiative { get => Speed; }

        public UnitStats(UnitStatData data)
        {
            // Set general values
            minStatValue = data.minStatValue;
            maxStatValue = data.maxStatValue;

            // Set speed-specific values
            minMovementRange = data.minMovementRange;
            maxMovementRange = data.maxMovementRange;
        }
    }
}
