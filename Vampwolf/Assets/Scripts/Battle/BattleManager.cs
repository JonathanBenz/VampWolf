using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vampwolf.Battles.Commands;
using Vampwolf.Battles.States;
using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Input;
using Vampwolf.StateMachines;
using Vampwolf.Units;

namespace Vampwolf.Battles
{
    public class BattleManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputReader inputReader;

        private Queue<BattleUnit> turnQueue;
        private Queue<IBattleCommand> commandQueue;

        private StateMachine stateMachine;

        [Header("Fields")]
        [SerializeField] private string currentState;
        [SerializeField] private BattleUnit activeUnit;
        [SerializeField] private int numberOfEnemies;
        [SerializeField] private bool commanding;
        [SerializeField] private bool processing;
        [SerializeField] private bool changingTurns;
        [SerializeField] private bool skippingTurn;

        private EventBinding<MoveCellSelected> onMoveCellSelected;
        private EventBinding<TargetCellSelected> onTargetCellSelected;
        private EventBinding<SkipTurn> onSkipTurn;
        private EventBinding<RemoveUnit> onRemoveUnit;

        public Queue<BattleUnit> TurnQueue => turnQueue;
        public Queue<IBattleCommand> CommandQueue => commandQueue;
        public BattleUnit ActiveUnit { get => activeUnit; set => activeUnit = value; }
        public bool SkippingTurn { get => skippingTurn; set => skippingTurn = value; }
        public bool Commanding { get => commanding; set => commanding = value; }
        public bool Processing  { get => processing; set => processing = value; }
        public bool ChangingTurns { get => changingTurns; set => changingTurns = value; }

        private void OnEnable()
        {
            onMoveCellSelected = new EventBinding<MoveCellSelected>(MoveActivePlayerUnit);
            EventBus<MoveCellSelected>.Register(onMoveCellSelected);

            onTargetCellSelected = new EventBinding<TargetCellSelected>(CastSpell);
            EventBus<TargetCellSelected>.Register(onTargetCellSelected);

            onSkipTurn = new EventBinding<SkipTurn>(SkipTurn);
            EventBus<SkipTurn>.Register(onSkipTurn);

            onRemoveUnit = new EventBinding<RemoveUnit>(RemoveUnit);
            EventBus<RemoveUnit>.Register(onRemoveUnit);
        }

        private void OnDisable()
        {
            EventBus<MoveCellSelected>.Deregister(onMoveCellSelected);
            EventBus<TargetCellSelected>.Deregister(onTargetCellSelected);
            EventBus<SkipTurn>.Deregister(onSkipTurn);
            EventBus<RemoveUnit>.Deregister(onRemoveUnit);
        }

