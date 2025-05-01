using DG.Tweening;
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
        [Header("References")]
        [SerializeField] private Image background;
        [SerializeField] private CanvasGroup outOfStockGroup;
        [SerializeField] private CanvasGroup poorGroup;
        [SerializeField] private Image itemIcon;
        [SerializeField] private Text nameText;
        [SerializeField] private Text userText;
        [SerializeField] private Text priceText;
        private Button button;

        [Header("Tweenign Variables")]
        [SerializeField] private float highlightDuration;
        [SerializeField] private Color highlightColor;
        private Color initialColor;
        private Tween highlightTween;

        public event Action<ItemButton> OnItemClicked = delegate { };

        public Item Item { get; private set; }

        public void OnDestroy()
        {
            // Kill any existing tweens
            highlightTween?.Kill();
        }

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
        /// Check if the item is available for purchase
        /// </summary>
        public void CheckAvailability(float currentGold)
        {
            // Exit case - the item is already bought
            if (Item.Bought)
            {
                // Set the button to non-interactable
                button.interactable = false;

                // Enable the out-of-stock group
                outOfStockGroup.alpha = 1f;
                outOfStockGroup.interactable = true;
                outOfStockGroup.blocksRaycasts = true;

                return;
            }

            // Exit case - the player does not have enough gold to buy the item
            if (Item.Cost > currentGold)
            {
                // Set the button to non-interactable
                button.interactable = false;

                // Enable the poor group
                poorGroup.alpha = 1f;
                poorGroup.interactable = true;
                poorGroup.blocksRaycasts = true;

                return;
            }

            // Set the button to interactable
            button.interactable = true;
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

            // Exit case - the button is not interactable
            if (!button.interactable) return;

            // Set the highlight color
            Highlight(highlightColor, highlightDuration);
        }

        /// <summary>
        /// Clear the item info on mouse exit
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            // Raise the event to clear the item info panel
            EventBus<ClearItemInfo>.Raise(new ClearItemInfo());

            // Exit case - the button is not interactable
            if (!button.interactable) return;

            // Set the initial color
            Highlight(initialColor, highlightDuration);
        }

        /// <summary>
        /// Handle tweening the highlight color
        /// </summary>
        private void Highlight(Color color, float duration)
        {
            // Kill the highlight tween if it exists
            highlightTween?.Kill();

            // Create the highlight tween
            highlightTween = background.DOColor(color, duration).SetEase(Ease.InOutSine);
        }
    }
}
