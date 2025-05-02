using System.Collections.Generic;
using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Units;

namespace Vampwolf.Spells
{
    public class SpellsController : MonoBehaviour
    {
        private SpellsModel model;
        private SpellsView view;
        private BattleUnit castingUnit;

        [SerializeField] private List<SpellData> initialSpells = new List<SpellData>();

        private EventBinding<ShowSpells> onShowSpells;
        private EventBinding<HideSpells> onHideSpells;
        private EventBinding<DisableSpells> onDisableSpells;

        private void Awake()
        {
            // Create the model
            model = new SpellsModel();

            // Iterate through the initial spells
            foreach (SpellData data in initialSpells)
            {
                // Add the spells to the model
                model.Add(new Spell(model, data));
            }

            // Get the view
            view = GetComponent<SpellsView>();

            // Connect the view and the model to the controller
            ConnectModel();
            ConnectView();
        }

        private void OnEnable()
        {
            onShowSpells = new EventBinding<ShowSpells>(ShowSpells);
            EventBus<ShowSpells>.Register(onShowSpells);

            onHideSpells = new EventBinding<HideSpells>(HideSpells);
            EventBus<HideSpells>.Register(onHideSpells);

            onDisableSpells = new EventBinding<DisableSpells>(DisableSpells);
            EventBus<DisableSpells>.Register(onDisableSpells);
        }

        private void OnDisable()
        {
            EventBus<ShowSpells>.Deregister(onShowSpells);
            EventBus<HideSpells>.Deregister(onHideSpells);
            EventBus<DisableSpells>.Deregister(onDisableSpells);
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
            view.RegisterVampireClickListeners(OnVampireSpellButtonPressed);
            view.RegisterWerewolfClickListeners(OnWerewolfSpellButtonPressed);
            view.RegisterVampireEnterListeners(OnVampireSpellButtonEntered);
            view.RegisterWerewolfEnterListeners(OnWerewolfSpellButtonEntered);
            view.RegisterVampireExitListeners(OnSpellButtonExited);
            view.RegisterWerewolfExitListeners(OnSpellButtonExited);

            // Update the view icons
            view.UpdateVampireButtonsSprites(model.VampireSpells);
            view.UpdateWerewolfButtonsSprites(model.WerewolfSpells);
        }

        /// <summary>
        /// Cast the Vampire spell at the given button index
        /// </summary>
        private void OnVampireSpellButtonPressed(int index)
        {
            // Set the spell selection mode using the spell at the given index
            EventBus<SetSpellSelectionMode>.Raise(new SetSpellSelectionMode()
            {
                Spell = model.VampireSpells[index],
                GridPosition = castingUnit.GridPosition,
            });
        }

        /// <summary>
        /// Cast the Werewolf spell at the given button index
        /// </summary>
        private void OnWerewolfSpellButtonPressed(int index)
        {
            // Set the spell selection mode using the spell at the given index
            EventBus<SetSpellSelectionMode>.Raise(new SetSpellSelectionMode()
            {
                Spell = model.WerewolfSpells[index],
                GridPosition = castingUnit.GridPosition,
            });
        }

        /// <summary>
        /// Show the tooltip for the Vampire spell button at the given index
        /// </summary>
        private void OnVampireSpellButtonEntered(int index) => view.ShowVampireTooltip(index, model.VampireSpells[index]);

        /// <summary>
        /// Show the tooltip for the Werewolf spell button at the given index
        /// </summary>
        private void OnWerewolfSpellButtonEntered(int index) => view.ShowWerewolfTooltip(index, model.WerewolfSpells[index]);

        /// <summary>
        /// Hide the tooltip for the Vampire and Werewolf spell buttons
        /// </summary>
        private void OnSpellButtonExited() => view.HideTooltip();

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
        private void ShowSpells(ShowSpells eventData)
        {
            // Set data
            view.ShowSpells(eventData.CharacterType);
            castingUnit = eventData.CastingUnit;
        }

        /// <summary>
        /// Hide the spells and resources
        /// </summary>
        private void HideSpells() => view.HideSpells();

        /// <summary>
        /// Disable the spells to prevent them from being used
        /// </summary>
        private void DisableSpells() => view.DisableSpells();

        /// <summary>
        /// Cast the enemy spell at the given index
        /// </summary>
        /// <param name="index">Index of the spell to cast.</param>
        /// <param name="gridPos">Current position of the enemy Unit.</param>
        public void EnemySpellSelect(int index, BattleUnit castingUnit)
        {
            // Set the spell selection mode using the spell at the given index
            EventBus<SetSpellSelectionMode>.Raise(new SetSpellSelectionMode()
            {
                Spell = model.EnemySpells[index],
                GridPosition = castingUnit.GridPosition,
            });
        }

        public int GetEnemySpellAttackRange(int index)
        {
            return model.EnemySpells[index].Range;
        }
    }
}
