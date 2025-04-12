using System.Collections.Generic;
using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf.Spells
{
    public class SpellsController : MonoBehaviour
    {
        private SpellsModel model;
        private SpellsView view;

        [SerializeField] private List<SpellData> initialSpells = new List<SpellData>();

        private EventBinding<ShowSpells> onShowSpells;

        private void Awake()
        {
            // Create the model
            model = new SpellsModel();

            // Iterate through the initial spells
            foreach (SpellData data in initialSpells)
            {
                // Add the spells to the model
                model.Add(new Spell(data));
            }

            // Get the view
            view = GetComponent<SpellsView>();

            // Connect the view and the model to the controller
            ConnectModel();
            ConnectView();

            // Show the vampire spells by default
            ShowSpells(new ShowSpells()
            {
                CharacterType = CharacterType.Vampire
            });
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
        /// Connect the model to the controller
        /// </summary>
        private void ConnectModel()
        {
            model.VampireSpells.AnyValueChanged += UpdateVampireButtonSprites;
            model.WerewolfSpells.AnyValueChanged += UpdateWerewolfButtonSprites;
            model.BloodAmountChanged += UpdateVampireCastStatus;
            model.RageAmountChanged += UpdateWerewolfCastStatus;
            model.BloodAmountChanged += UpdateVampireResource;
            model.RageAmountChanged += UpdateWerewolfResource;

            // Set default values for the Vampire and Werewolf spells
            model.Blood = 100f;
            model.Rage = 0f;
        }

        /// <summary>
        /// Connect the view to the controller
        /// </summary>
        private void ConnectView()
        {
            // Iterate through each Vampire Spell button in the view
            for (int i = 0; i < view.VampireButtons.Length; i++)
            {
                // Register the button press event
                view.VampireButtons[i].RegisterListener(OnVampireSpellButtonPressed);
            }

            // Iterate through each Werewolf Spell button in the view
            for (int i = 0; i < view.VampireButtons.Length; i++)
            {
                // Register the button press event
                view.WerewolfButtons[i].RegisterListener(OnWerewolfSpellButtonPressed);
            }

            // Update the view icons
            view.UpdateVampireButtonsSprites(model.VampireSpells);
            view.UpdateWerewolfButtonsSprites(model.WerewolfSpells);
        }

        /// <summary>
        /// Update the Vampire buttons sprites within the view
        /// </summary>
        private void UpdateVampireButtonSprites(IList<Spell> spells) => view.UpdateVampireButtonsSprites(spells);

        /// <summary>
        /// Update the Werewolf buttons sprites within the view
        /// </summary>
        private void UpdateWerewolfButtonSprites(IList<Spell> spells) => view.UpdateWerewolfButtonsSprites(spells);

        /// <summary>
        /// Cast the Vampire spell at the given button index
        /// </summary>
        private void OnVampireSpellButtonPressed(int index) => model.VampireSpells[index].Cast(model);

        /// <summary>
        /// Cast the Werewolf spell at the given button index
        /// </summary>
        private void OnWerewolfSpellButtonPressed(int index) => model.WerewolfSpells[index].Cast(model);

        /// <summary>
        /// Update the cast status of the Vampire Spell buttons
        /// </summary>
        private void UpdateVampireCastStatus(float blood) => view.UpdateVampireCastStatus(model.VampireSpells, blood);

        /// <summary>
        /// Update the cast status of the Werewolf Spell buttons
        /// </summary>
        private void UpdateWerewolfCastStatus(float rage) => view.UpdateWerewolfCastStatus(model.WerewolfSpells, rage);

        /// <summary>
        /// Update the Vampire resource
        /// </summary>
        private void UpdateVampireResource(float blood) => view.UpdateBlood(blood);

        /// <summary>
        /// Update the Werewolf resource
        /// </summary>
        private void UpdateWerewolfResource(float rage) => view.UpdateRage(rage);

        /// <summary>
        /// Show the spells and resource of the given character type
        /// </summary>
        private void ShowSpells(ShowSpells eventData) => view.ShowSpells(eventData.CharacterType);
    }
}
