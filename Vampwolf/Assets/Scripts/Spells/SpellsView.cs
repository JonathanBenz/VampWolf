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
        [SerializeField] private SpellTooltip tooltip;

        public SpellTooltip Tooltip => tooltip;

        private void Awake()
        {
            // Initialize the spell bars and tooltip
            vampireSpellBar.Initialize();
            werewolfSpellBar.Initialize();
            tooltip.Initialize();
        }

        /// <summary>
        /// Register click listeners to the Vampire spell buttons
        /// </summary>
        public void RegisterVampireClickListeners(Action<int> listener) => vampireSpellBar.RegisterClickListeners(listener);

        /// <summary>
        /// Register enter listeners to the Vampire spell buttons
        /// </summary>
        public void RegisterVampireEnterListeners(Action<int> listener) => vampireSpellBar.RegisterEnterListeners(listener);

        /// <summary>
        /// Register exit listeners to the Vampire spell buttons
        /// </summary>
        public void RegisterVampireExitListeners(Action listener) => vampireSpellBar.RegisterExitListeners(listener);

        /// <summary>
        /// Register click listeners to the Werewolf spell buttons
        /// </summary>
        public void RegisterWerewolfClickListeners(Action<int> listener) => werewolfSpellBar.RegisterClickListeners(listener);

        /// <summary>
        /// Register enter listeners to the Werewolf spell buttons
        /// </summary>
        public void RegisterWerewolfEnterListeners(Action<int> listener) => werewolfSpellBar.RegisterEnterListeners(listener);

        /// <summary>
        /// Register exit listeners to the Werewolf spell buttons
        /// </summary>
        public void RegisterWerewolfExitListeners(Action listener) => werewolfSpellBar.RegisterExitListeners(listener);

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
        /// Show the Vampire Tooltip
        /// </summary>
        public void ShowVampireTooltip(int index, Spell spell)
        {
            // Set the tooltip properties
            tooltip.Name = spell.Name;
            tooltip.Description = spell.Description;

            string costColor = spell.Cost > 0 ? "<color=#B22222>-</color>" : "<color=#40826D>+</color>";
            string costString = costColor.Insert(16, Mathf.Abs(spell.Cost).ToString());

            tooltip.Cost = $"Cost: {costString}";
            tooltip.Range = $"Range: {spell.Range}";

            // Move the tool tip
            Vector2 position = vampireSpellBar.SpellButtons[index].transform.position;
            tooltip.MoveTooltip(position);

            // Show the tooltip
            tooltip.Show();
        }

        /// <summary>
        /// Show the Werewolf Tooltip
        /// </summary>
        public void ShowWerewolfTooltip(int index, Spell spell)
        {
            // Set the tooltip properties
            tooltip.Name = spell.Name;
            tooltip.Description = spell.Description;

            string costColor = spell.Cost > 0 ? "<color=#B22222>-</color>" : "<color=#40826D>+</color>";
            string costString = costColor.Insert(16, Mathf.Abs(spell.Cost).ToString());

            tooltip.Cost = $"Cost: {costString}";
            tooltip.Range = $"Range: {spell.Range}";

            // Move the tool tip
            Vector2 position = werewolfSpellBar.SpellButtons[index].transform.position;
            tooltip.MoveTooltip(position);

            // Show the tooltip
            tooltip.Show();
        }

        /// <summary>
        /// Hide the tooltip
        /// </summary>
        public void HideTooltip() => tooltip.Hide();

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

        /// <summary>
        /// Hide both spell bars
        /// </summary>
        public void HideSpells()
        {
            vampireSpellBar.SetVisibility(false);
            werewolfSpellBar.SetVisibility(false);
        }

        /// <summary>
        /// Disable both spell bars
        /// </summary>
        public void DisableSpells()
        {
            vampireSpellBar.Disable();
            werewolfSpellBar.Disable();
        }
    }
}
