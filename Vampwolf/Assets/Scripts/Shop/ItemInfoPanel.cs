using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf.Shop
{
    public class ItemInfoPanel : MonoBehaviour
    {
        private Text text;

        [Header("Fields")]
        [SerializeField] private float characterSpeed;
        private Tween typeTween;

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

        private void OnDestroy()
        {
            // Kill any existing tweens
            typeTween?.Kill();
        }

        /// <summary>
        /// Initialize the item info panel
        /// </summary>
        public void Initialize()
        {
            // Get components
            text = GetComponentInChildren<Text>();
        }

        /// <summary>
        /// Show information according to an item
        /// </summary>
        private void ShowItemInfo(ShowItemInfo eventData)
        {
            // Kill the type sequence if it exists
            typeTween?.Kill();

            // Clear the text
            text.text = string.Empty;

            // Compute the total duration
            string description = eventData.Item.Description;
            float totalDuration = description.Length * characterSpeed;

            // Set the type tween
            typeTween = text.DOText(description, totalDuration, true).SetEase(Ease.Linear);
        }

        /// <summary>
        /// Clear the item info panel
        /// </summary>
        private void ClearItemInfo()
        {
            // Kill the type sequence if it exists
            typeTween?.Kill();

            // Clear the text
            text.text = string.Empty;
        }
    }
}
