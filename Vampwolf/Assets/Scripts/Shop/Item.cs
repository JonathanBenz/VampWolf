using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Spells;

namespace Vampwolf.Shop
{
    public class Item
    {
        private readonly ItemData data;
        private bool bought;

        public string Name => data.Name;
        public string Description => data.Description;
        public string Flavor => data.Flavor;
        public int Cost => data.Cost;
        public bool Bought => bought;
        public UserType User => data.User;
        public Sprite Icon => data.Icon;
        public SpellData Spell => data.Spell;

        public Item(ItemData data)
        {
            this.data = data;
        }

        /// <summary>
        /// Buy the item
        /// </summary>
        public void Buy()
        {
            // Exit case - the item is already bought
            if (bought) return;

            // Set to bought
            bought = true;

            // Remove gold from the bank
            Bank.Instance.RemoveGold(Bank.Instance.Gold);

            // Add the item to the inventory
            EventBus<AddItemToInventory>.Raise(new AddItemToInventory()
            {
                Item = this
            });
        }
    }
}
