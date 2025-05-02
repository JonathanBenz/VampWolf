using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Vampwolf.EventBus;
using Vampwolf.Events;
using Vampwolf.Input;
using Vampwolf.Spells;

namespace Vampwolf.Grid
{
    public class GridSelector : MonoBehaviour
    {
        public enum SelectionMode
        {
            Move,
            Target
        }

        [Header("References")]
        [SerializeField] private InputReader inputReader;
        [SerializeField] private GameObject hoverCursorPrefab;
        private GridManager gridManager;
        private GridHighlighter gridHighlighter;
        private GameObject hoverCursor;

        private Spell currentSpell;

        [Header("Fields")]
        [SerializeField] private bool active;
        [SerializeField] private bool isEnemyUsing;
        [SerializeField] private SelectionMode selectionState;
        private Vector3Int currentCellPos;
        private Vector3Int lastCellPos;

        private EventBinding<SetGridSelector> onSetGridSelector;
        private EventBinding<SetMovementSelectionMode> onSetMovementSelectionMode;
        private EventBinding<SetSpellSelectionMode> onSetSpellSelectionMode;

        private void Awake()
        {
            // Get components
            gridManager = GetComponent<GridManager>();
            gridHighlighter = GetComponent<GridHighlighter>();

            // Create the hover cursor
            hoverCursor = Instantiate(hoverCursorPrefab, transform);
        }

        private void OnEnable()
        {
            inputReader.Select += Select;

            onSetGridSelector = new EventBinding<SetGridSelector>(SetGridSelector);
            EventBus<SetGridSelector>.Register(onSetGridSelector);

            onSetMovementSelectionMode = new EventBinding<SetMovementSelectionMode>(SetMovementSelectionMode);
            EventBus<SetMovementSelectionMode>.Register(onSetMovementSelectionMode);

            onSetSpellSelectionMode = new EventBinding<SetSpellSelectionMode>(SetSpellSelectionMode);
            EventBus<SetSpellSelectionMode>.Register(onSetSpellSelectionMode);
        }

        private void OnDisable()
        {
            inputReader.Select -= Select;

            EventBus<SetGridSelector>.Deregister(onSetGridSelector);
            EventBus<SetMovementSelectionMode>.Deregister(onSetMovementSelectionMode);
            EventBus<SetSpellSelectionMode>.Deregister(onSetSpellSelectionMode);
        }

        private void Update()
        {
            // Exit case - the selector is not active, or an enemy AI is currently using it
            if (!active || isEnemyUsing) return;

            // Get the current mouse position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3Int hoveredCell = gridManager.GetGridPositionFromWorld(mousePos);

            // Exit case - the grid position does not exist
            if (!gridManager.GridPositionExists(hoveredCell)) return;

            // Exit case - the cell is not highlighted
            if (!gridHighlighter.IsCellHighlighted(hoveredCell))
            {
                // Deactivate the hover cursor
                hoverCursor.SetActive(false);
                return;
            }

            // Exit case - hovering over the same cell
            if (hoveredCell == lastCellPos) return;

            // Set the hover cursor's position
            hoverCursor.transform.position = gridManager.GetWorldPositionFromGrid(hoveredCell);

            // Update cell positions
            lastCellPos = hoveredCell;
            currentCellPos = hoveredCell;

            // Exit case - the hover cursor is already active
            if (hoverCursor.activeSelf) return;

            hoverCursor.SetActive(true);
        }

        /// <summary>
        /// Set whether or not the grid selector is active
        /// </summary>
        private void SetGridSelector(SetGridSelector eventData) { active = eventData.Active; isEnemyUsing = eventData.isEnemyTurn; }

        /// <summary>
        /// Set the movement selection mode
        /// </summary>
        private void SetMovementSelectionMode(SetMovementSelectionMode eventData)
        {
            // Exit case - the selector is not active
            if (!active) return;

            // Set the selection state
            selectionState = SelectionMode.Move;

            // Clear the current spell
            currentSpell = null;

            // Highlight the cells in range
            gridHighlighter.HighlightCellsInMovementRange(eventData.GridPosition, eventData.Range, eventData.HighlightType);
        }

        /// <summary>
        /// Set the selection mode to targeting for spells
        /// </summary>
        private void SetSpellSelectionMode(SetSpellSelectionMode eventData)
        {
            // Exit case - the selector is not active
            if (!active) return;

            // Set the current spell
            currentSpell = eventData.Spell;

            // Set the selection state
            selectionState = SelectionMode.Target;
            
            // Highlight the cells in range
            gridHighlighter.HighlightCellsInSpellRange(eventData.GridPosition, eventData.Spell.Range, eventData.Spell.SpellType, eventData.Spell.Predicate);
        }

