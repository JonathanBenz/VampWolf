using System;

namespace Vampwolf.Units.Stats
{
    public abstract class StatModifier : IDisposable
    {
        private int turnsLeft;

        public bool MarkedForRemoval { get; private set; }
        public event Action<StatModifier> OnDispose = delegate { };

        protected StatModifier(int duration)
        {
            // Exit case - if the duration is less than or equal to 0
            if (turnsLeft <= 0) return;

            // Set the duration of the modifier
            turnsLeft = duration;
        }

        /// <summary>
        /// Update the remaining amount of turns for the stat modifier
        /// </summary>
        public void UpdateRemainingTurns()
        {
            // Decrease the turns left
            turnsLeft--;

            // Exit case - if there are still turns remaining
            if (turnsLeft > 0) return;

            // Mark the modifier for removal
            MarkedForRemoval = true;
        }

        /// <summary>
        /// Handle the Stat Modifier
        /// </summary>
        public abstract void Handle(object sender, Query query);

        /// <summary>
        /// Dispose of the Stat Modifier
        /// </summary>
        public void Dispose() => OnDispose.Invoke(this);
    }
}
