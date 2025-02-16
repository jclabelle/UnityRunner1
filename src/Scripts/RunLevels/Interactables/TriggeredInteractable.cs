using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggeredInteractable : RunnerInteractable
{

    protected void DestroySelf()
    {
        Destroy(gameObject);
    }

    protected void SetColliderAsTrigger()
    {
        SetCollider();
        if (CollisionBox.isTrigger is false)
            CollisionBox.isTrigger = true;
    }

    protected void SetCollider()
    {
        if (!CollisionBox)
        {
            CollisionBox = GetComponent<Collider>();
        }
        
        if (!CollisionBox)
        {
            CollisionBox = GetComponentInChildren<Collider>();
        }
    }
    
}
