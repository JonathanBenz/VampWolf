using Cysharp.Threading.Tasks;
using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Grid;
using Vampwolf.Spells;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;

namespace Vampwolf.Units
{
    public class Enemy : BattleUnit
    {
        private Vampire vampire;
        private Werewolf werewolf;
        private Vector3 targetPos = Vector3.positiveInfinity;
        private GridSelector gridSelector;
        private SpellsController spellController;

        private EventBinding<CommandProcessed> onCommandProcessed;
        private UniTaskCompletionSource commandCompletionSource;

        private Dictionary<BattleUnit, int> aggressiveThreats;
        private Dictionary<BattleUnit, int> ignoredTargets;

        private const int Melee = 0;
        private const int Ranged = 1;

        private void Start()
        {
            vampire = FindObjectOfType<Vampire>();
            werewolf = FindObjectOfType<Werewolf>();
            gridSelector = FindObjectOfType<GridSelector>();
            spellController = FindObjectOfType<SpellsController>();

            // Initialize the aggro dictionary
            aggressiveThreats = new Dictionary<BattleUnit, int>();

            // Initialize the ignored targets dictionary
            ignoredTargets = new Dictionary<BattleUnit, int>();
        }

        private void OnEnable()
        {
            onCommandProcessed = new EventBinding<CommandProcessed>(EnemyCommandCompleted);
            EventBus<CommandProcessed>.Register(onCommandProcessed);
        }

        private void OnDisable()
        {
            EventBus<CommandProcessed>.Deregister(onCommandProcessed);
        }

        public override void AwaitCommands()
        {
            ExecuteEnemyTurn().Forget();
        }

        public override async UniTask StartTurn()
        {
            await base.StartTurn();

            Debug.Log($"It's {gameObject.name}'s turn!");

            await UniTask.CompletedTask;
        }

        public override async UniTask EndTurn()
        {
            // Tick down the duration of any priority effects
            TickPriorityDurations();

            // Clear the highlights
            EventBus<ClearHighlights>.Raise(new ClearHighlights());

            await base.EndTurn();
        }

        private async UniTask ExecuteEnemyTurn()
        {
            await MoveToTarget();
            await UniTask.Delay(200);

            await AttackIfPossible();
            await UniTask.Delay(200);

            EventBus<SkipTurn>.Raise(new SkipTurn());
        }

        protected override void OnDeath()
        {
            // Remove the unit
            EventBus<RemoveUnit>.Raise(new RemoveUnit()
            {
                Unit = this,
                IsEnemy = true
            });

            // Display unit death sprite
            spriteRenderer.sprite = statData.deathSprite;
            ringSprite.enabled = false;
        }

        /// <summary>
        /// This will command the active enemy unit to move toward the closest player
        /// </summary>
        private async UniTask MoveToTarget()
        {
            // Exit case - still has movement
            if (movementLeft <= 0) return;

            targetPos = CalculateTarget();
            if (targetPos == Vector3.negativeInfinity) return; // Exit case - no valid positions were found when calculating. 

            // If already 1 tile away from target, there is no need to move
            if (Mathf.Abs((transform.position - targetPos).magnitude) < 1.5f) return;

            // Enable the grid selector
            EventBus<SetGridSelector>.Raise(new SetGridSelector()
            {
                Active = true,
                isEnemyTurn = true
            });

            // Set the movement selection mode
            EventBus<SetMovementSelectionMode>.Raise(new SetMovementSelectionMode()
            {
                GridPosition = gridPosition,
                Range = movementLeft,
                HighlightType = Grid.HighlightType.Move
            });

            commandCompletionSource = new UniTaskCompletionSource();

            // Calculate the path to the player and move towards there
            gridSelector.EnemyMovementCellSelect(GridPosition, targetPos);

            await commandCompletionSource.Task;
        }

        /// <summary>
        /// This will command the enemy to attack if an enemy is within their attack's range
        /// </summary>
        /// <returns></returns>
        private async UniTask AttackIfPossible()
        {
            if (hasCasted) return;

            targetPos = CalculateTarget(); // Check to see if there is a new closest target 

            // Enable the grid selector
            EventBus<SetGridSelector>.Raise(new SetGridSelector()
            {
                Active = true,
                isEnemyTurn = true
            });

            spellController.EnemySpellSelect(Melee, this);

            commandCompletionSource = new UniTaskCompletionSource();

            // If no enemy can be attacked, set command as complete in order to move
            if (!gridSelector.EnemyAttackCellSelect(GridPosition, targetPos)) return;

            await commandCompletionSource.Task;

            commandCompletionSource = null;
        }

        private void EnemyCommandCompleted()
        {
            if (!hasCurrentTurn) return;

            commandCompletionSource?.TrySetResult(); // complete the UniTask
        }

