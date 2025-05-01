using Vampwolf.EventBus;
using Vampwolf.Shop;

namespace Vampwolf.Events
{
    public struct ShowShop : IEvent { }
    public struct HideShop : IEvent { }
    public struct ShowItemInfo : IEvent
    {
        public Item Item;
    }
    public struct UpdateGold : IEvent
    {
        public int CurrentGold;
    }
    public struct ClearItemInfo : IEvent { }
    public struct ShowInventory : IEvent { }
    public struct HideInventory : IEvent { }
    public struct AddItemToInventory : IEvent
    {
        public Item Item;
    }
}
