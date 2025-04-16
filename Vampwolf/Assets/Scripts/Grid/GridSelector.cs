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
            // Exit case - the selector is not active
            if (!active) return;

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
        private void SetGridSelector(SetGridSelector eventData) => active = eventData.Active;

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
            gridHighlighter.HighlightCellsInRange(eventData.GridPosition, eventData.Range);
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
            gridHighlighter.HighlightCellsInRange(eventData.GridPosition, eventData.Spell.Range);
        }

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
    }
}