        /// <summary>
        /// Helper function to return closest transform position of either the vampire or werewolf. Return Vector3.negativeInfinity if no valid position was found. 
        /// </summary>
        /// <returns></returns>
        private Vector3 CalculateTarget()
        {
            // Collect any alive, non-ignored taunt targets
            List<BattleUnit> tauntCandidates = new List<BattleUnit>();

            // Iterate through each threat
            foreach (KeyValuePair<BattleUnit, int> kv in aggressiveThreats)
            {
                // Extract the battle unit
                BattleUnit unit = kv.Key;

                // Check if the unit is alive and not ignored
                if (!unit.Dead && !ignoredTargets.ContainsKey(unit))
                {
                    // Add the unit to the list of taunt candidates
                    tauntCandidates.Add(unit);
                }
            }

            // Check if anyone is taunting the enemy
            if (tauntCandidates.Count > 0)
            {
                // Set the default closest distance to the first taunted enemy
                BattleUnit closestTaunt = tauntCandidates[0];
                float closestDistance = Vector3.Distance(transform.position, closestTaunt.transform.position);

                // Iterate through each taunt candidate
                for (int i = 1; i < tauntCandidates.Count; i++)
                {
                    // Compare distances
                    BattleUnit candidate = tauntCandidates[i];
                    float distance = Vector3.Distance(transform.position, candidate.transform.position);

                    // Check if the distance is less than the documented closest distance
                    if (distance < closestDistance)
                    {
                        // Update the closest distance
                        closestDistance = distance;
                        closestTaunt = candidate;
                    }
                }

                // Go to the nearest, taunted enemy
                return closestTaunt.transform.position;
            }

            // Fall back to the nearest alive, non-ignored player
            List<BattleUnit> proximityCandidates = new List<BattleUnit>() { vampire, werewolf };

            // Iterate through each candidate of attack via proximity
            for (int i = proximityCandidates.Count - 1; i >= 0; i--)
            {
                BattleUnit u = proximityCandidates[i];

                // Check if the unit is alive or ignored
                if (u.Dead || ignoredTargets.ContainsKey(u))
                {
                    // Remove the candidate
                    proximityCandidates.RemoveAt(i);
                }
            }

            // Exit case - there's no proximity candidates
            if (proximityCandidates.Count == 0) return Vector3.negativeInfinity;

            // Set the default distance of the first candidate
            BattleUnit nearest = proximityCandidates[0];
            float nearestDistance = Vector3.Distance(transform.position, nearest.transform.position);

            // Iterate through each candidate for proximity
            for (int i = 1; i < proximityCandidates.Count; i++)
            {
                // Compare the distances in proximity
                BattleUnit candidate = proximityCandidates[i];
                float distance = Vector3.Distance(transform.position, candidate.transform.position);

                // Check if the distance is less than the documented nearest distance
                if (distance < nearestDistance)
                {
                    // Update the nearest distance
                    nearestDistance = distance;
                    nearest = candidate;
                }
            }

            // Return the distance to the nearest candidate
            return nearest.transform.position;
        }

        /// <summary>
        /// Tick down the durations of priority effects
        /// </summary>
        private void TickPriorityDurations()
        {
            // Check if there are targets being ignored
            if (ignoredTargets.Count > 0)
            {
                // Create a container for the targets to remove
                List<BattleUnit> targetsToRemove = new List<BattleUnit>();

                foreach (KeyValuePair<BattleUnit, int> kv in ignoredTargets)
                {
                    // Decrement the number of turns left for the target
                    ignoredTargets[kv.Key] = kv.Value - 1;

                    // Check if any turns have expired
                    if (ignoredTargets[kv.Key] <= 0)
                        // Flag the target for removal
                        targetsToRemove.Add(kv.Key);
                }

                // Iterate through each target to remove
                foreach (BattleUnit unit in targetsToRemove)
                {
                    // Remove the target from the dictionary
                    ignoredTargets.Remove(unit);
                }
            }

            if (aggressiveThreats.Count > 0)
            {
                // Create a container for the targets to remove
                List<BattleUnit> targetsToRemove = new List<BattleUnit>();

                // Iterate through each threat
                foreach (KeyValuePair<BattleUnit, int> kv in aggressiveThreats)
                {
                    // Decrement the number of turns left for the target
                    aggressiveThreats[kv.Key] = kv.Value - 1;
                    // Check if any turns have expired
                    if (aggressiveThreats[kv.Key] <= 0)
                        // Flag the target for removal
                        targetsToRemove.Add(kv.Key);
                }
                // Iterate through each target to remove
                foreach (BattleUnit unit in targetsToRemove)
                {
                    // Remove the target from the dictionary
                    aggressiveThreats.Remove(unit);
                }
            }
        }

        /// <summary>
        /// Aggro a target
        /// </summary>
        public void AggroTarget(BattleUnit unit, int turns)
        {
            if (aggressiveThreats.ContainsKey(unit))
            {
                // Get the existing turn duration
                int existing = aggressiveThreats[unit];

                // Extend the existing turn duration
                aggressiveThreats[unit] = Mathf.Max(existing, turns);

                return;
            }

            // Add the unit as an aggressive threat
            aggressiveThreats.Add(unit, turns);
        }

        /// <summary>
        /// Ignore a target
        /// </summary>
        public void IgnoreTarget(BattleUnit unit, int turns)
        {
            // Exit case - the unit is already being ignored
            if (ignoredTargets.ContainsKey(unit))
            {
                // Get the existing turn duration
                int existing = ignoredTargets[unit];

                // Extend the existing turn duration
                ignoredTargets[unit] = Mathf.Max(existing, turns);

                return;
            }

            // Add the unit to the ignored targets
            ignoredTargets.Add(unit, turns);
        }
    }
}