using UnityEngine;

namespace Vampwolf.Units.Stats
{
    public enum StatType
    {
        Might,
        Fortitude,
        Agility
    }

    public class UnitStats
    {
        private readonly StatsMediator mediator;
        private readonly int baseMight;
        private readonly int baseFortitude;
        private readonly int baseAgility;

        private readonly int minMovementRange;
        private readonly int maxMovementRange;
        private readonly int minStatValue;
        private readonly int maxStatValue;

        public int Might
        {
            get
            {
                // Create the query
                Query q = new Query(StatType.Might, baseMight);

                // Perform the query
                mediator.PerformQuery(this, q);

                // Return the final, modified value
                return q.Value;
            }
        }

        public int Fortitude
        {
            get
            {
                // Create the query
                Query q = new Query(StatType.Fortitude, baseFortitude);

                // Perform the query
                mediator.PerformQuery(this, q);

                // Return the final, modified value
                return q.Value;
            }
        }

        public int Agility
        {
            get
            {
                // Create the query
                Query q = new Query(StatType.Agility, baseAgility);

                // Perform the query
                mediator.PerformQuery(this, q);

                // Return the final, modified value
                return q.Value;
            }
        }

        public int MovementRange
        {
            get
            {
                if (Agility < 0) return 0;

                return (int)Mathf.Lerp(minMovementRange, maxMovementRange, (Agility - minStatValue) / maxStatValue - 1f);
            }
        }

        public int Initiative { get => Agility + Random.Range(1, 7); } // Roll 1D6 + Agility for Initiative roll

        public int Modifiers => mediator.CurrentModifiers;

        public UnitStats(UnitStatData data)
        {
            // Initialize the stas mediator
            mediator = new StatsMediator();

            // Set general values
            minStatValue = data.minStatValue;
            maxStatValue = data.maxStatValue;

            // Set stat values
            baseMight = data.Might;
            baseFortitude = data.Fortitude;
            baseAgility = data.Agility;

            // Set speed-specific values
            minMovementRange = data.minMovementRange;
            maxMovementRange = data.maxMovementRange;
        }

        /// <summary>
        /// Add a stat modifier to the stats mediator
        /// </summary>
        public void AddModifier(StatModifier modifier) => mediator.AddModifier(modifier);

        /// <summary>
        /// Udpate the stat modifiers
        /// </summary>
        public void UpdateModifiers() => mediator.Update();
    }
}
