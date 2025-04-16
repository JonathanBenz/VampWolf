using System.Collections.Generic;
using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Units;

namespace Vampwolf.Battles
{
    public class InitiativeView : MonoBehaviour
    {
        [SerializeField] private InitiativeCard _initiativePrefab;
        [SerializeField] private Transform _initiativeParent;
        private InitiativeCardPool cardPool;

        private List<InitiativeCard> cardList;

        private EventBinding<InitiativeRegistered> onInitiativeRegistered;
        private EventBinding<InitiativeDeregistered> onInitiativeDeregistered;
        private EventBinding<TurnStarted> onTurnStarted;
        private EventBinding<HealthChanged> onHealthChanged;

        private void Awake()
        {
            // Initialize the card pool
            cardPool = new InitiativeCardPool(_initiativePrefab, _initiativeParent);

            // Initialize the card list
            cardList = new List<InitiativeCard>();
        }

        private void OnEnable()
        {
            onInitiativeRegistered = new EventBinding<InitiativeRegistered>(CreateInitiativeCard);
            EventBus<InitiativeRegistered>.Register(onInitiativeRegistered);

            onInitiativeDeregistered = new EventBinding<InitiativeDeregistered>(RemoveInitiativeCard);
            EventBus<InitiativeDeregistered>.Register(onInitiativeDeregistered);

            onTurnStarted = new EventBinding<TurnStarted>(SelectCard);
            EventBus<TurnStarted>.Register(onTurnStarted);

            onHealthChanged = new EventBinding<HealthChanged>(ChangeCardHealth);
            EventBus<HealthChanged>.Register(onHealthChanged);
        }

        private void OnDisable()
        {
            EventBus<InitiativeRegistered>.Register(onInitiativeRegistered);
            EventBus<InitiativeDeregistered>.Register(onInitiativeDeregistered);
            EventBus<TurnStarted>.Register(onTurnStarted);
            EventBus<HealthChanged>.Register(onHealthChanged);
        }

        /// <summary>
        /// Create an initiative card for a newly registered unit
        /// </summary>
        private void CreateInitiativeCard(InitiativeRegistered eventData)
        {
            // Extract the unit
            BattleUnit registeredUnit = eventData.Unit;

            // Get a card from the pool
            InitiativeCard card = cardPool.Get();

            // Initialize the card with the unit
            card.Initialize(registeredUnit);

            // Add the card to the list
            cardList.Add(card);
        }

        /// <summary>
        /// Remove an initiative card for a newly deregistered unit
        /// </summary>
        private void RemoveInitiativeCard(InitiativeDeregistered eventData)
        {
            // Extract the unit
            BattleUnit registeredUnit = eventData.Unit;

            // Get the first card that matches the unit
            InitiativeCard unitCard = cardList.Find(card => card.Unit == registeredUnit);

            // Exit case - no card was found with the attached unit
            if (unitCard == null) return;

            // Release the card back to the pool
            cardPool.Release(unitCard);

            // Remove the card from the list
            cardList.Remove(unitCard);
        }

        /// <summary>
        /// Select the card of the unit that has started their turn
        /// </summary>
        private void SelectCard(TurnStarted eventData)
        {
            // Extract the unit
            BattleUnit registeredUnit = eventData.Unit;

            // Iterate through each card in the list
            foreach (InitiativeCard card in cardList)
            {
                // Check if the card's unit does not match the registered unit
                if (card.Unit != registeredUnit)
                {
                    // Deslect the card and skip
                    card.Deselect();
                    continue;
                }

                // Select the card
                card.Select();
            }
        }

        /// <summary>
        /// Update the health of a card when the unit's health changes
        /// </summary>
        private void ChangeCardHealth(HealthChanged eventData)
        {
            // Extract the unit
            BattleUnit registeredUnit = eventData.Unit;

            // Get the first card that matches the unit
            InitiativeCard unitCard = cardList.Find(card => card.Unit == registeredUnit);

            // Exit case - no card was found with the attached unit
            if (unitCard == null) return;

            // Update the health of the card
            unitCard.UpdateHealth(eventData.CurrentHealth);
        }
    }
}
