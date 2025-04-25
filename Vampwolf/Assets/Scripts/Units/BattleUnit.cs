using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Vampwolf.Grid;
using DG.Tweening;
using Vampwolf.EventBus;
using Vampwolf.Spells;
using Vampwolf.Units.Stats;

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
        }

        /// <summary>
        /// Trigger behaviour on the start of the turn
        /// </summary>
        public virtual UniTask StartTurn()
        {
            // Set that the werewolf has not moved or attacked
            movementLeft = MovementRange;
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
                // Get the world position
                Vector3 worldPos = gridManager.GetWorldPositionFromGrid(path[i]);

                // Wait for the unit to move to the new position
                await transform.DOMove(worldPos, 0.5f).SetEase(Ease.Linear).ToUniTask();

                // Update the grid position
                gridPosition = path[i];

                // Skip the first movement (the zero node)
                if (i == 0) continue;

                movementLeft--;
            }

            // Set the unit at the grid cell
            gridManager.SetUnitAtGridCell(new Vector2Int(gridPosition.x, gridPosition.y), this);
        }

        /// <summary>
        /// Deal damage to the unit
        /// </summary>
        public virtual void DealDamage(int damage)
        {
            // Subtract the damage from the health
            health -= damage;

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
    }
}
