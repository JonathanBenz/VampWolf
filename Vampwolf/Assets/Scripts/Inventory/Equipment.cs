using System;
using UnityEngine;
using Vampwolf.Shop;
using Vampwolf.Spells;

namespace Vampwolf.Inventory
{
    [Serializable]
    public class Equipment
    {
        [SerializeField] private string name;
        [SerializeField] private string description;
        [SerializeField] private string flavor;
        private readonly Sprite icon;
        private readonly SpellData spell;
        [SerializeField] private UserType user;
        [SerializeField] private bool equipped;

        public string Name => name;
        public string Description => description;
        public string Flavor => flavor;
        public Sprite Icon => icon;
        public UserType User => user;
        public bool Equipped => equipped;
        public SpellData Spell => spell;

        public Equipment(Item item)
        {
            name = item.Name;
            description = item.Description;
            flavor = item.Flavor;
            icon = item.Icon;
            user = item.User;
            spell = item.Spell;
        }

        /// <summary>
        /// Equip the equipment
        /// </summary>
        public void Equip()
        {
            // Set the item equipped
            equipped = true;

            // Exit case - an item tracker does not exist
            if (!ItemTracker.HasInstance) return;

            // Add the equipment to the item tracker
            ItemTracker.Instance.AddEquipment(this);
        }

        /// <summary>
        /// Unequip the equipment
        /// </summary>
        public void Unequip()
        {
            // Set the item unequipped
            equipped = false;

            // Exit case - an item tracker does not exist
            if (!ItemTracker.HasInstance) return;

            // Remove the equipment from the item tracker
            ItemTracker.Instance.RemoveEquipment(this);
        }
    }
}
