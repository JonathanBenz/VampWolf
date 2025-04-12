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
            model.VampireSpells.AnyValueChanged += view.UpdateVampireButtonsSprites;
            model.WerewolfSpells.AnyValueChanged += view.UpdateWerewolfButtonsSprites;
            model.BloodAmountChanged += UpdateVampireCastStatus;
            model.RageAmountChanged += UpdateWerewolfCastStatus;
            model.BloodAmountChanged += view.UpdateBlood;
            model.RageAmountChanged += view.UpdateRage;

            // Set default values for the Vampire and Werewolf spells
            model.Blood = 100f;
            model.Rage = 0f;
        }

        /// <summary>
        /// Connect the view to the controller
        /// </summary>
        private void ConnectView()
        {
            // Register button listeners
            view.RegisterVampireListeners(OnVampireSpellButtonPressed);
            view.RegisterWerewolfListeners(OnWerewolfSpellButtonPressed);

            // Update the view icons
            view.UpdateVampireButtonsSprites(model.VampireSpells);
            view.UpdateWerewolfButtonsSprites(model.WerewolfSpells);
        }

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
        /// Show the spells and resource of the given character type
        /// </summary>
        private void ShowSpells(ShowSpells eventData) => view.ShowSpells(eventData.CharacterType);
    }
}
