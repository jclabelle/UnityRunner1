using System;
using System.Collections;
using System.Collections.Generic;
using RunLevels;
using UnityEngine;


public class FarmRunDataBridge : MonoBehaviour
{
    [field:SerializeField] public LevelData NextLevel { get; set; }

    private void Awake()
    {
        if(FindObjectsOfType<FarmRunDataBridge>().Length == 1)
            DontDestroyOnLoad(this);
        else
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
}
