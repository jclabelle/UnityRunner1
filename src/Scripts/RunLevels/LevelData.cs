using System;
using System.Collections.Generic;
using RunLevels.Interactables;
using UnityEngine;

namespace RunLevels
{
    [CreateAssetMenu]
    public class LevelData : ScriptableObject
    {
        public enum ECropType
        {
            Default,
            Oats,
        }

        [Serializable]
        public class LevelSectionData
        {
            [field:SerializeField] public GameObject Section { get; set; }
            [field:SerializeField] public Vector3 Position { get; set; }
            [field:SerializeField] public Vector3 Rotation { get; set; }

            public void CopyTo(LevelSectionData other)
            {
                other.Section = Section;
                other.Position = Position;
                other.Rotation = Rotation;
            }
        }

        [Serializable]
        public class ChoiceGateData : LevelSectionData
        {
            [field: SerializeField] public ChoiceGate.EEffectType EffectTypeRight { get; set; }
            [field: SerializeField] public float EffectRight { get; set; }
            [field: SerializeField] public ChoiceGate.EEffectType EffectTypeLeft { get; set; }
            [field: SerializeField] public float EffectLeft { get; set; }
        }
        
        [Serializable]
        public class AddScoreData : LevelSectionData
        {
            [field: SerializeField] public int ScoreAdded { get; set; }
            [field: SerializeField] public List<int> ScoresAdded { get; set; } = new List<int>();
        }
        
        [Serializable]
        public class RemoveScoreData : LevelSectionData
        {
            [field: SerializeField] public int ScoreRemoved { get; set; }
            [field: SerializeField] public List<int> ScoresRemoved { get; set; } = new List<int>();

        }
        
        [Serializable]
        public class MultiplyScoreData : LevelSectionData
        {
            [field: SerializeField] public float ScoreMultiplier { get; set; }
            [field: SerializeField] public List<float> ScoreMultipliers { get; set; } = new List<float>();
        }
        
        [Serializable]
        public class DivideScoreData : LevelSectionData
        {
            [field: SerializeField] public float ScoreDivisor { get; set; }
            [field: SerializeField] public List<float> ScoreDivisors { get; set; } = new List<float>();
        }
        
        
        [field:SerializeField] public ECropType CropType { get; set; }
        [field:SerializeField] public GameObject LevelSectionBase { get; set; }
        
        [field:SerializeField] public List<LevelSectionData> Sections { get; set; } = new List<LevelSectionData>();        
        [field:SerializeField] public List<ChoiceGateData> Gates { get; set; } = new List<ChoiceGateData>();
        [field:SerializeField] public List<AddScoreData> AddScores { get; set; } = new List<AddScoreData>();
        [field:SerializeField] public List<RemoveScoreData> RemoveScores { get; set; } = new List<RemoveScoreData>();
        [field:SerializeField] public List<MultiplyScoreData> MultiplyScores { get; set; } = new List<MultiplyScoreData>();
        [field:SerializeField] public List<DivideScoreData> DivideScores { get; set; } = new List<DivideScoreData>();


    }
}
