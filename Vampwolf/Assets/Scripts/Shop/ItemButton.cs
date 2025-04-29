using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf.Shop
{
    public class ItemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Button button;
        [SerializeField] private CanvasGroup outOfStockGroup;
        [SerializeField] private Image itemIcon;
        [SerializeField] private Text nameText;
        [SerializeField] private Text userText;
        [SerializeField] private Text priceText;

        public event Action<ItemButton> OnItemClicked = delegate { };

        public Item Item { get; private set; }

        /// <summary>
        /// Initialize the item button
        /// </summary>
        public void Initialize(Item item)
        {
            // Set the item
            Item = item;

            // Set the item icon
            itemIcon.sprite = Item.Icon;

            // Set the name text
            nameText.text = Item.Name;
            gameObject.name = Item.Name;

            // Set the user text
            switch (Item.User)
            {
                case UserType.Vampire:
                    userText.text = "Vampire";
                    break;
                case UserType.Werewolf:
                    userText.text = "Werewolf";
                    break;
            }

            // Build the price text and set it
            StringBuilder priceBuilder = new StringBuilder();
            priceBuilder.Append(Item.Cost);
            priceBuilder.Append(" G");
            priceText.text = priceBuilder.ToString();

            // Get the button component and add an on-click listener
            button = GetComponent<Button>();
            button.onClick.AddListener(Buy);
        }

        /// <summary>
        /// Buy the item
        /// </summary>
        private void Buy()
        {
            // Buy the item
            Item.Buy();

            // Disable the button
            button.interactable = false;

            // Enable the out-of-stock group
            outOfStockGroup.alpha = 1f;
            outOfStockGroup.interactable = true;
            outOfStockGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// Show the item info on mouse enter
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Raise the event to display the item info
            EventBus<ShowItemInfo>.Raise(new ShowItemInfo()
            {
                Item = Item
            });
        }

        /// <summary>
        /// Clear the item info on mouse exit
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            // Raise the event to clear the item info panel
            EventBus<ClearItemInfo>.Raise(new ClearItemInfo());
        }
    }
}
