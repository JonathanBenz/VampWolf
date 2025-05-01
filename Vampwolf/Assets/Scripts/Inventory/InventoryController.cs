using System.Collections.Generic;
using UnityEngine;

namespace Vampwolf.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        private InventoryModel model;
        private InventoryView view;

        public void Awake()
        {
            // Initialize the model and view
            model = new InventoryModel();
            view = GetComponent<InventoryView>();

            // Connect the model and the view to the controller
            ConnectModel();
            ConnectView();
        }

        private void ConnectModel()
        {

        }

        private void ConnectView()
        {

        }

        /// <summary>
        /// Update the inventory when equipments are added or removed
        /// </summary>
        private void UpdateInventory(IList<Equipment> equipments)
        {
            // Iterate through each equipment
            foreach(Equipment equipment in equipments)
            {
                // Add the equipment to the view
                view.AddEquipment(equipment);
            }
        }
    }
}
