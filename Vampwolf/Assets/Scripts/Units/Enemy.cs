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
        private Transform vampire;
        private Transform werewolf;
        private Vector3 closestTargetPos = Vector3.positiveInfinity;
        private GridSelector gridSelector;
        private SpellsController spellController;

        private EventBinding<CommandProcessed> onCommandProcessed;
        private UniTaskCompletionSource commandCompletionSource;

        private const int Melee = 0;
        private const int Ranged = 1;

        private void Start()
        {
            vampire = FindObjectOfType<Vampire>().transform;
            werewolf = FindObjectOfType<Werewolf>().transform;
            gridSelector = FindObjectOfType<GridSelector>();
            spellController = FindObjectOfType<SpellsController>();
        }

        private void OnEnable()
        {
            onCommandProcessed = new EventBinding<CommandProcessed>(OnEnemyCommandComplete);
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

            // Enable the grid selector
            EventBus<SetGridSelector>.Raise(new SetGridSelector()
            {
                Active = true,
                isEnemyTurn = true
            });

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

        private void OnEnemyCommandComplete()
        {
            if (!hasCurrentTurn) return;

            commandCompletionSource?.TrySetResult(); // complete the UniTask
        }

        /// <summary>
        /// This will command the active enemy unit to move toward the closest player
        /// </summary>
        private async UniTask MoveToClosestPlayer()
        {
            // Exit case - still has movement
            if (movementLeft <= 0) return;

            closestTargetPos = CalculateClosestPlayer();
            if (Mathf.Abs((transform.position - closestTargetPos).magnitude) <= 1.415f) return; // If already 1 tile away from target, there is no need to move

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

            // Wait until movement is complete 
            await commandCompletionSource.Task;
        }

        /// <summary>
        /// This will command the enemy to attack if an enemy is within their attack's range
        /// </summary>
        /// <returns></returns>
        private async UniTask AttackIfPossible()
        {
            if (hasCasted) return;
            spellController.EnemySpellSelect(Melee, this);

            commandCompletionSource = new UniTaskCompletionSource();

            // If no enemy can be attacked, set command as complete in order to move on
            if (!gridSelector.EnemyAttackCellSelect(GridPosition, closestTargetPos)) OnEnemyCommandComplete();

            await commandCompletionSource.Task;

            commandCompletionSource = null;
        }

        /// <summary>
        /// Helper function to return closest transform position of either the vampire or werewolf
        /// </summary>
        /// <returns></returns>
        private Vector3 CalculateClosestPlayer()
        {
            return (Vector3.Distance(transform.position, vampire.position) <= Vector3.Distance(transform.position, werewolf.position))
                ? vampire.position : werewolf.position;
        }
    }
}