        private void Start()
        {
            // Initialize the queues
            turnQueue = new Queue<BattleUnit>();
            commandQueue = new Queue<IBattleCommand>();

            // Start counting enemies
            numberOfEnemies = 0;

            // Find all units in the scene and order them by initiative
            BattleUnit[] units = FindObjectsOfType<BattleUnit>().OrderByDescending(unit => unit.Initiative).ToArray();

            // Iterate through each unit
            for (int i = 0; i < units.Length; i++)
            {
                // Place the unit on the grid
                EventBus<PlaceUnit>.Raise(new PlaceUnit
                {
                    Unit = units[i],
                    GridPosition = units[i].GridPosition
                });

                // Check if the unit is not a Werewolf and not a Vampire
                if (units[i] is Enemy)
                {
                    // Increment the number of enemies
                    numberOfEnemies++;
                }

                // Add the units to the turn queue
                turnQueue.Enqueue(units[i]);

                // Notify that the unit's initiative has been registered
                EventBus<InitiativeRegistered>.Raise(new InitiativeRegistered()
                {
                    Unit = units[i]
                });
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

            currentState = stateMachine.GetState().ToString();
        }

        /// <summary>
        /// Set up the state machine
        /// </summary>
        private void SetupStateMachine()
        {
            // Initialize the state machine
            stateMachine = new StateMachine();

            // Create the states
            StartTurnState startTurn = new StartTurnState(this);
            AwaitCommandState awaitCommand = new AwaitCommandState(this, inputReader);
            ProcessCommandState processCommand = new ProcessCommandState(this);
            NextTurnState nextTurn = new NextTurnState(this);
            EndBattleState endBattle = new EndBattleState(this);

            // Definte state transitions
            stateMachine.At(startTurn, awaitCommand, new FuncPredicate(() => commanding));
            stateMachine.At(awaitCommand, processCommand, new FuncPredicate(() => commandQueue.Count > 0));
            stateMachine.At(awaitCommand, nextTurn, new FuncPredicate(() => skippingTurn));
            stateMachine.At(processCommand, awaitCommand, new FuncPredicate(() => !processing && commanding));
            stateMachine.At(processCommand, nextTurn, new FuncPredicate(() => !processing && !commanding));
            stateMachine.At(nextTurn, startTurn, new FuncPredicate(() => !changingTurns));
            stateMachine.Any(endBattle, new FuncPredicate(() => numberOfEnemies <= 0));

            // Set the default state
            stateMachine.SetState(startTurn);
        }

        /// <summary>
        /// Skip the active unit's turn
        /// </summary>
        private void SkipTurn() => skippingTurn = true;

        /// <summary>
        /// Move the active player unit to a new grid position
        /// </summary>
        private void MoveActivePlayerUnit(MoveCellSelected eventData)
        {
            // Exit case - the unit has already moved
            if (activeUnit.MovementLeft <= 0) return;

            // Create a new move command
            MoveCommand moveCommand = new MoveCommand(activeUnit, eventData.GridManager, eventData.GridPosition);

            // Add the command to the queue
            commandQueue.Enqueue(moveCommand);
        }

        /// <summary>
        /// Attempt to cast a spell on a target cell
        /// </summary>
        private void CastSpell(TargetCellSelected eventData)
        {
            // Exit case - the unit has already casted
            if (activeUnit.HasCasted) return;

            // Get the list of units
            List<BattleUnit> units = turnQueue.ToList();

            // Create a default, empty target
            BattleUnit target = null;

            // Iterate through the units
            foreach (BattleUnit unit in units)
            {
                // Skip if the unit's grid position does not match the given grid position
                if (unit.GridPosition != eventData.GridPosition) continue;

                target = unit;
            }

            // Exit case - if a target is not found
            if (target == null) return;

            // Create a new cast command
            SpellCommand spellCommand = new SpellCommand(target, eventData.Spell);

            // Add the command to the queue
            commandQueue.Enqueue(spellCommand);

            // Disable the spellbar for the rest of the turn
            EventBus<DisableSpells>.Raise(new DisableSpells());

            // Notify that the unit has casted
            activeUnit.HasCasted = true;
        }

        /// <summary>
        /// Remove a unit from the turn queue
        /// </summary
        private void RemoveUnit(RemoveUnit eventData)
        {
            // Cast the turn queue to a list
            List<BattleUnit> activeUnits = turnQueue.ToList();

            // Exit case - the unit is not in the turn queue
            if (!activeUnits.Contains(eventData.Unit)) return;

            // Remove the unit from the turn queue
            activeUnits.Remove(eventData.Unit);

            // Remove the unit from the initiative tracker
            EventBus<InitiativeDeregistered>.Raise(new InitiativeDeregistered()
            {
                Unit = eventData.Unit
            });

            // Check if the unit is an enemy
            if (eventData.IsEnemy)
                // Decrement the number of enemies
                numberOfEnemies--;

            // Clear the turn queue
            turnQueue.Clear();

            // Rebuild the turn queue
            foreach (BattleUnit unit in activeUnits)
            {
                // Add the unit to the turn queue
                turnQueue.Enqueue(unit);
            }
        }
    }
}
