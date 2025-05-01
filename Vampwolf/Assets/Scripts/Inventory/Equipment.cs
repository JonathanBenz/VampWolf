using UnityEngine;
using Vampwolf.Shop;

namespace Vampwolf.Inventory
{
    public class Equipment
    {
        private readonly string name;
        private readonly string description;
        private readonly string flavor;
        private readonly Sprite icon;
        private readonly UserType user;
        private bool equipped;

        public string Name => name;
        public string Description => description;
        public string Flavor => flavor;
        public Sprite Icon => icon;
        public UserType User => user;
        public bool Equipped => equipped;

        public Equipment(Item item)
        {
            name = item.Name;
            description = item.Description;
            flavor = item.Flavor;
            icon = item.Icon;
            user = item.User;
        }

        /// <summary>
        /// Equip the equipment
        /// </summary>
        public void Equip()
        {
            equipped = true;
        }

        /// <summary>
        /// Unequip the equipment
        /// </summary>
        public void Unequip()
        {
            equipped = false;
        }
    }
}
