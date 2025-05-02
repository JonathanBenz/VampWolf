using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Utilities.Singletons;
using UnityEngine;

namespace Vampwolf
{
    public class Bank : PersistentSingleton<Bank>
    {
        [SerializeField] private int gold;

        public int Gold => gold;

        /// <summary>
        /// Set the gold amount in the bank
        /// </summary>
        public void SetGold(int amount)
        {
            // Set the gold amount
            gold = amount;

            // Notify that the gold has changed
            EventBus<UpdateGold>.Raise(new UpdateGold()
            {
                CurrentGold = gold
            });
        }

        /// <summary>
        /// Add gold to the bank
        /// </summary>
        public void AddGold(int amount)
        {
            // Update the gold amount
            gold += amount;

            // Notify that the gold has changed
            EventBus<UpdateGold>.Raise(new UpdateGold()
            {
                CurrentGold = gold
            });
        }

        /// <summary>
        /// Remove gold from the bank
        /// </summary>
        public void RemoveGold(int amount)
        {
            // Update the gold amount
            gold -= amount;

            // Notify that the gold has changed
            EventBus<UpdateGold>.Raise(new UpdateGold()
            {
                CurrentGold = gold
            });
        }
    }
}
