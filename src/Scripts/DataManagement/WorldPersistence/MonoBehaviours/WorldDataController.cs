using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using UnityEngine;
using WorldPersistence;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class WorldDataController : MonoBehaviour, IWorldData
{
    WorldData wData;
    Dictionary<string, GameObject> persistentPrefabsDB;
    string PATH_TO_PREFABS = "Prefabs/"; // Prefabs folder MUST be in Assets/Resources

    public WorldData WData { get => wData; set => wData = value; }

    private void Awake()
    {
        // DontDestroyOnLoad(this);

        WData = new WorldData();
        WData.Init();
        WData.Load();

        LoadGamePrefabs();

    }

    public void SaveAndCommit()
    {
        SaveScenePersistentData();
        CommitWorldData();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadScenePersistentDataV2();
    }
    
    void StartV1()
    {
        LoadScenePersistentData();
    }

    void LoadScenePersistentData()
    {

        // If we have already visited this scene, destroy existing Persistent prefabs objects and load from WorldData
        if (WData.IsSceneInWorldData(SceneManager.GetActiveScene().name) is true)
        {

            var allPersistent = FindObjectsOfType<PersistenceController>();
            foreach (var po in allPersistent)
            {
                po.DestroyParent();
            }

            var pObjectsList = WData.GetPObjectsInScene(SceneManager.GetActiveScene().name);
            foreach(var pObjectData in pObjectsList)
            {
                Dictionary<string, Dictionary<string, string>> deSerializedData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(pObjectData.SerializedData);

                var prefab = Instantiate(persistentPrefabsDB[deSerializedData["PersistenceController"]["prefabGuid"]]);

                var persistenceController = prefab.GetComponent<PersistenceController>();
                persistenceController.SetPersistentData(pObjectData);
       
            }
        }


    }
    
    void LoadScenePersistentDataV2()
    {

        // If we have already visited this scene, destroy existing Persistent prefabs objects and load from WorldData
        if (WData.IsSceneInWorldData(SceneManager.GetActiveScene().name) is true)
        {

            var allPersistent = FindObjectsOfType<PersistenceController>();
            foreach (var po in allPersistent)
            {
                po.DestroyParent();
            }

            var pObjectsList = WData.GetPObjectsInScene(SceneManager.GetActiveScene().name);
            foreach(var pObjectData in pObjectsList)
            {
                Dictionary<string, Dictionary<string, string>> deSerializedData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(pObjectData.SerializedData);

                //Debug.Log("Starting Persistent Object Instantiation");
                var prefab = Instantiate(persistentPrefabsDB[deSerializedData["PersistenceController"]["prefabGuid"]]);
                //Debug.Log("Finished Persistent Object Instantiation");

                var persistenceController = prefab.GetComponent<PersistenceController>();
                persistenceController.SetPersistentData(pObjectData);
       
            }
        }
        else // Take a snapshot of the scene's persistent objects.
        {
            SaveScenePersistentData();
            CommitWorldData();
        }

    }
    
    // Updates persistent object data on the datatables. Not a commit to disk. Use CommitWorldData to save the datatables to disk.
    public void SaveScenePersistentData()
    {

        var allPersistent = FindObjectsOfType<PersistenceController>();

        List<PObjectData> pObjectList = new List<PObjectData>();
        foreach(var po in allPersistent)
        {
            pObjectList.Add(po.GetPersistentData());
        }
        WData.SetPObjectsInScene(SceneManager.GetActiveScene().name, pObjectList);
        
    }

    // Commits the datatables to disk. Permanent.
    public void CommitWorldData()
    {
        WData.Save();
    }



    public Dictionary<string, GameObject> LoadGamePrefabs()
    {
        persistentPrefabsDB = new Dictionary<string, GameObject>();
        var prefabs = Resources.LoadAll<GameObject>(PATH_TO_PREFABS);

        foreach(var p in prefabs)
        {
            if(p.GetComponent<PersistenceController>() is PersistenceController pc)
            {
                persistentPrefabsDB.Add(pc.PrefabGuid, p);
            }
        }
        return persistentPrefabsDB;
    }



    void TestIn()
    {
        var outdata = WData.GetPObjectsInScene(SceneManager.GetActiveScene().name);
        foreach(var pobject in outdata)
        {
            Debug.Log(pobject.SerializedData);
        }

        PObjectData po = new PObjectData("64a671b2-7dc3-4512-928d-1de927a5e48b", "WAS SOMETHING SOMETHING");
        List<PObjectData> list = new List<PObjectData>();
        list.Add(po);
        WData.SetPObjectsInScene(SceneManager.GetActiveScene().name, list);
        outdata = WData.GetPObjectsInScene(SceneManager.GetActiveScene().name);
        foreach (var pobject in outdata)
        {
            Debug.Log(pobject.SerializedData);
        }
        WData.Save();

    }


    public GameObject GetPrefab(string prefabGuid)
    {
        if (persistentPrefabsDB.ContainsKey(prefabGuid))
            return persistentPrefabsDB[prefabGuid];

        return null;
    }

    public void RemovePersistentObject(string worldGuid)
    {
        WData.RemoveObjectFromWorldData(SceneManager.GetActiveScene().name, worldGuid);
    }

    public void AddPersistentObject(PObjectData pObject)
    {
        WData.AddObjectToWorldData(SceneManager.GetActiveScene().name, pObject);
    }

    public void PrintState()
    {
        using(var writer = new StringWriter())
        {
            WData.State.WriteXml(writer);
            Debug.Log(writer.ToString());
        }
    }
    
    public void PrintScenes()
    {
        using(var writer = new StringWriter())
        {
            WData.ScenesTBL.WriteXml(writer);
            Debug.Log(writer.ToString());
        }
    }
    
    public void PrintPObjects()
    {
        using(var writer = new StringWriter())
        {
            WData.PObjectsTBL.WriteXml(writer);
            Debug.Log(writer.ToString());
        }
    }
    
    
}

public interface IWorldData
{
    public GameObject GetPrefab(string prefabGuid);
    public void RemovePersistentObject(string worldGuid);
    public void AddPersistentObject(PObjectData pObject);
}