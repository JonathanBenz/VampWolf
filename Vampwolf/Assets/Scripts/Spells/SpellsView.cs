using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vampwolf.Spells
{
    public class SpellsView : MonoBehaviour
    {
        [Header("Canvas Groups")]
        [SerializeField] private CanvasGroup vampireSpellButtons;
        [SerializeField] private CanvasGroup werewolfSpellButtons;
        [SerializeField] private CanvasGroup vampireResource;
        [SerializeField] private CanvasGroup werewolfResource;

        [Header("Resources")]
        [SerializeField] private Image bloodFill;
        private Text bloodText;
        [SerializeField] private Image rageFill;
        private Text rageText;

        [Header("Buttons")]
        [SerializeField] private SpellButton[] vampireButtons;
        [SerializeField] private SpellButton[] werewolfButtons;

        [Header("Tweening Variables")]
        [SerializeField] private float resourceDuration;
        private Sequence resourceChangeSequence;

        public SpellButton[] VampireButtons => vampireButtons;
        public SpellButton[] WerewolfButtons => werewolfButtons;

        private void Awake()
        {
            // Iterate through each Vampire Spell button
            for (int i = 0; i < vampireButtons.Length; i++)
            {
                // Initialize the buttons with their index
                vampireButtons[i].Initialize(i);
            }

            // Iterate through each Werewolf Spell button
            for (int i = 0; i < vampireButtons.Length; i++)
            {
                // Initialize the buttons with their index
                werewolfButtons[i].Initialize(i);
            }

            // Get the fills
            bloodText = vampireResource.GetComponentInChildren<Text>();
            rageText = werewolfResource.GetComponentInChildren<Text>();
        }

        private void OnDestroy()
        {
            // Kill any sequences if they exist
            resourceChangeSequence?.Kill();
        }

        /// <summary>
        /// Update the cast status of the Vampire Spell buttons
        /// </summary>
        public void UpdateVampireCastStatus(IList<Spell> vampireSpells, float blood)
        {
            // Iterate through each button
            for(int i = 0; i < vampireButtons.Length; i++)
            {
                // Skip if the button is not within the spells list range
                if (i >= vampireSpells.Count) continue;

                // Check if the button can be cast based on the current blood and spell cost
                vampireButtons[i].CheckCanCast(vampireSpells[i].Cost <= blood);
            }
        }

        /// <summary>
        /// Update the cast status of the Werewolf Spell buttons
        /// </summary>
        public void UpdateWerewolfCastStatus(IList<Spell> werewolfSpells, float rage)
        {
            // Iterate through each button
            for (int i = 0; i < werewolfButtons.Length; i++)
            {
                // Skip if the button is not within the spells list range
                if (i >= werewolfSpells.Count) continue;

                // Check if the button can be cast based on the current rage and spell cost
                werewolfButtons[i].CheckCanCast(werewolfSpells[i].Cost <= rage);
            }
        }

        /// <summary>
        /// Update the Vampire button sprites based on the provided Vampire spells
        /// </summary>
        public void UpdateVampireButtonsSprites(IList<Spell> vampireSpells)
        {
            // Iterate through each button
            for(int i = 0; i < vampireButtons.Length; i++)
            {
                // Check if the button index is included in the spells list
                if(i < vampireSpells.Count)
                {
                    // Set the button sprite using the spell index
                    vampireButtons[i].UpdateButtonSprite(vampireSpells[i].Icon);
                } else
                {
                    // Otherwise, deactivate the button
                    vampireButtons[i].gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Update the Werewolf button sprites based on the provided Wereolf spells
        /// </summary>
        public void UpdateWerewolfButtonsSprites(IList<Spell> werewolfSpells)
        {
            // Iterate through each button
            for (int i = 0; i < werewolfButtons.Length; i++)
            {
                // Check if the button index is included in the spells list
                if (i < werewolfSpells.Count)
                {
                    // Set the button sprite using the spell index
                    werewolfButtons[i].UpdateButtonSprite(werewolfSpells[i].Icon);
                }
                else
                {
                    // Otherwise, deactivate the button
                    werewolfButtons[i].gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Update the Vampire resource
        /// </summary>
        public void UpdateBlood(float currentBlood) => UpdateResource(bloodFill, bloodText, currentBlood, resourceDuration);

        /// <summary>
        /// Update the Werewolf resource
        /// </summary>
        public void UpdateRage(float currentRage) => UpdateResource(rageFill, rageText, currentRage, resourceDuration);

        /// <summary>
        /// Update the resource fill and text
        /// </summary>
        private void UpdateResource(Image fill, Text text, float currentValue, float duration)
        {
            // Kill any existing sequence
            resourceChangeSequence?.Kill();

            // Create a new sequence
            resourceChangeSequence = DOTween.Sequence();

            // Get the previous value of the fill
            float previousValue = fill.fillAmount * 100f;

            // Calculate the fill
            float fillAmount = currentValue / 100f;
            
            // Create the fill tween
            Tween fillTween = fill.DOFillAmount(fillAmount, duration);

            // Create the number change tween
            Tween numberTween = DOVirtual.Float(previousValue, currentValue, duration, value =>
            {
                text.text = $"{Mathf.Round(value).ToString()} / 100";
            });

            // Add and join the tweens to the sequence
            resourceChangeSequence.Append(fillTween);
            resourceChangeSequence.Join(numberTween);

            // Play the sequence
            resourceChangeSequence.Play();
        }

        /// <summary>
        /// Show the spells of the currently selected character
        /// </summary
        public void ShowSpells(CharacterType characterType)
        {
            switch (characterType)
            {
                case CharacterType.Vampire:
                    werewolfSpellButtons.alpha = 0;
                    werewolfSpellButtons.interactable = false;
                    werewolfSpellButtons.blocksRaycasts = false;

                    werewolfResource.alpha = 0;
                    vampireResource.alpha = 1;

                    vampireSpellButtons.alpha = 1;
                    vampireSpellButtons.interactable = true;
                    vampireSpellButtons.blocksRaycasts = true;
                    break;

                case CharacterType.Werewolf:
                    vampireSpellButtons.alpha = 0;
                    vampireSpellButtons.interactable = false;
                    vampireSpellButtons.blocksRaycasts = false;

                    vampireResource.alpha = 0;
                    werewolfResource.alpha = 1;

                    werewolfSpellButtons.alpha = 1;
                    werewolfSpellButtons.interactable = true;
                    werewolfSpellButtons.blocksRaycasts = true;
                    break;
            }
        }
    }
}
