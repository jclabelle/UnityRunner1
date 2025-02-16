using System;
using System.Collections.Generic;
using UnityEngine;
using WorldPersistence;

namespace Progress
{
    public class FarmProgressionContributor : MonoBehaviour, IFarmProgress, IPersistentObject
    {
        [field: SerializeField, Tooltip("How much xp does this contribute to progression")]
        public int Contribution { get; set; } = 1;
        public bool ContributionSent { get; set; }

        // Start is called before the first frame update

        public void SendContribution()
        {
            Debug.Log(gameObject.name + ": Trying to send contribution");
            
            if (ContributionSent)
                return;
            
            (FindObjectOfType<PlayerProgress>() as IPlayerProgress).ContributeToProgression(Contribution);
            ContributionSent = true;
            

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public Dictionary<string, string> GetPersistentData()
        {
            Dictionary<string, string> pData = new Dictionary<string, string>();

            pData.Add($"{nameof(ContributionSent)}", Convert.ToString(ContributionSent));
            
            return pData;

        }

        public void SetPersistentData(Dictionary<string, string> pData)
        {
            ContributionSent = Convert.ToBoolean(pData[nameof(ContributionSent)]);
        }
    }
}
