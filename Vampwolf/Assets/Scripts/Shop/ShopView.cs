using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf.Shop
{
    public class ShopView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ItemButton prefab;
        [SerializeField] private Transform buttonParent;
        [SerializeField] private Button exitButton;
        private CanvasGroup shopPanel;
        private ItemInfoPanel itemInfoPanel;
        private ShopDialoguePanel dialoguePanel;

        private ItemPool itemPool;
        private List<ItemButton> itemButtons;

        [Header("Tweening Variables")]
        [SerializeField] private float fadeDuration;
        private Tween fadeTween;

        private void OnDestroy()
        {
            // Kill any existing tweens
            fadeTween?.Kill();
        }

        /// <summary>
        /// Initialize the shop view
        /// </summary>
        public void Initialize(IList<Item> initialItems)
        {
            // Get components
            shopPanel = GetComponent<CanvasGroup>();
            itemInfoPanel = GetComponentInChildren<ItemInfoPanel>();
            dialoguePanel = GetComponentInChildren<ShopDialoguePanel>();

            // Initialize panels
            itemInfoPanel.Initialize();
            dialoguePanel.Initialize(); 

            // Initialize the list of item buttons
            itemButtons = new List<ItemButton>();

            // Create the item pool
            itemPool = new ItemPool(prefab, buttonParent);

            // Iterate through each initial item
            foreach (Item item in initialItems)
            {
                // Add the item to the shop stock list
                AddItem(item);
            }

            // Set the functionality for the exit button
            exitButton.onClick.AddListener(() =>
            {
                EventBus<HideShop>.Raise(new HideShop());
            });
        }

        /// <summary>
        /// Add an item to the shop stock list
        /// </summary>
        public void AddItem(Item item)
        {
            // Create a default case of whether or not item already exists as a button
            bool itemExists = false;

            // Iterate through each existing button
            foreach (ItemButton existingButton in itemButtons)
            {
                // Skip if the button's item does not match the item to add
                if (existingButton.Item != item) continue;

                // Notify that the item exists
                itemExists = true;
            }

            // Exit case - the item already exists in the shop
            if (itemExists) return;

            // Get an item button from the pool
            ItemButton button = itemPool.Get();

            // Initialize the item button using the item
            button.Initialize(item);

            // Add the item button to the list
            itemButtons.Add(button);
        }

        /// <summary>
        /// Remove an item from the shop stock list
        /// </summary>
        public void RemoveItem(Item item)
        {
            // Create an initial item button
            ItemButton button = null;

            foreach(ItemButton itemButton in itemButtons)
            {
                // Skip if the item button does not contain the item to remove
                if (itemButton.Item != item) continue;

                // Set the item button and break
                button = itemButton;
                break;
            }

            // Exit case - no item button was found with the attached item
            if (button == null) return;

            // Remove the item button from the list
            itemButtons.Remove(button);

            // Release the item button back to the pool
            itemPool.Release(button);
        }

        /// <summary>
        /// Show the shop
        /// </summary>
        public void Show()
        {
            // Fade in the shop
            Fade(1f, fadeDuration, () =>
            {
                // Set non-interactable and to not block raycasts
                shopPanel.interactable = true;
                shopPanel.blocksRaycasts = true;

                // Create a piece of dialogue
                dialoguePanel.CreateWelcomeDialogue();
            });
        }

        /// <summary>
        /// Hide the shop
        /// </summary>
        public void Hide()
        {
            // Fade out the shop
            Fade(0f, fadeDuration, () =>
            {
                // Set non-interactable and to not block raycasts
                shopPanel.interactable = false;
                shopPanel.blocksRaycasts = false;

                // Clear the dialogue
                dialoguePanel.ClearDialogue();
            });
        }

        /// <summary>
        /// Handle fading for the shop panel
        /// </summary>
        private void Fade(float endValue, float duration, TweenCallback onComplete = null)
        {
            // Kill the fade tween if it exists
            fadeTween?.Kill();

            // Create a new fade tween
            fadeTween = shopPanel.DOFade(endValue, duration);

            // Exit case - no completion action was given
            if (onComplete == null) return;

            // Set the completion action
            fadeTween.OnComplete(onComplete);
        }
    }
}
