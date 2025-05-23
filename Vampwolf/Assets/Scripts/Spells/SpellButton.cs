using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Vampwolf.Spells
{
    public class SpellButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private int index;
        [SerializeField] private Image disabledOverlay;
        [SerializeField] private Image spellIcon;
        private bool canCast;
        private bool active;

        public bool Active { get => active; set => active = value; }

        public event Action<int> OnButtonPressed = delegate { };
        public event Action<int> OnPointerEntered = delegate { };
        public event Action OnPointerExited = delegate { };

        private void Start()
        {
            // Get the Button and add an event listener to it
            Button button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                // Exit case - if the button is not active
                if (!active) return;

                // Exit case - if the spell cannot be cast
                if (!canCast) return;

                OnButtonPressed.Invoke(index);
            });
        }

        /// <summary>
        /// Register a listener to the button pressed event
        /// </summary>
        public void RegisterClickListener(Action<int> listener) => OnButtonPressed += listener;

        /// <summary>
        /// Register a listener to the pointer entered event
        /// </summary>
        public void RegisterEnterListener(Action<int> listener) => OnPointerEntered += listener;

        /// <summary>
        /// Register a listener to the pointer exited event
        /// </summary>
        public void RegisterExitListener(Action listener) => OnPointerExited += listener;

        /// <summary>
        /// Initialize the Spell Button
        /// </summary>
        public void Initialize(int index)
        {
            this.index = index;
        }

        /// <summary>
        /// Change the icon of the button
        /// </summary>
        public void UpdateButtonSprite(Sprite newIcon) => spellIcon.sprite = newIcon;

        /// <summary>
        /// Show the Spell Icon
        /// </summary>
        public void Show() => spellIcon.DOFade(1f, 0f);

        /// <summary>
        /// Hide the Spell Icon
        /// </summary>
        public void Hide() => spellIcon.DOFade(0f, 0f);

        /// <summary>
        /// Check if the Spell can be cast
        /// </summary>
        public void CheckCanCast(bool canCast)
        {
            disabledOverlay.gameObject.SetActive(!canCast);
            this.canCast = canCast;
        }

        /// <summary>
        /// Handle the pointer enter event
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Exit case - not active
            if (!active) return;

            OnPointerEntered.Invoke(index);
        }

        /// <summary>
        /// Handle the pointer exit event
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            // Exit case - not active
            if (!active) return;

            OnPointerExited.Invoke();
        }
    }
}
