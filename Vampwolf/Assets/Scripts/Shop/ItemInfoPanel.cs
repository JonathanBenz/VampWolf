using UnityEngine;
using UnityEngine.UI;
using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Utilities.Typewriter;

namespace Vampwolf.Shop
{
    public class ItemInfoPanel : MonoBehaviour
    {
        [SerializeField] private Text nameText;
        [SerializeField] private Text infoText;
        [SerializeField] private Text flavorText;
        private Typewriter typewriter;
        private Typewriter flavorTypewriter;

        [Header("Fields")]
        [SerializeField] private float characterSpeed;

        private EventBinding<ShowItemInfo> onDisplayItemInfo;
        private EventBinding<ClearItemInfo> onHideItemInfo;

        private void OnEnable()
        {
            onDisplayItemInfo = new EventBinding<ShowItemInfo>(ShowItemInfo);
            EventBus<ShowItemInfo>.Register(onDisplayItemInfo);

            onHideItemInfo = new EventBinding<ClearItemInfo>(ClearItemInfo);
            EventBus<ClearItemInfo>.Register(onHideItemInfo);
        }

        private void OnDisable()
        {
            EventBus<ShowItemInfo>.Deregister(onDisplayItemInfo);
            EventBus<ClearItemInfo>.Deregister(onHideItemInfo);
        }

        /// <summary>
        /// Initialize the item info panel
        /// </summary>
        public void Initialize()
        {
            // Create the typewriter
            typewriter = new Typewriter(infoText, characterSpeed);
            flavorTypewriter = new Typewriter(flavorText, characterSpeed);
        }

        /// <summary>
        /// Show information according to an item
        /// </summary>
        private void ShowItemInfo(ShowItemInfo eventData)
        {
            // Set the name
            nameText.text = eventData.Item.Name;

            // Write out the description
            typewriter.Write(eventData.Item.Description, () =>
            {
                // Write out the flavor text when finished
                flavorTypewriter.Write(eventData.Item.Flavor);
            });
        }

        /// <summary>
        /// Clear the item info panel
        /// </summary>
        private void ClearItemInfo()
        {
            // Clear the name text
            nameText.text = string.Empty;

            // Clear the typewriters
            typewriter.Clear();
            flavorTypewriter.Clear();
        }
    }
}
