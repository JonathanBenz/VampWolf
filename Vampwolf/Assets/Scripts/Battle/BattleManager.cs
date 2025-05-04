using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vampwolf.Battles.Commands;
using Vampwolf.Battles.States;
using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Grid;
using Vampwolf.Input;
using Vampwolf.Spells;
using Vampwolf.StateMachines;
using Vampwolf.Units;

namespace Vampwolf.Battles
{
    public class BattleManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputReader inputReader;
        [SerializeField] private GridManager gridManager;

        private Queue<BattleUnit> turnQueue;
        private Queue<IBattleCommand> commandQueue;

        private StateMachine stateMachine;

        [Header("Fields")]
        [SerializeField] private string currentState;
        [SerializeField] private BattleUnit activeUnit;
        [SerializeField] private int numberOfEnemies;
        [SerializeField] private int numberOfPlayers;
        [SerializeField] private bool commanding;
        [SerializeField] private bool processing;
        [SerializeField] private bool changingTurns;
        [SerializeField] private bool skippingTurn;
        private Vector3Int hellPortalPos;

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
        public int NumberOfEnemies { get => numberOfEnemies; }

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

        private void Awake()
        {
            // Find the hell portal in the scene and place it on the grid
            HellPortal hellPortal = FindObjectOfType<HellPortal>();
            EventBus<PlaceHellPortal>.Raise(new PlaceHellPortal
            {
                HellPortal = hellPortal,
                GridPosition = hellPortal.GridPosition
            });
            hellPortalPos = hellPortal.GridPosition;
        }

        private void Start()
        {
            // Initialize the queues
            turnQueue = new Queue<BattleUnit>();
            commandQueue = new Queue<IBattleCommand>();

            // Start counting enemies and players
            numberOfEnemies = 0;
            numberOfPlayers = 2;

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
            stateMachine.At(awaitCommand, nextTurn, new FuncPredicate(() => skippingTurn && commandQueue.Count == 0));
            stateMachine.At(processCommand, awaitCommand, new FuncPredicate(() => !processing && commanding));
            stateMachine.At(processCommand, nextTurn, new FuncPredicate(() => !processing && !commanding && commandQueue.Count == 0));
            stateMachine.At(nextTurn, startTurn, new FuncPredicate(() => !changingTurns));
            stateMachine.Any(endBattle, new FuncPredicate(() => numberOfEnemies <= 0 || numberOfPlayers <= 0));

            // Set the default state
            stateMachine.SetState(startTurn);
        }

        /// <summary>
        /// Skip the active unit's turn
        /// </summary>
        private void SkipTurn() => skippingTurn = true;

        /// <summary>
        /// Find a unit in the turn queue using a position
        /// </summary>
        private BattleUnit FindUnitAtPosition(Vector3Int gridPosition)
        {
            // Cast the turn queue to a list
            List<BattleUnit> activeUnits = turnQueue.ToList();

            // Iterate through the active units
            foreach (BattleUnit currentUnit in activeUnits)
            {
                // Check if the grid positions are equal
                if(currentUnit.GridPosition == gridPosition)
                {
                    // Set the found unit
                    return currentUnit;
                }
            }

            return null;
        }

        /// <summary>
        /// Find a unit in the turn queue using a character type
        /// </summary>
        private BattleUnit FindUnitByType(CharacterType characterType)
        {
            // Cast the turn queue to a list
            List<BattleUnit> activeUnits = turnQueue.ToList();

            // Iterate through the active units
            foreach (BattleUnit currentUnit in activeUnits)
            {
                // Check if the unit is of the same type
                if (currentUnit.CharacterType == characterType)
                {
                    // Set the found unit
                    return currentUnit;
                }
            }

            return null;
        }

        private List<BattleUnit> FindUnitsInRange(Vector3Int center, int range)
        {
            // Cast the turn queue to a list
            List<BattleUnit> activeUnits = turnQueue.ToList();
            List<Vector3Int> reachableCells = gridManager.GetReachableCells(center, range);
            List<BattleUnit> unitsInRange = new List<BattleUnit>();

            // Iterate through the active units
            foreach (BattleUnit currentUnit in activeUnits)
            {
                foreach(Vector3Int cell in reachableCells)
                {
                    // Skip if the unit is not in range
                    if (currentUnit.GridPosition != cell) continue;

                    // Add the current unit
                    unitsInRange.Add(currentUnit);
                }
            }

            return unitsInRange;
        }

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

            // If the spell was cast to open the portal, then open the portal and return
            if (eventData.GridPosition == hellPortalPos)
            {
                EventBus<PortalOpened>.Raise(new PortalOpened());
                return;
            }

            // Try to find the target at the grid position
            BattleUnit target = FindUnitAtPosition(eventData.GridPosition);

            // Exit case - the target is null and the spell requires a target
            if (target == null && eventData.Spell.RequiresTarget) return;

            // Create a default, empty caster
            BattleUnit caster = FindUnitByType(eventData.Spell.CharacterType);

            // Exit case - the caster is null and the spell requires a caster
            if (caster == null && eventData.Spell.RequiresTarget) return;

            // Find units in range
            List<BattleUnit> unitsInRange = FindUnitsInRange(caster.GridPosition, eventData.Spell.Range);

            // Get all units on the board
            List<BattleUnit> allUnits = turnQueue.ToList();

            // Create a new cast command
            SpellCommand spellCommand = new SpellCommand(caster, target, unitsInRange, allUnits, eventData.Spell, eventData.GridPosition);

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
            List<BattleUnit> battleUnits = turnQueue.ToList();

            // Extract the unit
            BattleUnit unit = eventData.Unit;

            // Exit case - the unit is not in the turn queue
            if (!battleUnits.Contains(unit)) return;

            // Remove the unit from the initiative tracker
            EventBus<InitiativeDeregistered>.Raise(new InitiativeDeregistered()
            {
                Unit = unit
            });

            // Check if the unit is an enemy
            if (eventData.IsEnemy)
                // Decrement the number of enemies
                numberOfEnemies--;
            else numberOfPlayers--;
        }
    }
}
