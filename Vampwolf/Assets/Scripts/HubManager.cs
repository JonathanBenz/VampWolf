using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf
{
    public class HubManager : MonoBehaviour
    {
        void Start()
        {
            // Ensure the shop and inventory screens are hidden
            EventBus<HideShop>.Raise(new HideShop());
            EventBus<HideInventory>.Raise(new HideInventory());
        }

        /// <summary>
        /// Open the shop
        /// </summary>
        public void OpenShop() => EventBus<ShowShop>.Raise(new ShowShop());

        /// <summary>
        /// Open the inventory
        /// </summary>
        public void OpenInventory() => EventBus<ShowInventory>.Raise(new ShowInventory());
    }
}
