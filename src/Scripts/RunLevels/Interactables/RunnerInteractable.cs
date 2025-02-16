using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RunnerInteractable : MonoBehaviour, IRunnerInteractable
{
    protected abstract Collider CollisionBox { get; set; }
    public abstract void Interact(GameObject runner);
    public bool IsEnabled => this.enabled;
}

public interface IRunnerInteractable
{
    public void Interact(GameObject runner);
    public bool IsEnabled { get; }
}
