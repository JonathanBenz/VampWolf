using UnityEngine;

namespace Vampwolf.Shop
{
    public enum UserType
    {
        Vampire,
        Werewolf
    }

    [CreateAssetMenu(fileName = "New Item", menuName = "Shop/Item Data")]
    public class ItemData : ScriptableObject
    {
        public string Name;
        [TextArea(5, 10)] public string Description;
        [TextArea(1, 3)] public string Flavor;
        public int Cost;
        public UserType User;
        public Sprite Icon;
    }
}
