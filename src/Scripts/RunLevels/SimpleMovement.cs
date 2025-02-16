using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SimpleMovement : MonoBehaviour
{
    [field: SerializeField] public Vector3 StartPosition { get; set; }
    [field: SerializeField] public Vector3 EndPosition { get; set; }
    [field: SerializeField] public float Duration { get; set; }
    
    [field: SerializeField] private EPositionType PositionType { get; set; }

    public enum EPositionType
    {
        Global,
        Local,
    }
    private void Awake()
    {
        if (PositionType == EPositionType.Local)
        {
            var position = transform.position;
            StartPosition += position;
            EndPosition += position;
        }
        transform.position = StartPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        Vector3[] path = new[] { StartPosition, EndPosition, StartPosition };
        transform.DOPath(path, Duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }
    
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
