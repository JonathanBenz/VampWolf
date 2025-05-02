using Cysharp.Threading.Tasks;
using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Grid;
using Vampwolf.Spells;
using UnityEngine;

namespace Vampwolf.Units
{
    public class Enemy : BattleUnit
    {
        private Vampire vampire;
        private Werewolf werewolf;
        private Vector3 closestTargetPos = Vector3.positiveInfinity;
        private GridSelector gridSelector;
        private SpellsController spellController;

        private EventBinding<CommandProcessed> onCommandProcessed;
        private UniTaskCompletionSource commandCompletionSource;

        private const int Melee = 0;
        private const int Ranged = 1;

        private void Start()
        {
            vampire = FindObjectOfType<Vampire>();
            werewolf = FindObjectOfType<Werewolf>();
            gridSelector = FindObjectOfType<GridSelector>();
            spellController = FindObjectOfType<SpellsController>();
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
            // Clear the highlights
            EventBus<ClearHighlights>.Raise(new ClearHighlights());

            await base.EndTurn();
        }

        private async UniTask ExecuteEnemyTurn()
        {
            await MoveToClosestPlayer();
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
        private async UniTask MoveToClosestPlayer()
        {
            // Exit case - still has movement
            if (movementLeft <= 0) return;

            closestTargetPos = CalculateClosestPlayer();
            if (closestTargetPos == Vector3.negativeInfinity) return; // Exit case - no valid positions were found when calculating. 

            // If already 1 tile away from target, there is no need to move
            if (Mathf.Abs((transform.position - closestTargetPos).magnitude) < 1.5f) return;

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
            gridSelector.EnemyMovementCellSelect(GridPosition, closestTargetPos);

            await commandCompletionSource.Task;
        }

        /// <summary>
        /// This will command the enemy to attack if an enemy is within their attack's range
        /// </summary>
        /// <returns></returns>
        private async UniTask AttackIfPossible()
        {
            if (hasCasted) return;

            closestTargetPos = CalculateClosestPlayer(); // Check to see if there is a new closest target 

            // Enable the grid selector
            EventBus<SetGridSelector>.Raise(new SetGridSelector()
            {
                Active = true,
                isEnemyTurn = true
            });

            spellController.EnemySpellSelect(Melee, this);

            commandCompletionSource = new UniTaskCompletionSource();

            // If no enemy can be attacked, set command as complete in order to move
            if (!gridSelector.EnemyAttackCellSelect(GridPosition, closestTargetPos)) return;

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
        private Vector3 CalculateClosestPlayer()
        {
            float distToVamp = Vector3.Distance(transform.position, vampire.transform.position);
            float distToWolf = Vector3.Distance(transform.position, werewolf.transform.position);

            // If target is close and alive, go to that target
            if (distToVamp <= distToWolf && !vampire.Dead) return vampire.transform.position;
            if (distToWolf <= distToVamp && !werewolf.Dead) return werewolf.transform.position;

            // If target is far, but the close target is dead, go to the far target
            if (distToVamp > distToWolf && werewolf.Dead) return vampire.transform.position;
            if (distToWolf > distToVamp && vampire.Dead) return werewolf.transform.position;

            // Else no valid position was found
            else return Vector3.negativeInfinity;
        }
    }
}