using UnityEngine;

namespace Progress
{
    public class FarmProgressionContributorNotPersistent : MonoBehaviour, IFarmProgress
    {
        [field: SerializeField, Tooltip("How much xp does this contribute to progression")]
        public int Contribution { get; set; } = 1;
        public bool ContributionSent { get; set; }

        public void SendContribution()
        {
            if (ContributionSent)
                return;
            
            (FindObjectOfType<PlayerProgress>() as IPlayerProgress).ContributeToProgression(Contribution);
            ContributionSent = true;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
