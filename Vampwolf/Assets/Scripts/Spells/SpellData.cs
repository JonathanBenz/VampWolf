using UnityEngine;

namespace Vampwolf.Spells
{
    public enum CharacterType
    {
        Vampire,
        Werewolf,
        Enemy
    }

    public enum SpellType
    {
        Attack,
        Heal,
        Buff,
        Movement
    }

    [CreateAssetMenu(fileName = "Spell Data", menuName = "Spells/Data")]
    public class SpellData : ScriptableObject
    {
        public CharacterType CharacterType;
        public SpellType spellType;
        public string Name;
        [TextArea] public string Description;
        public float Cost;
        public int Range;
        public bool RequiresTarget;
        public Sprite Icon;
        public SpellStrategy Strategy;
    }
}
