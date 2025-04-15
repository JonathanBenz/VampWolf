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
            spriteRenderer.sprite = enemyData.sprite;
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
            EventBus<EnemyDeathEvent>.Raise(new EnemyDeathEvent() { });
        }
    }
}
