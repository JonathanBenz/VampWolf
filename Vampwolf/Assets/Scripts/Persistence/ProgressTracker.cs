using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vampwolf.Utilities.Singletons;

namespace Vampwolf
{
    /// <summary>
    /// Tracks what levels the player has successfully completed
    /// </summary>
    public class ProgressTracker : PersistentSingleton<ProgressTracker>
    {
        public bool level1Complete;
        public bool level2Complete;
        public bool level3Complete;
    }
}
