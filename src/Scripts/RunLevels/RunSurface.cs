using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Debug = System.Diagnostics.Debug;

public class RunSurface : MonoBehaviour
{
    [field: SerializeField] public EType Type { get; set; }
    public Bounds bounds;
    [field: SerializeField] public float Width { get; set; }
    public enum EType
    {
        Y0,
        Y90,
        Y180,
        Y270
    }

// Start is called before the first frame update
    void Start()
    {
        bounds = GetComponent<MeshCollider>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Vector3 Rotation(RunSurface runSurface)
    {
        var newRotation = runSurface.Type switch
        {
            RunSurface.EType.Y0 => new Vector3(0, 0, 0),
            RunSurface.EType.Y90 => new Vector3(0, 90, 0),
            RunSurface.EType.Y180 => new Vector3(0, 180, 0),
            RunSurface.EType.Y270 => new Vector3(0, 270, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(runSurface.Type) )
        };

        return newRotation;
    }

    // Get the left direction vector for a RunSurface 
    public static Vector3 MoveLeft(RunSurface runSurface)
    {
        var moveLeft = runSurface.Type switch
        {
            RunSurface.EType.Y0 => new Vector3(-1, 0, 0),
            RunSurface.EType.Y90 => new Vector3(0, 0, 1),
            RunSurface.EType.Y180 => new Vector3(1, 0, 0),
            RunSurface.EType.Y270 => new Vector3(0, 0, -1),
            _ => throw new ArgumentOutOfRangeException(nameof(runSurface.Type))
        };

        return moveLeft;
    }
    

    // Get the right direction vector for a RunSurface 
    public static Vector3 MoveRight(RunSurface runSurface)
    {
        return RunSurface.MoveLeft(runSurface) * -1;
    }
    
    // Get the forward direction vector for a RunSurface
    public static Vector3 MoveForward(RunSurface runSurface)
    {
        var newMove = runSurface.Type switch
        {
            RunSurface.EType.Y0 => new Vector3(0, 0, 1),
            RunSurface.EType.Y90 => new Vector3(1, 0, 0),
            RunSurface.EType.Y180 => new Vector3(0, 0, -1),
            RunSurface.EType.Y270 => new Vector3(-1, 0, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(runSurface.Type) )
        };

        return newMove;
    }

    public static Vector3 Offsets(RunSurface runSurface, float height, float distanceFromSubject, float leftOffset = 0)
    {
        
        var newPosition = runSurface.Type switch
        {
            RunSurface.EType.Y0 => new Vector3(-leftOffset,height, -distanceFromSubject),
            RunSurface.EType.Y90 => new Vector3(-distanceFromSubject,height, leftOffset),
            RunSurface.EType.Y180 => new Vector3(leftOffset,height, distanceFromSubject),
            RunSurface.EType.Y270 => new Vector3(distanceFromSubject,height, -leftOffset),
            _ => throw new ArgumentOutOfRangeException(nameof(runSurface.Type) )
        };

        return newPosition;
    }

   

}
