using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RunLevels;
using RunLevels.Interactables;
using UnityEngine;
using Utilities;

public class LevelDataLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoInstantiate());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DoInstantiate()
    {
        yield return null;
        
        var bridge = FindObjectsOfType<FarmRunDataBridge>().First(bridge => bridge.IsValid());
        
        Debug.Log("Loading " + bridge.NextLevel.name);

        foreach (var sectionData in bridge.NextLevel.Sections)
        {
            var section = LevelSectionInstantiator.InstantiateSection(sectionData.Section, sectionData.Position, sectionData.Rotation);
            
          
        }
        
        foreach (var gateData in bridge.NextLevel.Gates)
        {
            var section = LevelSectionInstantiator.InstantiateSection(gateData.Section, gateData.Position, gateData.Rotation);

            var choiceGates = section.GetComponentsInChildren<ChoiceGate>();
            var rightGate =
                (from gate in choiceGates
                    where gate.name == "RightGate"
                    select gate).First();
            var leftGate =
                (from gate in choiceGates
                    where gate.name == "LeftGate"
                    select gate).First();
                
            rightGate.Effect = gateData.EffectRight;
            rightGate.EffectType = gateData.EffectTypeRight;
                
            leftGate.Effect = gateData.EffectLeft;
            leftGate.EffectType = gateData.EffectTypeLeft;
                
            rightGate.SetMaterials();
            rightGate.AddScoringEffect();
                
            leftGate.SetMaterials();
            leftGate.AddScoringEffect();
        }

        foreach (var addScoreData in bridge.NextLevel.AddScores)
        {
            var section = LevelSectionInstantiator.InstantiateSection(addScoreData.Section, addScoreData.Position, addScoreData.Rotation);
            var addScores = section.GetComponentsInChildren<AddScore>();
            int index = 0;
            foreach (var addScore in addScores)
            {
                if(index >= addScores.Length)
                    break;
                if (index >= addScoreData.ScoresAdded.Count)
                    break;

                addScores[index].ScoreAdded = addScoreData.ScoresAdded[index];
                index++;
            }
        }
        
        foreach (var removeScoreData in bridge.NextLevel.RemoveScores)
        {
            var section = LevelSectionInstantiator.InstantiateSection(removeScoreData.Section, removeScoreData.Position, removeScoreData.Rotation);
            var removeScores = section.GetComponentsInChildren<RemoveScore>();
            int index = 0;
            foreach (var addScore in removeScores)
            {
                if(index >= removeScores.Length)
                    break;
                if (index >= removeScoreData.ScoresRemoved.Count)
                    break;

                removeScores[index].ScoreRemoved = removeScoreData.ScoresRemoved[index];
                index++;
            }
        }
        
        foreach (var multiplyScoreData in bridge.NextLevel.MultiplyScores)
        {
            var section = LevelSectionInstantiator.InstantiateSection(multiplyScoreData.Section, multiplyScoreData.Position, multiplyScoreData.Rotation);
            var multiplyScores = section.GetComponentsInChildren<MultiplyScore>();
            int index = 0;
            foreach (var addScore in multiplyScores)
            {
                if(index >= multiplyScores.Length)
                    break;
                if (index >= multiplyScoreData.ScoreMultipliers.Count)
                    break;

                multiplyScores[index].ScoreMultiplier = multiplyScoreData.ScoreMultipliers[index];
                index++;
            }
        }
        
        foreach (var divideScoreData in bridge.NextLevel.DivideScores)
        {
            var section = LevelSectionInstantiator.InstantiateSection(divideScoreData.Section, divideScoreData.Position, divideScoreData.Rotation);
            var divideScores = section.GetComponentsInChildren<DivideScore>();
            int index = 0;
            foreach (var addScore in divideScores)
            {
                if(index >= divideScores.Length)
                    break;
                if (index >= divideScoreData.ScoreDivisors.Count)
                    break;

                divideScores[index].ScoreDivisor = divideScoreData.ScoreDivisors[index];
                index++;
            }
        }
    }
    
}
