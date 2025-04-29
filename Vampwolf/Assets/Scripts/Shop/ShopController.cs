using System.Collections.Generic;
using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf.Shop
{
    public class ShopController : MonoBehaviour
    {
        private ShopModel model;
        private ShopView shopView;

        [SerializeField] private List<ItemData> initialItems;

        private EventBinding<ShowShop> onShowShop;
        private EventBinding<HideShop> onHideShop;

        private void Awake()
        {
            // Initialize the model and view
            model = new ShopModel();
            shopView = GetComponent<ShopView>();

            // Connect the model and the view to the controller
            ConnectModel();
            ConnectView();
        }

        private void OnEnable()
        {
            onShowShop = new EventBinding<ShowShop>(Show);
            EventBus<ShowShop>.Register(onShowShop);

            onHideShop = new EventBinding<HideShop>(Hide);
            EventBus<HideShop>.Register(onHideShop);
        }

        private void OnDisable()
        {
            EventBus<ShowShop>.Deregister(onShowShop);
            EventBus<HideShop>.Deregister(onHideShop);

            // Disconnect the model and the controller
            model.RemoveListener(UpdateItems);
        }

        /// <summary>
        /// Connect the model to the controller
        /// </summary>
        private void ConnectModel()
        {
            // Iterate through each initial item data
            foreach(ItemData data in initialItems)
            {
                // Add the item to the model
                model.Add(data);
            }

            // Update the view when the model changes
            model.AddListener(UpdateItems);
        }

        /// <summary>
        /// Connect the view to the controller
        /// </summary>
        private void ConnectView() => shopView.Initialize(model.ItemStock);

        /// <summary>
        /// Update the items in the view when the model changes
        /// </summary>
        private void UpdateItems(IList<Item> items)
        {
            // Iterate through each item in the list
            foreach (Item item in items)
            {
                // Add the item to the view
                shopView.AddItem(item);
            }
        }

        /// <summary>
        /// Show the shop view
        /// </summary>
        private void Show() => shopView.Show();

        /// <summary>
        /// Hide the shop view
        /// </summary>
        private void Hide() => shopView.Hide();
    }
}
