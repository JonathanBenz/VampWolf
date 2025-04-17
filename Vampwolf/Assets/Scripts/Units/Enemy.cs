using Cysharp.Threading.Tasks;
using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Grid;
using UnityEngine;
using System.Collections.Generic;

namespace Vampwolf.Units
{
    public class Enemy : BattleUnit
    {
        private Transform vampire;
        private Transform werewolf;
        private Vector3 closestTargetPos = Vector3.positiveInfinity;
        private GridSelector gridSelector;

        private void Start()
        {
            gridSelector = FindObjectOfType<GridSelector>();
            vampire = FindObjectOfType<Vampire>().transform;
            werewolf = FindObjectOfType<Werewolf>().transform;
        }

        public override void AwaitCommands()
        {
            MoveToClosestPlayer();
            AttackIfPossible();
            //EventBus<SkipTurn>.Raise(new SkipTurn());
        }

        public override async UniTask EndTurn()
        {
            hasCurrentTurn = false;
            await UniTask.CompletedTask;
        }

        public override async UniTask StartTurn()
        {
            hasMoved = false;
            hasCasted = false;
            hasCurrentTurn = true;

            // Enable the grid selector
            EventBus<SetGridSelector>.Raise(new SetGridSelector()
            {
                Active = true,
                isEnemyTurn = true
            });

            Debug.Log($"It's {gameObject.name}'s turn!");

            await UniTask.CompletedTask;
        }

        protected override void OnDeath()
        {
            // Remove the unit
            EventBus<RemoveUnit>.Raise(new RemoveUnit()
            {
                Unit = this,
                IsEnemy = true
            });

            // Hide the unit
            //gameObject.SetActive(false);

            // Display unit death sprite
            spriteRenderer.sprite = statData.deathSprite;
        }

        /// <summary>
        /// This will command the active enemy unit to move toward the closest player
        /// </summary>
        private void MoveToClosestPlayer()
        {
            closestTargetPos = CalculateClosestPlayer();
            if (Mathf.Abs((transform.position - closestTargetPos).magnitude) <= 1.415f) return; // If already 1 tile away from target, there is no need to move

            // Set the movement selection mode
            EventBus<SetMovementSelectionMode>.Raise(new SetMovementSelectionMode()
            {
                GridPosition = gridPosition,
                Range = stats.MovementRange,
                tileColor = 0
            });

            // Calculate the path to the player and move towards there
            Vector3Int start = gridPosition;
            gridSelector.EnemyCellSelect(GridPosition, closestTargetPos);
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

        private void AttackIfPossible()
        {

        }
    }
}