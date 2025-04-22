using UnityEngine;

namespace Vampwolf.Spells
{
    public enum CharacterType
    {
        Vampire,
        Werewolf,
        Enemy
    }

    [CreateAssetMenu(fileName = "Spell Data", menuName = "Spells/Data")]
    public class SpellData : ScriptableObject
    {
        public CharacterType CharacterType;
        public string Name;
        [TextArea] public string Description;
        public float Cost;
        public int Range;
        public Sprite Icon;
        public SpellStrategy Strategy;
    }
}
