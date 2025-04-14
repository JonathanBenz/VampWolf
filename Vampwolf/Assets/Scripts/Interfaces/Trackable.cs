using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampwolf
{
    /// <summary>
    /// Abstract class to keep track of Trackable GameObjects (since you can't find objects by type with an interface)
    /// </summary>
    public abstract class Trackable : MonoBehaviour
    {
        public abstract int Initiative { get; }
        public abstract bool IsEnemy { get; }
        public abstract void RollForInitiative();
        public abstract void StartTurn();
    }
}
