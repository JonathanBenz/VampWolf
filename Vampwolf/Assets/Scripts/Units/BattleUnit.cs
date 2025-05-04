using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Vampwolf.Grid;
using DG.Tweening;
using Vampwolf.EventBus;
using Vampwolf.Spells;
using Vampwolf.Units.Stats;
using Vampwolf.Events;

namespace Vampwolf.Units
{
    public abstract class BattleUnit : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] public UnitStatData statData;

        [Header("Details")]
        [SerializeField] private CharacterType characterType;
        [SerializeField] private string unitName;
        [SerializeField] protected Vector3Int gridPosition;
        [SerializeField] protected int health;
        [SerializeField] protected UnitStats stats;
        [SerializeField] protected bool hasCasted;
        protected bool hasCurrentTurn;
        protected int movementLeft;
        protected SpriteRenderer spriteRenderer;
        protected SpriteRenderer ringSprite;
        protected GameObject bloodSplatter;
        private StoneTiles stoneTiles;

        public CharacterType CharacterType => characterType;
        public string Name => unitName;
        public Vector3Int GridPosition => gridPosition;
        public int Initiative => stats.Initiative;
        public int MovementLeft => movementLeft;
        public bool HasCasted { get => hasCasted; set => hasCasted = value; }
        public int Health => health;
        public bool Dead => health <= 0;
        public int MovementRange => stats.MovementRange;
        public Sprite Frame => statData.frame;
        public Sprite Portrait => statData.portrait;

        // Caching this array for fog removal
        Vector3Int[] fogRemovalDirs = {
                        new Vector3Int( 0,  0, 0),    // DEFAULT                       
                        new Vector3Int( 1,  0, 0),    // RIGHT
                        new Vector3Int( 2,  0, 0),
                        new Vector3Int( 3,  0, 0),
                        new Vector3Int( 4,  0, 0),
                        new Vector3Int(-1,  0, 0),    // LEFT
                        new Vector3Int(-2,  0, 0),
                        new Vector3Int(-3,  0, 0),
                        new Vector3Int(-4,  0, 0),
                        new Vector3Int( 0,  1, 0),    // TOP
                        new Vector3Int( 0,  2, 0),
                        new Vector3Int( 0,  3, 0),
                        new Vector3Int( 0,  4, 0),
                        new Vector3Int( 0, -1, 0),    // BOTTOM
                        new Vector3Int( 0, -2, 0),
                        new Vector3Int( 0, -3, 0),
                        new Vector3Int( 0, -4, 0),
                        new Vector3Int( 1,  1, 0),    // TOP-RIGHT
                        new Vector3Int( 1,  2, 0),
                        new Vector3Int( 1,  3, 0),
                        new Vector3Int( 2,  1, 0),
                        new Vector3Int( 2,  2, 0),
                        new Vector3Int( 2,  3, 0),
                        new Vector3Int( 3,  1, 0),
                        new Vector3Int( 3,  2, 0),
                        new Vector3Int( 3,  3, 0),
                        new Vector3Int(-1,  1, 0),    // TOP-LEFT
                        new Vector3Int(-1,  2, 0),
                        new Vector3Int(-1,  3, 0),
                        new Vector3Int(-2,  1, 0),
                        new Vector3Int(-2,  2, 0),
                        new Vector3Int(-2,  3, 0),
                        new Vector3Int(-3,  1, 0),
                        new Vector3Int(-3,  2, 0),
                        new Vector3Int(-3,  3, 0),
                        new Vector3Int( 1, -1, 0),    // BOTTOM-RIGHT
                        new Vector3Int( 1, -2, 0),
                        new Vector3Int( 1, -3, 0),
                        new Vector3Int( 2, -1, 0),
                        new Vector3Int( 2, -2, 0),
                        new Vector3Int( 2, -3, 0),
                        new Vector3Int( 3, -1, 0),
                        new Vector3Int( 3, -2, 0),
                        new Vector3Int( 3, -3, 0),
                        new Vector3Int(-1, -1, 0),   // BOTTOM-LEFT
                        new Vector3Int(-1, -2, 0),
                        new Vector3Int(-1, -3, 0),
                        new Vector3Int(-2, -1, 0),
                        new Vector3Int(-2, -2, 0),
                        new Vector3Int(-2, -3, 0),
                        new Vector3Int(-3, -1, 0),
                        new Vector3Int(-3, -2, 0),
                        new Vector3Int(-3, -3, 0)};

        private void Awake()
        {
            // Initialize the Unit Stats
            stats = new UnitStats(statData);

            // Set to no actions taken this turn
            movementLeft = MovementRange;
            hasCasted = false;

            // Cache local SpriteRenderer component
            spriteRenderer = GetComponent<SpriteRenderer>();
            ringSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
            bloodSplatter = transform.GetChild(1).gameObject;

            stoneTiles = FindObjectOfType<StoneTiles>();
        }

        /// <summary>
        /// Trigger behaviour on the start of the turn
        /// </summary>
        public virtual UniTask StartTurn()
        {
            // Set that the werewolf has not moved or attacked
            // Adjust movement based on whether or not the unit is standing on a stone tile
            if (stoneTiles != null && stoneTiles.IsStandingOnStoneTile(gridPosition))
            {
                movementLeft = MovementRange * 3;
            }
            else movementLeft = MovementRange;
            hasCasted = false;
            hasCurrentTurn = true;
            ringSprite.color = Color.white; // Default color

            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Trigger behaviour on the end of the turn
        /// </summary>
        public virtual UniTask EndTurn()
        {
            hasCurrentTurn = false;
            ringSprite.color = Color.black; // Inactive color

            // Update the stats mediator
            stats.UpdateModifiers();

            return UniTask.CompletedTask;
        }

        public abstract void AwaitCommands();

        /// <summary>
        /// Move to a specified grid position
        /// </summary>
        public async UniTask MoveThrough(GridManager gridManager, List<Vector3Int> path)
        {
            // Remove the unit from the grid cell
            gridManager.RemoveUnitAtGridCell(new Vector2Int(gridPosition.x, gridPosition.y));

            // Loop through the path
            for (int i = 0; i < path.Count; i++)
            {
                // If the player is moving, check for fog and remove it as the player moves
                RemoveFogNearPlayerUnit(gridManager);

                // Get the world position
                Vector3 worldPos = gridManager.GetWorldPositionFromGrid(path[i]);

                // Wait for the unit to move to the new position
                await transform.DOMove(worldPos, 0.33f).SetEase(Ease.Linear).ToUniTask();

                // Update the grid position
                gridPosition = path[i];

                // Skip the first movement (the zero node)
                if (i == 0) continue;

                movementLeft--;
            }

            // Set the unit at the grid cell
            gridManager.SetUnitAtGridCell(new Vector2Int(gridPosition.x, gridPosition.y), this);

            // If the player is moving, check for fog and remove it as the player moves
            RemoveFogNearPlayerUnit(gridManager);
        }

        /// <summary>
        /// Blink a battle unit across the grid
        /// </summary>
        public void Blink(Vector3Int targetPos)
        {
            // Move the unit instantly
            EventBus<MoveUnit>.Raise(new MoveUnit()
            {
                Unit = this,
                LastPosition = new Vector3Int(gridPosition.x, gridPosition.y),
                NewPosition = targetPos
            });

            // Set the grid position
            gridPosition = targetPos;
        }

        /// <summary>
        /// Deal damage to the unit
        /// </summary>
        public virtual void DealDamage(int damage)
        {
            // Subtract the damage from the health
            health -= damage;
            bloodSplatter.SetActive(true);

            // Notify that the health has changed
            EventBus<HealthChanged>.Raise(new HealthChanged()
            {
                Unit = this,
                CurrentHealth = health
            });

            // Check if the unit is dead
            CheckDeath();
        }

        /// <summary>
        /// Check if the unit is dead
        /// </summary>
        private void CheckDeath()
        {
            // Exit case - the unit is not dead
            if (health > 0) return;

            // Trigger the death behaviour
            OnDeath();
        }

        /// <summary>
        /// Trigger behaviour on death
        /// </summary>
        protected abstract void OnDeath();

        /// <summary>
        /// Update unit's sprite to face cardinal directions dependent on circumstance
        /// </summary>
        protected void UpdateCharacterSprite(Vector3 targetPos)
        {
            // Calculate direction vector
            Vector3 targetdir = (targetPos - this.transform.position).normalized;

            // Set sprite to face either front or back
            if (targetdir.y <= 0.2f) spriteRenderer.sprite = statData.frontFacingSprite;
            else if (targetdir.y > 0.2f) spriteRenderer.sprite = statData.backFacingSprite;

            // Set sprite to face either left or right
            if (targetdir.x >= 0) transform.localScale = new Vector3(-1, 1, 1);
            else if (targetdir.x < 0) transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Add a stat modifier to the stats mediator
        /// </summary>
        public void AddStatModifier(StatModifier modifier) => stats.AddModifier(modifier);

        /// <summary>
        /// Check cells within a three-ring range for fog and remove it.
        /// </summary>
        private void RemoveFogNearPlayerUnit(GridManager gridManager)
        {
            // Remove fog of war as player is moving through the level
            if (characterType == CharacterType.Vampire || characterType == CharacterType.Werewolf)
            {
                foreach (Vector2Int dir in fogRemovalDirs)
                {
                    Vector2Int d = (Vector2Int)gridPosition + dir;
                    gridManager.RemoveFogAtGridCell(d);
                }
            }
        }
    }
}