        /// <summary>
        /// Input handler for selecting a tile
        /// </summary>
        private void Select(bool started)
        {
            // Exit case - the selector is not active
            if (!active) return;

            // Exit case - the button was lifted
            if (!started) return;

            // Exit case - if the hover cursor is not active (not hovering over a cell)
            if (!hoverCursor.activeSelf) return;

            switch (selectionState)
            {
                case SelectionMode.Move:
                    // Notify that a cell has been selected for movement
                    EventBus<MoveCellSelected>.Raise(new MoveCellSelected()
                    {
                        GridManager = gridManager,
                        GridPosition = currentCellPos
                    });
                    break;

                case SelectionMode.Target:
                    // Exit case - there's no spell selected
                    if (currentSpell == null) return;

                    // Notify that a cell has been selected for casting
                    EventBus<TargetCellSelected>.Raise(new TargetCellSelected()
                    {
                        Spell = currentSpell,
                        GridPosition = currentCellPos
                    });
                    break;
            }
        }

        /// <summary>
        /// During an enemy's turn, this method is called to handle their movement behavior
        /// </summary>
        /// <param name="gridPos">The enemy's current GridPosition from raising the SetMovementSelectionMode event.</param>
        /// <param name="targetPos">Where the enemy wants to move toward (e.g., the closest player's location).</param>
        public void EnemyMovementCellSelect(Vector3Int gridPos, Vector3 targetPos)
        {
            Vector3Int target = gridManager.GetGridPositionFromWorld(targetPos);

            // Calculate the path to the target player and the unit's highlighted cells to choose from
            List<Vector3Int> path = gridManager.FindPath(gridPos, target);
            List<HighlightTile> highlightedTiles = gridHighlighter.HighlightedCells;

            // If no valid path, return
            if (path.Count == 0) return;

            // Find the best highlighted tile to go to 
            Vector3Int bestTile = gridPos;
            for (int i = 1; i < path.Count - 1; i++)
            {
                bool isHighlighted = highlightedTiles.Exists(h => h.GridPosition == path[i]);
                if (isHighlighted) bestTile = path[i]; // Update the best tile

                // If we reach a tile that is no longer within the Unit's movement range, then break out because we have already found the best possible tile
                else break;
            }
            StartCoroutine(MoveToBestCalculatedTile(bestTile));
        }

        /// <summary>
        /// During an enemy's turn, this method is called to handle their attack behavior. Return false if no target can be attacked
        /// </summary>
        /// <param name="gridPos"></param>
        /// <param name="targetPos"></param>
        public bool EnemyAttackCellSelect(Vector3Int gridPos, Vector3 targetPos)
        {
            Vector3Int target = gridManager.GetGridPositionFromWorld(targetPos);

            // Calculate the path to the target player and the unit's highlighted cells to choose from
            List<Vector3Int> path = gridManager.FindPath(gridPos, target);
            List<HighlightTile> highlightedTiles = gridHighlighter.HighlightedCells;

            // Find if the enemy lies on a highlighted tile
            Vector3Int enemyTile = path[path.Count - 1];
            if (highlightedTiles.Exists(h => h.GridPosition == enemyTile))
            {
                StartCoroutine(AttackEnemyTile(enemyTile));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Helper coroutine to move the Enemy to the best calculated tile during EnemyCellSelect()
        /// </summary>
        private IEnumerator MoveToBestCalculatedTile(Vector3Int bestTile)
        {
            // Activate the hover cursor over the bestTile
            hoverCursor.transform.position = gridManager.GetWorldPositionFromGrid(bestTile);
            currentCellPos = bestTile;
            hoverCursor.SetActive(true);

            yield return new WaitForSeconds(1f); // Have the hover appear for a second before raising the event

            hoverCursor.SetActive(false);
            // Move to the bestTile
            EventBus<MoveCellSelected>.Raise(new MoveCellSelected()
            {
                GridManager = gridManager,
                GridPosition = currentCellPos
            });
        }

        private IEnumerator AttackEnemyTile(Vector3Int enemyTile)
        {
            // Activate the hover cursor over the player character
            hoverCursor.transform.position = gridManager.GetWorldPositionFromGrid(enemyTile);
            currentCellPos = enemyTile;
            hoverCursor.SetActive(true);

            yield return new WaitForSeconds(1f);

            hoverCursor.SetActive(false);
            // Exit case - there's no spell selected
            if (currentSpell == null) { Debug.Log("ERROR: Enemy Spell was NULL"); yield break; }
            EventBus<TargetCellSelected>.Raise(new TargetCellSelected()
            {
                Spell = currentSpell,
                GridPosition = currentCellPos
            });
        }
    }
}
