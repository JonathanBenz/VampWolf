using System;
using UnityEngine;
using Vampwolf.Utilities.ObservableList;

namespace Vampwolf.Spells
{
    public class SpellsModel
    {
        private ObservableList<Spell> vampireSpells = new ObservableList<Spell>();
        private ObservableList<Spell> werewolfSpells = new ObservableList<Spell>();
        private float blood;
        private float rage;

        public event Action<float> BloodAmountChanged = delegate { };
        public event Action<float> RageAmountChanged = delegate { };

        public ObservableList<Spell> VampireSpells => vampireSpells;
        public ObservableList<Spell> WerewolfSpells => werewolfSpells;

        public float Blood
        {
            get => blood;
            set
            {
                // Clamp the Blood within the range
                blood = Mathf.Clamp(value, 0f, 100f);

                // Notify the change in Blood
                BloodAmountChanged.Invoke(blood);
            }
        }

        public float Rage
        {
            get => rage;
            set
            {
                // Clamp the Rage within the range
                rage = Mathf.Clamp(value, 0f, 100f);

                // Notify the change in rage
                RageAmountChanged.Invoke(rage);
            }
        }

        /// <summary>
        /// Add a Spell to the Vampire Spells
        /// </summary>
        public void Add(Spell spell)
        {
            // Check the character type
            switch (spell.CharacterType)
            {
                case CharacterType.Vampire:
                    // Exit case - if the spell is already in the list
                    if (vampireSpells.Contains(spell)) return;

                    // Add the spell to the Vampire Spells list
                    vampireSpells.Add(spell);
                    break;

                case CharacterType.Werewolf:
                    // Exit case - if the spell is already in the list
                    if (werewolfSpells.Contains(spell)) return;

                    // Add the spell to the Werewolf Spells list
                    werewolfSpells.Add(spell);
                    break;
            }
        }
    }
}
