using System.Collections.Generic;
using System.Linq;
using CustomTools;
using RunLevels;
using RunLevels.Interactables;
using RunLevels.Sections;
using UnityEditor;
using UnityEngine;

namespace CustomTools
{
    public class SaveLevelLayout : EditorWindow
    {
        
        [MenuItem("Window/My Tools/Show Save Level Layout")]
        public static void Enable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }
 
        [MenuItem("Window/My Tools/Hide Save Level Layout")]
        public static void Disable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }
 
        private static void OnSceneGUI(SceneView sceneView)
        {
            Handles.BeginGUI();

            if (GUILayout.Button("Save Layout"))
            {
                Debug.Log("Saving Level Layout");
                SaveLayout();
                Debug.Log("Level Layout Saved");

            }
 
            Handles.EndGUI();
        }

        private static void SaveLayout()
        {
            var allLevelSections = FindObjectsOfType<LevelSectionV2>();
            var levelData = ScriptableObject.CreateInstance<LevelData>();

            foreach (var section in allLevelSections)
            {
                var sectionData = new LevelData.LevelSectionData();
                sectionData.Section = PrefabUtility.GetCorrespondingObjectFromOriginalSource(section.gameObject);
                sectionData.Position = section.transform.position;
                sectionData.Rotation = section.transform.rotation.eulerAngles;
                
                if (section.GetComponentsInChildren<ChoiceGate>() is { } choiceGates &&
                    choiceGates.Length > 0)
                {
                    var choiceGateData = new LevelData.ChoiceGateData();
                    sectionData.CopyTo(choiceGateData);
                    
                    var rightGate =
                        (from gate in choiceGates
                            where gate.name == "RightGate"
                            select gate).First();
                    var leftGate =
                        (from gate in choiceGates
                            where gate.name == "LeftGate"
                            select gate).First();
                    
                    choiceGateData.EffectRight = rightGate.Effect;
                    choiceGateData.EffectTypeRight = rightGate.EffectType;
                    
                    choiceGateData.EffectLeft = leftGate.Effect;
                    choiceGateData.EffectTypeLeft = leftGate.EffectType;
                    
                    levelData.Gates.Add(choiceGateData);
                }
                else if(section.GetComponentsInChildren<AddScore>() is { } addScores
                        && addScores.Length > 0)
                {
                    var addScoreData = new LevelData.AddScoreData();
                    sectionData.CopyTo(addScoreData);
                    
                    foreach (var addScore in addScores)
                    {
                        addScoreData.ScoresAdded.Add(addScore.ScoreAdded);
                    }
                    levelData.AddScores.Add(addScoreData);
                }
                else if(section.GetComponentsInChildren<RemoveScore>() is { } removeScores
                        && removeScores.Length > 0)
                {
                    var removeScoreData = new LevelData.RemoveScoreData();
                    sectionData.CopyTo(removeScoreData);
                    foreach (var removeScore in removeScores)
                    {
                        removeScoreData.ScoresRemoved.Add(removeScore.ScoreRemoved);
                    }
                    levelData.RemoveScores.Add(removeScoreData);
                }
                else if(section.GetComponentsInChildren<MultiplyScore>() is { } multiplyScores
                        && multiplyScores.Length > 0)
                {
                    var multiplyScoreData = new LevelData.MultiplyScoreData();
                    sectionData.CopyTo(multiplyScoreData);
                    foreach (var multiplyScore in multiplyScores)
                    {
                        multiplyScoreData.ScoreMultipliers.Add(multiplyScore.ScoreMultiplier);
                    }
                    levelData.MultiplyScores.Add(multiplyScoreData);
                }
                else if(section.GetComponentsInChildren<DivideScore>() is { } divideScores
                        && divideScores.Length > 0)
                {
                    var divideScoreData = new LevelData.DivideScoreData();
                    sectionData.CopyTo(divideScoreData);
                    foreach (var divideScore in divideScores)
                    {
                        divideScoreData.ScoreDivisors.Add(divideScore.ScoreDivisor);
                    }
                    levelData.DivideScores.Add(divideScoreData);
                }
                else
                {
                    levelData.Sections.Add(sectionData);
                }
                
            }

            levelData.LevelSectionBase = AssetTools.GetAssetByName<GameObject>("EmptyLevelSection");
            
            var uniqueName = AssetDatabase.GenerateUniqueAssetPath("Assets/Levels/Level.asset");
            AssetDatabase.CreateAsset(levelData, uniqueName);
            
        }

        private static void SaveLayoutV2()
        {
            var sections = new List<LevelData.LevelSectionData>();
            var allLevelSections = FindObjectsOfType<LevelSectionV2>();
            var levelData = ScriptableObject.CreateInstance<LevelData>();

            foreach (var section in allLevelSections)
            {
                var sectionData = new LevelData.LevelSectionData();
                sectionData.Section = PrefabUtility.GetCorrespondingObjectFromOriginalSource(section.gameObject);
                sectionData.Position = section.transform.position;
                sectionData.Rotation = section.transform.rotation.eulerAngles;
                
                if (section.GetComponentsInChildren<ChoiceGate>() is { } choiceGates &&
                    choiceGates.Length > 0)
                {
                    var choiceGateData = new LevelData.ChoiceGateData();
                    sectionData.CopyTo(choiceGateData);
                    
                    var rightGate =
                        (from gate in choiceGates
                        where gate.name == "RightGate"
                        select gate).First();
                    var leftGate =
                        (from gate in choiceGates
                        where gate.name == "LeftGate"
                        select gate).First();
                    
                    choiceGateData.EffectRight = rightGate.Effect;
                    choiceGateData.EffectTypeRight = rightGate.EffectType;
                    
                    choiceGateData.EffectLeft = leftGate.Effect;
                    choiceGateData.EffectTypeLeft = leftGate.EffectType;
                    
                    levelData.Gates.Add(choiceGateData);

                }
            }
            
            levelData.LevelSectionBase = AssetTools.GetAssetByName<GameObject>("EmptyLevelSection");
            
            var uniqueName = AssetDatabase.GenerateUniqueAssetPath("Assets/Levels/Level.asset");
            AssetDatabase.CreateAsset(levelData, uniqueName);
        }

    }
}
