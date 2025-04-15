using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Vampwolf.Grid;
using Vampwolf.Units;

namespace Vampwolf.Battles.Commands
{
    public class MoveCommand : IBattleCommand
    {
        private readonly BattleUnit unit;
        private readonly GridManager gridManager;
        private readonly Vector3Int targetPosition;

        public MoveCommand(BattleUnit unit, GridManager gridManager, Vector3Int targetPosition)
        {
            this.unit = unit;
            this.gridManager = gridManager;
            this.targetPosition = targetPosition;
        }

        /// <summary>
        /// Executes the move command for the unit
        /// </summary>
        public async UniTask Execute()
        {
            // Find the path to the target position
            List<Vector3Int> path = gridManager.FindPath(unit.GridPosition, targetPosition);

            // Move through the path asynchronously
            await unit.MoveThrough(gridManager, path);
        }
    }
}
