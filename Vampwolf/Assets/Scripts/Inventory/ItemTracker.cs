using System.Collections.Generic;
using Unity.VisualScripting;
using Vampwolf.Shop;
using Vampwolf.Utilities.Singletons;

namespace Vampwolf.Inventory
{
    public class ItemTracker : PersistentSingleton<ItemTracker>
    {
        private List<Equipment> vampireEquipment;
        private List<Equipment> werewolfEquipment;

        public List<Equipment> VampireEquipment => vampireEquipment;
        public List<Equipment> WerewolfEquipment => werewolfEquipment;

        protected override void Awake()
        {
            base.Awake();

            // Initialize the lists
            vampireEquipment = new List<Equipment>();
            werewolfEquipment = new List<Equipment>();
        }

        /// <summary>
        /// Add a piece of equipment to be tracked
        /// </summary>
        public void AddEquipment(Equipment equipment)
        {
            switch (equipment.User)
            {
                case UserType.Vampire:
                    // Exit case - the equipment is already in the vampire list
                    if (vampireEquipment.Contains(equipment)) return;

                    // Add the vampire equipment to the list
                    vampireEquipment.Add(equipment);
                    break;

                case UserType.Werewolf:
                    // Exit case - the equipment is already in the werewolf list
                    if (werewolfEquipment.Contains(equipment)) return;

                    // Add the werewolf equipment to the list
                    werewolfEquipment.Add(equipment);
                    break;
            }
        }

        /// <summary>
        /// Add a piece of equipment to be tracked
        /// </summary>
        public void RemoveEquipment(Equipment equipment)
        {
            switch (equipment.User)
            {
                case UserType.Vampire:
                    // Exit case - the equipment is not in the vampire list
                    if (!vampireEquipment.Contains(equipment)) return;

                    // Remove the vampire equipment from the list
                    vampireEquipment.Remove(equipment);
                    break;

                case UserType.Werewolf:
                    // Exit case - the equipment is not in the werewolf list
                    if (!werewolfEquipment.Contains(equipment)) return;

                    // Remove the werewolf equipment from the list
                    werewolfEquipment.Remove(equipment);
                    break;
            }
        }
    }
}
