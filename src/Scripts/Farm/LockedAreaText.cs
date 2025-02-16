using System;
using System.Collections;
using System.Collections.Generic;
using Farm;
using TMPro;
using UnityEngine;

public class LockedAreaText : MonoBehaviour
{
    private TextMeshPro _Text { get; set; }
    private LockedArea _LockedArea { get; set; }

    private void Awake()
    {
        _Text = GetComponent<TextMeshPro>();
        _LockedArea = GetComponentInParent<LockedArea>();

    }

    // Start is called before the first frame update
    void Start()
    {
        _Text.text = "Unlock at Level " + _LockedArea.FarmLevelRequirement.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
