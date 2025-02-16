using System;
using System.Collections.Generic;
using Progress;
using UI;
using UnityEngine;
using Utilities;
using WorldPersistence;
using Utilities;

namespace Farm
{
    [RequireComponent(typeof(UIButton))]
    public class LockedArea : MonoBehaviour, IClickable, ILockedFarmArea, IFarmLevelUp, IHide
    {
        [field: SerializeField, Tooltip("Minimum farm level for the area's Harvestables to be available for unlock")] public int FarmLevelRequirement { get; set; }
        private List<IAreaLock> LockedObjects { get; set; } = new List<IAreaLock>();
        private IPlayerProgress _PlayerProgress { get; set; }
        public bool IsUnlocked => FarmLevelRequirement <= _PlayerProgress.CurrentLevel;

        private void Awake()
        {
            _PlayerProgress = FindObjectOfType<PlayerProgress>();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (IsUnlocked)
                Hide();
            else
                _PlayerProgress.RegisterForLevelUp(this);
            
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Click()
        {
            throw new System.NotImplementedException();
        }

        public Color ButtonColor { get; }
        public string ButtonText { get; }

        public void RegisterLockedInstantiater(IAreaLock objLockedByArea)
        {

                LockedObjects.Add(objLockedByArea);
                objLockedByArea.farmArea = this;
        }
        
        public void DeregisterLockedInstantiater(IAreaLock objLockedByArea)
        {
                LockedObjects.Remove(objLockedByArea);
        }

        public void DoLevelUp()
        {
            var playerProgress = FindObjectOfType<PlayerProgress>() as IPlayerProgress;
            if (playerProgress.CurrentLevel == FarmLevelRequirement)
            {
                foreach (var lockedHarvestable in LockedObjects)
                {
                    if((lockedHarvestable as MonoBehaviour).IsValid())
                        lockedHarvestable.Show();
                }

                playerProgress.DeregisterForLevelUp(this);
                Hide();
            }

        }

        public void Hide()
        {
            GetComponent<MeshRenderer>().enabled = false; 
            GetComponent<BoxCollider>().enabled = false; 
            GetComponent<UIButton>().Hide();
            var childTransforms = GetComponentsInChildren<Transform>();
            foreach (var t in childTransforms)
            {
                t.gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collider: " + other);
            if (IsUnlocked is false &&
                other.gameObject.GetComponent<IAreaLock>() is { } locked
                && (locked as MonoBehaviour).IsValid())
            {
                locked.Hide();
                RegisterLockedInstantiater(locked);
            }
        }


       
    }

    public interface ILockedFarmArea
    {
        public void RegisterLockedInstantiater(IAreaLock objLockedByArea);
        public void DeregisterLockedInstantiater(IAreaLock objLockedByArea); 


    }

    public interface IAreaLock
    {
        public void Hide();
        public void Show();
        public LockedArea farmArea { get; set; }
    }
}
