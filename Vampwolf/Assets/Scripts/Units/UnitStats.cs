using UnityEngine;

namespace Vampwolf.Units
{
    public class UnitStats
    {
        public int Might;
        public int Fortitude;
        public int Agility;

        private int minMovementRange;
        private int maxMovementRange;
        private int minStatValue;
        private int maxStatValue;

        public int MovementRange
        {
            get
            {
                return (int)Mathf.Lerp(minMovementRange, maxMovementRange, (Agility - minStatValue) / maxStatValue - 1f);
            }
        }

        public int Initiative { get => Agility; }

        public UnitStats(UnitStatData data)
        {
            // Set general values
            minStatValue = data.minStatValue;
            maxStatValue = data.maxStatValue;

            // Set stat values
            Might = data.Might;
            Fortitude = data.Fortitude;
            Agility = data.Agility;

            // Set speed-specific values
            minMovementRange = data.minMovementRange;
            maxMovementRange = data.maxMovementRange;
        }
    }
}
