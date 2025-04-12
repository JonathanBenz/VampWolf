using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vampwolf.Spells
{
    public class SpellsView : MonoBehaviour
    {
        [Header("Spell Bars")]
        [SerializeField] private SpellBar vampireSpellBar;
        [SerializeField] private SpellBar werewolfSpellBar;

        private void Awake()
        {
            // Initialize the spell bars
            vampireSpellBar.Initialize();
            werewolfSpellBar.Initialize();
        }

        /// <summary>
        /// Register listeners to the Vampire spell buttons
        /// </summary>
        public void RegisterVampireListeners(Action<int> listener) => vampireSpellBar.RegisterListeners(listener);

        /// <summary>
        /// Register listeners to the Werewolf spell buttons
        /// </summary>
        public void RegisterWerewolfListeners(Action<int> listener) => werewolfSpellBar.RegisterListeners(listener);

        /// <summary>
        /// Update the Vampire spell icons
        /// </summary>
        public void UpdateVampireButtonsSprites(IList<Spell> spells) => vampireSpellBar.UpdateButtonSprites(spells);

        /// <summary>
        /// Update the Werewolf spell icons
        /// </summary>
        public void UpdateWerewolfButtonsSprites(IList<Spell> spells) => werewolfSpellBar.UpdateButtonSprites(spells);

        /// <summary>
        /// Update the cast status of the Vampire spells
        /// </summary>
        public void UpdateVampireCastStatus(IList<Spell> spells, float blood) => vampireSpellBar.UpdateCastStatus(spells, blood);

        /// <summary>
        /// Update the cast status of the Werewolf spells
        /// </summary>
        public void UpdateWerewolfCastStatus(IList<Spell> spells, float rage) => werewolfSpellBar.UpdateCastStatus(spells, rage);

        /// <summary>
        /// Update the Blood resource for the Vampire
        /// </summary>
        public void UpdateBlood(float blood) => vampireSpellBar.UpdateResource(blood);

        /// <summary>
        /// Update the Rage resource for the Werewolf
        /// </summary>
        public void UpdateRage(float rage) => werewolfSpellBar.UpdateResource(rage);

        /// <summary>
        /// Show the correct spell bar
        /// </summary>
        public void ShowSpells(CharacterType characterType)
        {
            switch (characterType)
            {
                case CharacterType.Vampire:
                    // Show vampire, hide werewolf
                    vampireSpellBar.SetVisibility(true);
                    werewolfSpellBar.SetVisibility(false);
                    break;

                case CharacterType.Werewolf:
                    // Show werewolf, hide vampire
                    vampireSpellBar.SetVisibility(false);
                    werewolfSpellBar.SetVisibility(true);
                    break;
            }
        }
    }
}
