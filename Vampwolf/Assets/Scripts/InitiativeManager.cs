using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf
{
    /// <summary>
    /// This script is responsible for keeping track of initiative and turn-based combat logic, as well as keeping track of all enemies left standing. 
    /// </summary>
    public class InitiativeManager : MonoBehaviour
    {
        [SerializeField] float delayBetweenTurns = 0.5f; // in seconds

        List<Trackable> trackables;
        Trackable currentTrackable;
        int currentIdx = -1;
        int enemiesRemaining; 

        EventBinding<TurnEndedEvent> onTurnEndedEvent;
        EventBinding<EnemyDeathEvent> onEnemyDeathEvent;

        private void Start()
        {
            CreateInitiativeOrder();
        }

        private void OnEnable()
        {
            onTurnEndedEvent = new EventBinding<TurnEndedEvent>(NewTurn);
            EventBus<TurnEndedEvent>.Register(onTurnEndedEvent);

            onEnemyDeathEvent = new EventBinding<EnemyDeathEvent>(OnEnemyDeath);
            EventBus<EnemyDeathEvent>.Register(onEnemyDeathEvent);
        }

        private void OnDisable()
        {
            EventBus<TurnEndedEvent>.Deregister(onTurnEndedEvent);
            EventBus<EnemyDeathEvent>.Deregister(onEnemyDeathEvent);
        }

        /// <summary>
        /// Find all trackable objects, store them in a list, calculate their Initiative rolls, and then sort the list to match Initiative turn order. 
        /// </summary>
        private void CreateInitiativeOrder()
        {
            Trackable[] arrOfTrackables = FindObjectsByType<Trackable>(FindObjectsSortMode.None);
            trackables = new List<Trackable>(arrOfTrackables); // Copy array into list

            foreach (Trackable t in trackables)
            {
                t.RollForInitiative();
                if (t.IsEnemy) enemiesRemaining++;
            }

            // Sort list based on the calculated Initiative order
            trackables.Sort((a, b) => b.Initiative.CompareTo(a.Initiative)); 

            // DEBUG Purposes
            int order = 0;
            foreach (Trackable t in trackables) Debug.Log($"Initiative Order --> {order++}: {t.gameObject.name}");

            NewTurn(); 
        }

        /// <summary>
        /// This method gets called each time a Trackable raises an OnTurnEnded event. 
        /// </summary>
        private void NewTurn()
        {
            currentIdx++;
            if (currentIdx >= trackables.Count) currentIdx = 0;
            currentTrackable = trackables[currentIdx];
            Debug.Log($"It is currently {currentTrackable.gameObject.name}'s turn!");

            StartCoroutine(TurnDelay(delayBetweenTurns));
            currentTrackable.StartTurn();
        }

        /// <summary>
        /// When any enemy dies, this method gets called.
        /// </summary>
        private void OnEnemyDeath()
        {
            enemiesRemaining--;
            if (enemiesRemaining <= 0)
            {
                // TODO: Level win condition logic. 
                //       Bring players back to hub immediately? Or enable an exit level trigger? 
            }
        }

        /// <summary>
        /// Add a short delay between the last turn and next turn
        /// </summary>
        /// <param name="secondsDelay"></param>
        /// <returns></returns>
        IEnumerator TurnDelay(float secondsDelay)
        {
            yield return new WaitForSeconds(secondsDelay);
        }
    }
}