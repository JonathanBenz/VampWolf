using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampwolf.Interfaces
{
    public interface IActor 
    {
        void Move(Vector2 targetPos);
        void Cast();
        void Die();
        void TakeDamage(int dmg);
    }
}
