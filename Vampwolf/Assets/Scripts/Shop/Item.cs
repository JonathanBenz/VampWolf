using UnityEngine;

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
        public UserType User => data.User;
        public Sprite Icon => data.Icon;

        public Item(ItemData data)
        {
            this.data = data;
        }

        public void Buy()
        {
            // Exit case - the item is already bought
            if (bought) return;

            // Set to bought
            bought = true;

            // TODO: Remove money
        }
    }
}
