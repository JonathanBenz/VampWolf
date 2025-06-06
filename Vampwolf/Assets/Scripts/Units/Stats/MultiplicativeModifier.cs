using UnityEngine;

namespace Vampwolf.Units.Stats
{
    public class MultiplicativeModifier : StatModifier
    {
        private readonly StatType type;
        private readonly float amount;

        public MultiplicativeModifier(StatType type, int duration, float amount) : base(duration)
        {
            this.type = type;
            this.amount = amount;
        }

        /// <summary>
        /// Handle modification by using addition
        /// </summary>
        public override void Handle(object sender, Query query)
        {
            // Exit case - if the query is not for this stat type
            if (query.StatType != type) return;

            // Multiply the query by an amount
            int finalValue = Mathf.RoundToInt(query.Value * amount);

            // Set the final value
            query.Value = finalValue;
        }
    }
}
