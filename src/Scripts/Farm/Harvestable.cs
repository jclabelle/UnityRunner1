using System;
using System.Collections.Generic;
using Progress;
using RunLevels;
using UnityEngine;
using Utilities;
using WorldPersistence;

namespace Farm
{
    // Anything that can trigger a harvest run: fields, barns, and so on.
    public class Harvestable : MonoBehaviour, IHarvestable, IPersistentObject
    {
        private static float DefaultCoolDownTime { get; set; } = 60; 
        private static float DefaultHarvestBonus { get; set; } = 1f; 
        private Utilities.ITimer CooldownTimer { get; set; }
        [field: SerializeField, Tooltip("Duration of the Harvestable cooldown, in seconds.")] public float CooldownTime { get; set; } = DefaultCoolDownTime;
        public bool IsCooldownComplete { get; set; }
        public float CooldownTimeLeft => CooldownTime - CooldownTimer.ElapsedTime;
        [field: SerializeField] public LevelData.ECropType CropType { get; set; }

        [field: SerializeField, Tooltip("Bonus multiplier applied to score of harvest runs triggered by this Harvestable.")] public float HarvestBonus { get; set; } = DefaultHarvestBonus;


        public void ForceCooldownComplete()
        {
            ForceReadyForHarvest();
            CooldownTimer.SetState(ITimer.EState.Stopped);
        }

        private void Awake()
        {
            CooldownTimer = GetComponent<ITimer>();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (GetComponent<PersistenceController>().IsFirstAwake)
            {
                CooldownTimer.SetState(ITimer.EState.Running);
            }
        }

        // Update is called once per frame
        void Update()
        {
            CheckIfReadyForHarvest();
        }
        
        public void StartCooldownTimer()
        {
            CooldownTimer.SetState(ITimer.EState.Running);
            IsCooldownComplete = false;
        }
        private void CheckIfReadyForHarvest()
        {
            if (CooldownTimeLeft <= 0)
            {
                IsCooldownComplete = true;
                CooldownTimer.SetState(ITimer.EState.Stopped);

            }
        }
        
        public void ForceReadyForHarvest() => IsCooldownComplete = true;

        
        public Dictionary<string, string> GetPersistentData()
        {
            Dictionary<string, string> pData = new Dictionary<string, string>();

            pData.Add($"{nameof(CooldownTime)}", Convert.ToString(CooldownTime));
            pData.Add($"{nameof(HarvestBonus)}", Convert.ToString(HarvestBonus));

            return pData;


        }
        public void SetPersistentData(Dictionary<string, string> pData)
        {
            CooldownTime = (float)Convert.ToDouble(pData[nameof(CooldownTime)]);
            HarvestBonus = (float)Convert.ToDouble(pData[nameof(HarvestBonus)]);

        }

    }

    public interface IHarvestable
    {
        public bool IsCooldownComplete { get;  }
        public float CooldownTimeLeft { get; }
        public void StartCooldownTimer();
        public float HarvestBonus { get; }
        public void ForceCooldownComplete();
        public LevelData.ECropType CropType { get; }


    }
}
