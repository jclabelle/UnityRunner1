using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChaseCam : MonoBehaviour
{
    [field: SerializeField] public GameObject Player { get; set; }
    [field: SerializeField] public Vector3 OffsetRotY0 { get; set; }
    private Vector3 _currentOffset;
    [field: SerializeField] private float CamAngleX { get; set; } = 20f;
    [field: SerializeField] private float CamHeight { get; set; } = 3.4f;
    [field: SerializeField] private float CamDistanceFromSubject { get; set; } = 5f;
    // Start is called before the first frame update

    private void Awake()
    {
        _currentOffset = OffsetRotY0;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position + _currentOffset;
    }

    public void NewRunSurface(RunSurface runSurface, float rotationSpeed)
    {
        
        var newRotation = RunSurface.Rotation(runSurface);
        newRotation += new Vector3(CamAngleX, 0, 0);
        transform.DORotate(newRotation, rotationSpeed);
        
        var newOffset = RunSurface.Offsets(runSurface, CamHeight, CamDistanceFromSubject);
        DOTween.To(()=> _currentOffset, x=> _currentOffset = x, newOffset, rotationSpeed);
        
    }
}
