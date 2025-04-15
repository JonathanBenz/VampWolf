using Cysharp.Threading.Tasks;
using UnityEngine;
using Vampwolf.Units;

namespace Vampwolf.Battle.Commands
{
    public class MoveCommand : IBattleCommand
    {
        private readonly BattleUnit unit;
        private readonly Vector3Int targetPosition;

        public MoveCommand(BattleUnit unit, Vector3Int targetPosition)
        {
            this.unit = unit;
            this.targetPosition = targetPosition;
        }

        /// <summary>
        /// Executes the move command for the unit
        /// </summary>
        public async UniTask Execute() => await unit.MoveTo(targetPosition);
    }
}
