using System.Collections.Generic;
using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf.Spells
{
    public class SpellsView : MonoBehaviour
    {
        [Header("Canvas Groups")]
        [SerializeField] private CanvasGroup vampireCanvasGroup;
        [SerializeField] private CanvasGroup werewolfCanvasGroup;

        [Header("Buttons")]
        [SerializeField] private SpellButton[] vampireButtons;
        [SerializeField] private SpellButton[] werewolfButtons;

        public SpellButton[] VampireButtons => vampireButtons;
        public SpellButton[] WerewolfButtons => werewolfButtons;

        private EventBinding<ShowSpells> onShowSpells;

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
        }

        private void OnEnable()
        {
            onShowSpells = new EventBinding<ShowSpells>(ShowSpells);
            EventBus<ShowSpells>.Register(onShowSpells);
        }

        private void OnDisable()
        {
            EventBus<ShowSpells>.Deregister(onShowSpells);
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
                vampireButtons[i].CheckCanCast(blood <= vampireSpells[i].Cost);
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
                werewolfButtons[i].CheckCanCast(rage <= werewolfSpells[i].Cost);
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
        /// Show the spells of the currently selected character
        /// </summary
        private void ShowSpells(ShowSpells eventData)
        {
            switch (eventData.CharacterType)
            {
                case CharacterType.Vampire:
                    werewolfCanvasGroup.alpha = 0;
                    werewolfCanvasGroup.interactable = false;
                    werewolfCanvasGroup.blocksRaycasts = false;

                    vampireCanvasGroup.alpha = 1;
                    vampireCanvasGroup.interactable = true;
                    vampireCanvasGroup.blocksRaycasts = true;
                    break;

                case CharacterType.Werewolf:
                    vampireCanvasGroup.alpha = 0;
                    vampireCanvasGroup.interactable = false;
                    vampireCanvasGroup.blocksRaycasts = false;

                    werewolfCanvasGroup.alpha = 1;
                    werewolfCanvasGroup.interactable = true;
                    werewolfCanvasGroup.blocksRaycasts = true;
                    break;
            }
        }
    }
}
