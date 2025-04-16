using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vampwolf.Interfaces;
using Vampwolf.EventBus;
using Vampwolf.Events;

namespace Vampwolf.AI
{
    public class enemyController : Trackable, IActor, ITargetable
    {
        [SerializeField] EnemyDataSO enemyData;

        int currentHealth;
        int initiative;
        int damage;
        int movementRange;
        int attackRange;
        SpriteRenderer spriteRenderer;
        Vector2 closestTarget;

        public override int Initiative { get { return initiative; } }
        public override bool IsEnemy => true;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            currentHealth = enemyData.Health;
            damage = enemyData.Damage;
            movementRange = enemyData.MovementRange;
            attackRange = enemyData.AttackRange;
            spriteRenderer.sprite = enemyData.sprites[0]; // Front-facing Sprite

            // --- DEBUGGING Purposes ---
            closestTarget = FindObjectOfType<PlayerController>().transform.position;
        }

        private void Update()
        {
            UpdateCharacterSprite();
        }

        private void UpdateCharacterSprite()
        {
            Vector2 targetdir = (closestTarget - (Vector2)this.transform.position).normalized;

            if (targetdir.y >= 0) spriteRenderer.sprite = enemyData.sprites[1];
            else if (targetdir.y < 0) spriteRenderer.sprite = enemyData.sprites[0];

            if (targetdir.x >= 0) transform.localScale = Vector3.one;
            else if (targetdir.x < 0) transform.localScale = new Vector3(-1, 1, 1);
        }
        public override void StartTurn()
        {
            // Perform all logic, then end turn
            // TODO: Search for closest player, move toward to and attack them.
            EventBus<TurnEndedEvent>.Raise(new TurnEndedEvent() { });
            Debug.Log($"{this.gameObject.name} has ended their turn!");
        }

        public override void RollForInitiative()
        {
            initiative = Random.Range(1, 21); //D20 dice roll
        }

        public void Move(Vector2 targetPos)
        {
            throw new System.NotImplementedException();
        }

        public void Cast()
        {
            throw new System.NotImplementedException();
        }

        public void TakeDamage(int dmg)
        {
            currentHealth -= dmg;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
        }

        public void Die()
        {
            Debug.Log($"{this.gameObject.name} has lost all HP and is now dead!");
            spriteRenderer.sprite = enemyData.sprites[2]; // Death Sprite
            EventBus<EnemyDeathEvent>.Raise(new EnemyDeathEvent() { });
        }
    }
}
