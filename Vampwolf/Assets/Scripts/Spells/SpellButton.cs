using System;
using UnityEngine;
using UnityEngine.UI;

namespace Vampwolf.Spells
{
    public class SpellButton : MonoBehaviour
    {
        private int index;
        [SerializeField] private Image disabledOverlay;
        [SerializeField] private Image spellIcon;
        private bool canCast;

        public event Action<int> OnButtonPressed = delegate { };

        private void Start()
        {
            // Get the Button and add an event listener to it
            Button button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                // Exit case - if the spell cannot be cast
                if (!canCast) return;

                OnButtonPressed.Invoke(index);
            });
        }

        /// <summary>
        /// Register a listener to the button pressed event
        /// </summary>
        public void RegisterListener(Action<int> listener) => OnButtonPressed += listener;

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
        /// Check if the Spell can be cast
        /// </summary>
        public void CheckCanCast(bool canCast)
        {
            disabledOverlay.gameObject.SetActive(!canCast);
            this.canCast = canCast;
        }
    }
}
