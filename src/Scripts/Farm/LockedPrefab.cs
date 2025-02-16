using Progress;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using UI;
using Unity.Mathematics;
using Unity.VisualScripting;
using Utilities;
using WorldPersistence;

namespace Farm
{
    public class LockedPrefab : MonoBehaviour, IIstantiater, ICost, IClickable, IPersistentObject, IHide, IShow, IAreaLock
    {
        [field: SerializeField, Tooltip("Assign from Resources/Prefab, NOT directly from the scene")] private GameObject UnlockedPrefab { get; set; }

        [field: SerializeField, Tooltip("The cost of unlocking this object")] public int Cost { get; set;} = 0;
        
        [field: SerializeField] public Color ButtonColor { get; set; } = Color.white;
        [field: SerializeField] public string ButtonText { get;set; } = String.Empty;
        [field: SerializeField] public Vector3 Rotation { get;set; } = Vector3.zero;
        public LockedArea farmArea { get; set; }
        
        private bool IsUnlocked { get; set; }
        
        private List<GameObject> _ChildObjects { get; set; }

        public bool CanAfford()
        {
            var playerDollars = FindObjectOfType<PlayerProgress>() as IPlayerDollars;
            return Cost <= playerDollars.CurrentDollars;
        }


        public bool RequestInstantiate()
        {
            if (CanAfford() is false)
            {
                Debug.Log("Cannot afford purchase!");
                return false;
            }
           
            var playerDollars = FindObjectOfType<PlayerProgress>() as IPlayerDollars;
            playerDollars.RemoveDollars(Cost);
            
            var finalPosition = new Vector3(transform.position.x, UnlockedPrefab.transform.position.y, transform.position.z) ;
            var prefab = Instantiate(UnlockedPrefab, finalPosition, transform.rotation);
            prefab.transform.Rotate(Rotation, Space.World);
            
            prefab.GetComponent<PersistenceController>().AddSelfToWorldData();
            
            GetComponent<IFarmProgress>().SendContribution();
            
            GetComponent<IPersistentController>().RemoveSelfFromWorldData();
            
            if(farmArea is not null)
                farmArea.DeregisterLockedInstantiater(this);
            
            Destroy(gameObject);

            return true;
        }

        public void Click()
        {
            RequestInstantiate();
        }

        private void Awake()
        {
            ButtonText = "Unlock! " + "$" + Cost.ToString();
            _ChildObjects = new List<GameObject>();
            var childTransforms = GetComponentsInChildren<Transform>();
            foreach (var childTransform in childTransforms)
                _ChildObjects.Add(childTransform.gameObject);

        }

        // Start is called before the first frame update
        void Start()
        {
            if (CanAfford())
                ButtonColor = Color.cyan;
            else
                ButtonColor = Color.gray;
        }

        // Update is called once per frame
        void Update()
        {
            if (CanAfford())
                ButtonColor = Color.cyan;
            else
                ButtonColor = Color.gray;
        }


        public Dictionary<string, string> GetPersistentData()
        {
            Dictionary<string, string> pData = new Dictionary<string, string>();

            pData.Add($"{nameof(IsUnlocked)}", Convert.ToString(IsUnlocked));
            pData.Add($"{nameof(Cost)}", Convert.ToString(Cost));
            pData.Add($"RotationX", Convert.ToString(Rotation.x));
            pData.Add($"RotationY", Convert.ToString(Rotation.y));
            pData.Add($"RotationZ", Convert.ToString(Rotation.z));

            pData.Add($"{nameof(UnlockedPrefab)}", Convert.ToString(UnlockedPrefab.GetComponent<PersistenceController>().PrefabGuid));

            return pData;
        }

        public void SetPersistentData(Dictionary<string, string> pData)
        {
            if (Convert.ToBoolean(pData[nameof(IsUnlocked)]))
            {
                Destroy(gameObject);
                return;
            }
            
            Cost = (int)Convert.ToDouble(pData[nameof(Cost)]);
            
            ButtonText = "Unlock! " + "$" + Cost.ToString();

            var rotationX = (float)Convert.ToDouble(pData["RotationX"]);
            var rotationY = (float)Convert.ToDouble(pData["RotationY"]);
            var rotationZ = (float)Convert.ToDouble(pData["RotationZ"]);
            Rotation = new Vector3(rotationX, rotationY, rotationZ);
            
            var prefabGuid = Convert.ToString(pData[nameof(UnlockedPrefab)]);
            UnlockedPrefab = FindObjectOfType<WorldDataController>().GetPrefab(prefabGuid);

            if (Convert.ToBoolean(pData[nameof(IsUnlocked)]))
                Destroy(gameObject);

        }

        public void Hide()
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Light>().enabled = false;
            GetComponent<UIButton>().Hide();

            foreach (var childObject in _ChildObjects)
                childObject.gameObject.SetActive(false);


        }

        public void Show()
        {
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<Light>().enabled = true;
            GetComponent<UIButton>().Show();
            
            foreach (var childObject in _ChildObjects)
                childObject.gameObject.SetActive(true);
        }
    }



    public interface IIstantiater
    {
        public bool RequestInstantiate();
    }

    public interface ICost
    {
        public int Cost { get; }
        public bool CanAfford();
    }
}
