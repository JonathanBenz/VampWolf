using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampwolf
{
    public class ResetProgress : MonoBehaviour
    {
        /// <summary>
        /// When the Lose Screen is loaded, reset the player's progress
        /// </summary>
        void Start()
        {
            ProgressTracker.Instance.level1Complete = false;
            ProgressTracker.Instance.level2Complete = false;
        }
    }
}
