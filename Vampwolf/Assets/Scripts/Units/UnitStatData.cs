using UnityEngine;
using UnityEngine.UI;

namespace Vampwolf.Units
{
    [CreateAssetMenu(fileName = "Unit Stats", menuName = "Units/Stats")]
    public class UnitStatData : ScriptableObject
    {
        [Header("General")]
        public int minStatValue;
        public int maxStatValue;

        [Header("Stat Values")]
        public int Might;
        public int Fortitude;
        public int Agility;

        [Header("Agility")]
        public int minMovementRange;
        public int maxMovementRange;

        [Header("Sprites")]
        public Sprite frontFacingSprite;
        public Sprite backFacingSprite;
        public Sprite deathSprite;
        public Sprite frame;
        public Sprite portrait;
    }
}
