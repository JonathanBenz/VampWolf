using UnityEngine;
using UnityEngine.UI;
using Vampwolf.EventBus;

namespace Vampwolf
{
    public class EndTurnButton : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        private EventBinding<SetEndTurnButton> onSetEndTurnButton;

        private void Awake()
        {
            // Get components
            canvasGroup = GetComponent<CanvasGroup>();

            // Set on-click listeners
            Button button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnEnable()
        {
            onSetEndTurnButton = new EventBinding<SetEndTurnButton>(SetInteractable);
            EventBus<SetEndTurnButton>.Register(onSetEndTurnButton);
        }

        private void OnDisable()
        {
            EventBus<SetEndTurnButton>.Deregister(onSetEndTurnButton);
        }

        /// <summary>
        /// Set whether or not the button can be interacted with
        /// </summary>
        private void SetInteractable(SetEndTurnButton eventData)
        {
            canvasGroup.alpha = eventData.Active ? 1f : 0f;
            canvasGroup.interactable = eventData.Active;
            canvasGroup.blocksRaycasts = eventData.Active;
        }

        /// <summary>
        /// Skip the current turn on click
        /// </summary>
        private void OnClick() => EventBus<SkipTurn>.Raise(new SkipTurn());
    }
}
