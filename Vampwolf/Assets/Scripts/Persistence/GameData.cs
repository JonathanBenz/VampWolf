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
    public struct SpellCountPair
    {
        public string SpellName;
        public int Count;
    }

    [Serializable]
    public class VampireData : ISaveable, ISerializationCallbackReceiver
    {
        [field: SerializeField] public SerializableGuid ID { get; set; }
        [SerializeField] private List<SpellCountPair> serializedSpellCounts = new();
        private Dictionary<string, int> spellsCastedCount;
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

        /// <summary>
        /// Handle copying data before serialization
        /// </summary>
        public void OnBeforeSerialize()
        {
            // Clear the list
            serializedSpellCounts.Clear();

            // Iterate through the dictionary
            foreach (KeyValuePair<string, int> kvp in spellsCastedCount)
            {
                // Add the key-value pair to the list
                serializedSpellCounts.Add(new SpellCountPair
                {
                    SpellName = kvp.Key,
                    Count = kvp.Value
                });
            }
        }

        /// <summary>
        /// Handle data copying after deserialization
        /// </summary>
        public void OnAfterDeserialize()
        {
            // Create the dictionary
            spellsCastedCount = new Dictionary<string, int>();

            // Iterate through the list
            foreach (SpellCountPair pair in serializedSpellCounts)
            {
                // Set the data
                spellsCastedCount[pair.SpellName] = pair.Count;
            }
        }
    }

    [Serializable]
    public class WerewolfData : ISaveable, ISerializationCallbackReceiver
    {
        [field: SerializeField] public SerializableGuid ID { get; set; }
        [SerializeField] private List<SpellCountPair> serializedSpellCounts = new();
        private Dictionary<string, int> spellsCastedCount;
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

        /// <summary>
        /// Handle data copying before serialization
        /// </summary>
        public void OnBeforeSerialize()
        {
            // Clear the list
            serializedSpellCounts.Clear();

            // Iterate through the dictionary
            foreach (KeyValuePair<string, int> kvp in spellsCastedCount)
            {
                // Add the key-value pair to the list
                serializedSpellCounts.Add(new SpellCountPair
                {
                    SpellName = kvp.Key,
                    Count = kvp.Value
                });
            }
        }

        /// <summary>
        /// Handle data copying after deserialization
        /// </summary>
        public void OnAfterDeserialize()
        {
            // Create the dictionary
            spellsCastedCount = new Dictionary<string, int>();

            // Iterate through the list
            foreach (SpellCountPair pair in serializedSpellCounts)
            {
                // Set the data
                spellsCastedCount[pair.SpellName] = pair.Count;
            }
        }
    }
}
