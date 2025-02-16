using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine;

namespace Progress
{
    [CreateAssetMenu(menuName = "Progress/Farm Level Requirements")]
    public class FarmLevelsRequirements : ScriptableObject
    {
        [field: SerializeField, Tooltip("Farm Progress requirements, sorted in ascending order.")]
        public List<int> LevelRequirements { get; set; } = new List<int>();

        public int LevelCap => LevelRequirements.Count;

        public int GetRequirement(int farmLevelIndex)
        {
            return farmLevelIndex <= LevelRequirements.Count ? LevelRequirements[farmLevelIndex] : 0;
        }
    }
}
