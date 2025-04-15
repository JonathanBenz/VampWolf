using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampwolf.AI
{
    public enum EnemyType { melee, ranged }

    [CreateAssetMenu(fileName = "New Enemy Type", menuName = "Create Enemy")]
    public class EnemyDataSO : ScriptableObject
    {
        public EnemyType enemyType;
        public int Health;
        public int Damage;
        public int MovementRange;
        public int AttackRange;
        public Sprite[] sprites;
    }
}
