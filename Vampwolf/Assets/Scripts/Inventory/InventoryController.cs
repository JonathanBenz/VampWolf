using System.Collections.Generic;
using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        private InventoryModel model;
        private InventoryView view;

        private EventBinding<ShowInventory> onShowInventory;
        private EventBinding<HideInventory> onHideInventory;
        private EventBinding<AddItemToInventory> onAddItemToInventory;

        public InventoryModel Model => model;

        public void Awake()
        {
            // Initialize the model and view
            model = new InventoryModel();
            view = GetComponent<InventoryView>();

            // Connect the model and the view to the controller
            ConnectModel();
            ConnectView();
        }

        private void OnEnable()
        {
            onShowInventory = new EventBinding<ShowInventory>(Show);
            EventBus<ShowInventory>.Register(onShowInventory);

            onHideInventory = new EventBinding<HideInventory>(Hide);
            EventBus<HideInventory>.Register(onHideInventory);

            onAddItemToInventory = new EventBinding<AddItemToInventory>(AddItem);
            EventBus<AddItemToInventory>.Register(onAddItemToInventory);
        }

        private void OnDisable()
        {
            EventBus<ShowInventory>.Deregister(onShowInventory);
            EventBus<HideInventory>.Deregister(onHideInventory);
            EventBus<AddItemToInventory>.Deregister(onAddItemToInventory);
        }

        private void OnDestroy()
        {
            model.RemoveListener(UpdateInventory);
        }

        /// <summary>
        /// Connect the model to the controller
        /// </summary>
        private void ConnectModel()
        {
            model.AddListener(UpdateInventory);
        }

        /// <summary>
        /// Connect the view to the controller
        /// </summary>
        private void ConnectView()
        {
            // Initialize the view
            view.Initialize(this);
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

        /// <summary>
        /// Add an item to the inventory
        /// </summary>
        private void AddItem(AddItemToInventory eventData) => model.Add(eventData.Item);

        /// <summary>
        /// Show the inventory screen
        /// </summary>
        private void Show() => view.Show();

        /// <summary>
        /// Hide the inventory screen
        /// </summary>
        private void Hide() => view.Hide();
    }
}
