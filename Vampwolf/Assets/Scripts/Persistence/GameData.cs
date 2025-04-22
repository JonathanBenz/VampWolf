using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vampwolf.Persistence
{
    [Serializable]
    public class GameData
    {
        public string Name;
        public VampireData Vampire;
        public WerewolfData Werewolf;
    }

    [Serializable]
    public class VampireData : ISaveable
    {
        [field: SerializeField] public SerializableGuid ID { get; set; }
        [SerializeField] private Dictionary<string, int> spellsCastedCount;
        public int DamageTaken;
        public int DamageDealt;
        public int DamageHealed;

        public VampireData()
        {
            spellsCastedCount = new Dictionary<string, int>();
            DamageTaken = 0;
            DamageDealt = 0;
            DamageHealed = 0;
        }

        /// <summary>
        /// Increase the cast count for a given spell name.
        /// </summary>
        public void IncrementSpell(string spellName)
        {
            // Try to get a current value
            if (spellsCastedCount.TryGetValue(spellName, out int count))
                // Increment the current value
                spellsCastedCount[spellName] = count + 1;
            else
                // Otherwise, set it
                spellsCastedCount[spellName] = 1;
        }
    }

    [Serializable]
    public class WerewolfData : ISaveable
    {
        [field: SerializeField] public SerializableGuid ID { get; set; }
        [SerializeField] private Dictionary<string, int> spellsCastedCount;
        public int DamageTaken;
        public int DamageDealt;
        public int DamageHealed;

        public WerewolfData()
        {
            spellsCastedCount = new Dictionary<string, int>();
            DamageTaken = 0;
            DamageDealt = 0;
            DamageHealed = 0;
        }

        /// <summary>
        /// Increase the cast count for a given spell name.
        /// </summary>
        public void IncrementSpell(string spellName)
        {
            // Try to get a current value
            if (spellsCastedCount.TryGetValue(spellName, out int count))
                // Increment the current value
                spellsCastedCount[spellName] = count + 1;
            else
                // Otherwise, set it
                spellsCastedCount[spellName] = 1;
        }
    }
}
