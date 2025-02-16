using System;
using FluffyUnderware.DevTools.Extensions;
using TMPro;
using UnityEngine;

namespace RunLevels.Interactables
{
    public class ChoiceGate : MonoBehaviour, ILevelSection
    {
        public enum EEffectType
        {
            Add,
            Substract,
            Multiply,
            Divide
        }
    
        [field: SerializeField] public EEffectType EffectType { get; set; }
        [field: SerializeField] public float Effect { get; set; }
        [field: SerializeField] public Material GoodMaterial { get; set; }
        [field: SerializeField] public Material BadMaterial { get; set; }
    
        // Start is called before the first frame update
        void Start()
        {
            SetMaterials();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void OnValidate()
        {
            
            SetMaterials();
            AddScoringEffect();
        }

        public void SetMaterials()
        {
                        
            if(GoodMaterial  
               && EffectType is EEffectType.Add or EEffectType.Multiply 
               && GetComponent<MeshRenderer>() is {} goodGateRenderer)
                goodGateRenderer.material = GoodMaterial;
            
            if(BadMaterial 
               && EffectType is EEffectType.Substract or EEffectType.Divide
               && GetComponent<MeshRenderer>() is {} badGateRenderer)
                badGateRenderer.material = BadMaterial;
        }

        public void AddScoringEffect()
        {
            if(GetComponent<AddScore>() is {} addScore)
                addScore.enabled = false;
            
            if(GetComponent<RemoveScore>() is {} removeScore)
                removeScore.enabled = false;
            
            if(GetComponent<MultiplyScore>() is {} multiplyScore)
                multiplyScore.enabled = false;
            
            if(GetComponent<DivideScore>() is {} divideScore)
                divideScore.enabled = false;

            switch (EffectType)
            {
                case EEffectType.Add:
                    if (GetComponent<AddScore>() is { } add)
                    {
                        add.enabled = true;
                        add.ScoreAdded = (int)Effect;
                        GetComponentInChildren<TextMeshPro>().text = "+" + Effect.ToString();
                    }
                    break;
                case EEffectType.Substract:
                    if (GetComponent<RemoveScore>() is { } sub)
                    {
                        sub.enabled = true;
                        sub.ScoreRemoved = (int)Effect;
                        GetComponentInChildren<TextMeshPro>().text = "-" + Effect.ToString();
                    }
                    break;
                case EEffectType.Multiply:
                    if (GetComponent<MultiplyScore>() is { } mul)
                    {
                        mul.enabled = true;
                        mul.ScoreMultiplier = Effect;
                        GetComponentInChildren<TextMeshPro>().text = "×" + Effect.ToString();

                    };
                    break;
                case EEffectType.Divide:
                    if (GetComponent<DivideScore>() is { } div)
                    {
                        div.enabled = true;
                        div.ScoreDivisor = Effect;
                        GetComponentInChildren<TextMeshPro>().text = "÷" + Effect.ToString();

                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
