using UnityEngine;
using Vampwolf.EventBus;
using Vampwolf.Persistence;
using Vampwolf.Spells;

namespace Vampwolf
{
    public class VampireSaveHandler : SaveHandler, IBind<VampireData>
    {
        [SerializeField] private VampireData data;

        private EventBinding<DamageDealt> onDamageDealt;
        private EventBinding<DamageTaken> onDamageTaken;
        private EventBinding<DamageHealed> onDamageHealed;
        private EventBinding<SpellCast> onSpellCast;

        private void OnEnable()
        {
            onDamageDealt = new EventBinding<DamageDealt>(UpdateDamageDealt);
            EventBus<DamageDealt>.Register(onDamageDealt);

            onDamageTaken = new EventBinding<DamageTaken>(UpdateDamageTaken);
            EventBus<DamageTaken>.Register(onDamageTaken);

            onDamageHealed = new EventBinding<DamageHealed>(UpdateDamageHealed);
            EventBus<DamageHealed>.Register(onDamageHealed);

            onSpellCast = new EventBinding<SpellCast>(UpdateSpellCount);
            EventBus<SpellCast>.Register(onSpellCast);
        }

        private void OnDisable()
        {
            EventBus<DamageDealt>.Deregister(onDamageDealt);
            EventBus<DamageTaken>.Deregister(onDamageTaken);
            EventBus<DamageHealed>.Deregister(onDamageHealed);
            EventBus<SpellCast>.Deregister(onSpellCast);
        }

        /// <summary>
        /// Update how much damage was dealt to the Vampire
        /// </summary>
        private void UpdateDamageDealt(DamageDealt eventData)
        {
            // Exit case - the enemy is attacking
            if (eventData.CharacterType == CharacterType.Enemy) return;

            // Exit case - the werewolf's data is being updated
            if (eventData.CharacterType == CharacterType.Werewolf) return;

            // Update the data
            data.DamageDealt += eventData.Amount;

            // Save the game
            SaveLoadSystem.Instance.SaveGame();
        }

        /// <summary>
        /// Update how much damage was taken by the Vampire
        /// </summary>
        private void UpdateDamageTaken(DamageTaken eventData)
        {
            // Exit case - the enemy is attacking
            if (eventData.CharacterType == CharacterType.Enemy) return;

            // Exit case - the werewolf's data is being updated
            if (eventData.CharacterType == CharacterType.Werewolf) return;

            // Update the data
            data.DamageTaken += eventData.Amount;

            // Save the game
            SaveLoadSystem.Instance.SaveGame();
        }

        /// <summary>
        /// Update how much damage was healed by the Vampire
        /// </summary>
        private void UpdateDamageHealed(DamageHealed eventData)
        {
            // Exit case - the enemy is attacking
            if (eventData.CharacterType == CharacterType.Enemy) return;

            // Exit case - the werewolf's data is being updated
            if (eventData.CharacterType == CharacterType.Werewolf) return;

            // Update the data
            data.DamageHealed += eventData.Amount;

            // Save the game
            SaveLoadSystem.Instance.SaveGame();
        }

        /// <summary>
        /// Update how many times a spell was cast by the Vampire
        /// </summary>
        private void UpdateSpellCount(SpellCast eventData)
        {
            // Exit case - the enemy is attacking
            if (eventData.CharacterType == CharacterType.Enemy) return;

            // Exit case - the werewolf's data is being updated
            if (eventData.CharacterType == CharacterType.Werewolf) return;

            // Update the dictionary
            data.IncrementSpell(eventData.SpellName);

            // Save the game
            SaveLoadSystem.Instance.SaveGame();
        }

        /// <summary>
        /// Bind the player data for persistence
        /// </summary>
        public void Bind(VampireData data)
        {
            this.data = data;
            this.data.ID = ID;
        }
    }
}
