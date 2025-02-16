using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WorldPersistence
{


    public class PersistenceController : MonoBehaviour, IPersistentController
    {
        [SerializeField] private string worldGuid;
        [SerializeField] private string prefabGuid;
        public bool IsFirstAwake { get; set; }
 

        public PersistenceController()
        {
            if (string.IsNullOrEmpty(WorldGuid))
            {
                WorldGuid = Guid.NewGuid().ToString();
                PrefabGuid = WorldGuid;
                IsFirstAwake = true;
            }
        }

        public string WorldGuid { get => worldGuid; set => worldGuid = value; }
        public string PrefabGuid { get => prefabGuid; set => prefabGuid = value; }

        private void Awake()
        {
            // Is this our first time waking up?
            if (WorldGuid == PrefabGuid)
            {
                FirstAwake();
            }
        }

        private void FirstAwake()
        {
            WorldGuid = Guid.NewGuid().ToString();
        }

 

        public void DestroyParent()
        {
            Destroy(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        public void RemoveSelfFromWorldData()
        {
            var worldDataController =  FindObjectOfType<WorldDataController>();
            worldDataController.RemovePersistentObject(worldGuid);
            worldDataController.CommitWorldData();

            
        }

        public void AddSelfToWorldData()
        {
            var persistentData =  GetPersistentData();
            
           var worldDataController =  FindObjectOfType<WorldDataController>();
           worldDataController.AddPersistentObject(persistentData);
           worldDataController.CommitWorldData();
        }

        public PObjectData GetPersistentData()
        {
            
            Dictionary<string, Dictionary<string, string>> persistentData = new Dictionary<string, Dictionary<string, string>>();

            persistentData.Add("transform", GetTransformData());

            var allComponents = gameObject.GetComponents<MonoBehaviour>();

            foreach(var component in allComponents)
            {
                if(component is IPersistentObject persistent)
                {
                    var componentPData = persistent.GetPersistentData();
                    persistentData.Add(Convert.ToString(component.GetType()), componentPData);

                }
            }

            persistentData.Add("PersistenceController", GetControllerData());

        

            string serializedData = JsonConvert.SerializeObject(persistentData, Formatting.Indented);

            return new PObjectData(WorldGuid, serializedData);
        }

        public Dictionary<string, string> GetControllerData()
        {
            Dictionary<string, string> pData = new Dictionary<string, string>();
            pData.Add($"{nameof(prefabGuid)}", Convert.ToString(prefabGuid));

            return pData;
        }

        public Dictionary<string, string> GetTransformData()
        {
            Dictionary<string, string> pData = new Dictionary<string, string>();
            pData.Add("position.x", Convert.ToString(transform.position.x));
            pData.Add("position.y", Convert.ToString(transform.position.y));
            pData.Add("position.z", Convert.ToString(transform.position.z));

            pData.Add("rotation.x", Convert.ToString(transform.rotation.x));
            pData.Add("rotation.y", Convert.ToString(transform.rotation.y));
            pData.Add("rotation.z", Convert.ToString(transform.rotation.z));

            pData.Add("localScale.x", Convert.ToString(transform.localScale.x));
            pData.Add("localScale.y", Convert.ToString(transform.localScale.y));
            pData.Add("localScale.z", Convert.ToString(transform.localScale.z));


            return pData;

        }

        public void SetPersistentData(PObjectData pObjectData)
        {
            WorldGuid = pObjectData.GUID;

            var serializedData = pObjectData.SerializedData;
            Dictionary<string, Dictionary<string, string>> deSerializedData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(serializedData);
            SetTransformData(deSerializedData["transform"]);

            var allComponents = gameObject.GetComponents<MonoBehaviour>();

            foreach (var component in allComponents)
            {
                if (component is IPersistentObject persistent)
                {
                    persistent.SetPersistentData(deSerializedData[Convert.ToString(component.GetType())]);
                }
            }
        }

  

        public void SetTransformData(Dictionary<string, string> transformData)
        {
            float x = Convert.ToSingle(transformData["position.x"]);
            float y = Convert.ToSingle(transformData["position.y"]);
            float z = Convert.ToSingle(transformData["position.z"]);
            gameObject.transform.position = new Vector3( x, y, z );

            x = Convert.ToSingle(transformData["rotation.x"]);
            y = Convert.ToSingle(transformData["rotation.y"]);
            z = Convert.ToSingle(transformData["rotation.z"]);
            gameObject.transform.rotation = Quaternion.Euler(x, y, z);


            x = Convert.ToSingle(transformData["localScale.x"]);
            y = Convert.ToSingle(transformData["localScale.y"]);
            z = Convert.ToSingle(transformData["localScale.z"]);
            gameObject.transform.localScale= new Vector3(x, y, z);

        }
    }

public interface IPersistentController
{
    public bool IsFirstAwake { get; set; }
    public void RemoveSelfFromWorldData();
    public void AddSelfToWorldData();
}

public interface IPersistentObject
{
    public Dictionary<string, string> GetPersistentData();
    public void SetPersistentData(Dictionary<string, string> pData);

}

}


