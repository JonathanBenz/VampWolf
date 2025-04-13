using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vampwolf.Interfaces;

namespace Vampwolf.AI
{
    public class enemyController : MonoBehaviour, IActor, ITargetable, ITrackable
    {
        [SerializeField] EnemyDataSO enemyData;

        int currentHealth;
        int damage;
        SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            currentHealth = enemyData.Health;
            damage = enemyData.Damage;
            spriteRenderer.sprite = enemyData.sprite;
        }

        void Update()
        {

        }

        public void AddToInitiative()
        {
            throw new System.NotImplementedException();
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
        }
    }
}
