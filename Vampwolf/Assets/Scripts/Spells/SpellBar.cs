using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vampwolf.Spells
{
    public class SpellBar : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpellButton[] spellButtons;
        [SerializeField] private Image resourceFill;
        [SerializeField] private Text resourceText;
        [SerializeField] private CanvasGroup spellBarGroup;

        [Header("Tweening Settings")]
        [SerializeField] private float resourceTweenDuration = 0.5f;
        private Sequence resourceChangeSequence;

        public SpellButton[] SpellButtons => spellButtons;

        /// <summary>
        /// Initializes the spell bar
        /// </summary>
        public void Initialize()
        {
            // Iterate through each spell button
            for (int i = 0; i < spellButtons.Length; i++)
            {
                // Initialize the buttons with their index
                spellButtons[i].Initialize(i);
            }
        }

        /// <summary>
        /// Register listeners to the click event for each button
        /// </summary>
        public void RegisterClickListeners(Action<int> listener)
        {
            // Iterate through each spell button
            for (int i = 0; i < spellButtons.Length; i++)
            {
                // Register the listener to the button pressed event
                spellButtons[i].RegisterClickListener(listener);
            }
        }

        /// <summary>
        /// Register listeners to the pointer entered event for each button
        /// </summary>
        public void RegisterEnterListeners(Action<int> listener)
        {
            // Iterate through each spell button
            for (int i = 0; i < spellButtons.Length; i++)
            {
                // Register the listener to the pointer entered event
                spellButtons[i].RegisterEnterListener(listener);
            }
        }

        /// <summary>
        /// Register listeners to the pointer exited event for each button
        /// </summary>
        public void RegisterExitListeners(Action listener)
        {
            // Iterate through each spell button
            for (int i = 0; i < spellButtons.Length; i++)
            {
                // Register the listener to the pointer exited event
                spellButtons[i].RegisterExitListener(listener);
            }
        }

        /// <summary>
        /// Updates the sprites for the spell buttons using the provided list of spells
        /// </summary>
        public void UpdateButtonSprites(IList<Spell> spells)
        {
            // Iterate through each spell button
            for (int i = 0; i < spellButtons.Length; i++)
            {
                // Check if the button index is within the range of the spells list
                if (i < spells.Count)
                {
                    // Update the button icon and ensure it's shown
                    spellButtons[i].Active = true;
                    spellButtons[i].UpdateButtonSprite(spells[i].Icon);
                    spellButtons[i].Show();
                }
                else
                {
                    // Hide the spell icon
                    spellButtons[i].Active = false;
                    spellButtons[i].Hide();
                }
            }
        }

        /// <summary>
        /// Updates the cast status for each button based on available resource
        /// </summary>
        public void UpdateCastStatus(IList<Spell> spells, float resourceAmount)
        {
            // Iterate through each spell button
            for (int i = 0; i < spellButtons.Length; i++)
            {
                // Check if the button index is within the range of the spells list
                if (i < spells.Count)
                {
                    // Check if the spell can be cast based on the current resource and spell cost
                    bool canCast = spells[i].Cost <= resourceAmount;
                    spellButtons[i].CheckCanCast(canCast);
                }
            }
        }

        /// <summary>
        /// Updates the resource fill and text
        /// </summary>
        public void UpdateResource(float currentResource)
        {
            // Kill any existing tween
            resourceChangeSequence?.Kill();

            // Create a new sequence
            resourceChangeSequence = DOTween.Sequence();

            // Calculate the previous value and the new fill
            float previousValue = resourceFill.fillAmount * 100f;
            float newFill = currentResource / 100f;

            // Animate the fill amount
            Tween fillTween = resourceFill.DOFillAmount(newFill, resourceTweenDuration);

            // Animate the text value
            Tween numberTween = DOVirtual.Float(previousValue, currentResource, resourceTweenDuration, value =>
            {
                resourceText.text = $"{Mathf.Round(value)} / 100";
            });

            // Add tweens to the sequence and play it
            resourceChangeSequence.Append(fillTween);
            resourceChangeSequence.Join(numberTween);
            resourceChangeSequence.Play();
        }

        /// <summary>
        /// Toggles the visibility of the entire spell bar
        /// </summary>
        public void SetVisibility(bool visible)
        {
            spellBarGroup.alpha = visible ? 1f : 0f;
            spellBarGroup.interactable = visible;
            spellBarGroup.blocksRaycasts = visible;
        }

        /// <summary>
        /// Disable the spellbar
        /// </summary>
        public void Disable() => spellBarGroup.interactable = false;
    }
}
