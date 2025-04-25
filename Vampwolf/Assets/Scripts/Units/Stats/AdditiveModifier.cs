using System.Diagnostics;

namespace Vampwolf.Units.Stats
{
    public class AdditiveModifier : StatModifier
    {
        private readonly StatType type;
        private readonly int amount;

        public AdditiveModifier(StatType type, int duration, int amount) : base(duration)
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

            // Add the amount to the query value
            query.Value += amount;
        }
    }
}
