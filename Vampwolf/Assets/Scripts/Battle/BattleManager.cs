using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vampwolf.Battle.Commands;
using Vampwolf.Battle.States;
using Vampwolf.StateMachines;
using Vampwolf.Units;

namespace Vampwolf.Battle
{
    public class BattleManager : MonoBehaviour
    {
        private Queue<BattleUnit> turnQueue;
        private Queue<IBattleCommand> commandQueue;

        private StateMachine stateMachine;
        private BattleUnit activeUnit;

        public BattleUnit ActiveUnit => activeUnit;
        public Queue<IBattleCommand> CommandQueue => commandQueue;

        private void Start()
        {
            // Initialize the queues
            turnQueue = new Queue<BattleUnit>();
            commandQueue = new Queue<IBattleCommand>();

            // Find all units in the scene and order them by initiative
            BattleUnit[] units = FindObjectsOfType<BattleUnit>().OrderByDescending(unit => unit.Initiative).ToArray();

            // Iterate through each unit
            for(int i = 0; i < units.Length; i++)
            {
                // Add the units to the turn queue
                turnQueue.Enqueue(units[i]);
            }

            // Set the active unit
            activeUnit = turnQueue.Peek();

            // Set up the state machine
            SetupStateMachine();
        }

        private void Update()
        {
            // Update the state machine
            stateMachine.Update();
        }

        private void SetupStateMachine()
        {
            // Initialize the state machine
            stateMachine = new StateMachine();

            // Create the states
            AwaitCommandState awaitCommand = new AwaitCommandState(this);
            ProcessCommandState processCommand = new ProcessCommandState(this);

            // Definte state transitions
            stateMachine.At(awaitCommand, processCommand, new FuncPredicate(() => commandQueue.Count > 0));
            stateMachine.At(processCommand, awaitCommand, new FuncPredicate(() => commandQueue.Count == 0));

            // Set the default state
            stateMachine.SetState(awaitCommand);
        }
    }
}
