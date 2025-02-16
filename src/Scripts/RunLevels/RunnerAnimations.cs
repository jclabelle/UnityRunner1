using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

[RequireComponent(typeof(AnimancerComponent))]
public class RunnerAnimations : MonoBehaviour, IRunnerAnimations
{
    [field: SerializeField] private AnimationClip Walking { get; set; }
    [field: SerializeField] private AnimationClip Damage { get; set; }
    [field: SerializeField] private AnimationClip Idle { get; set; }
    [field: SerializeField] private AnimationClip Win { get; set; }
    [field: SerializeField] private AnimationClip Lose { get; set; }
    [field: SerializeField] public AnimancerComponent Animancer { get; set; }
    private AnimancerState CurrentState { get; set; }


    
    // Start is called before the first frame update
    void Start()
    {
        if (Animancer is null)
            Animancer = GetComponent<AnimancerComponent>();

        PlayIdle();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public AnimancerState Play(IRunnerAnimations.EClip clip)
    {
        switch (clip)
        {
            case IRunnerAnimations.EClip.Walk : 
                return PlayWalk();
                break;
            
            case IRunnerAnimations.EClip.Damage : 
                return PlayDamage();
                break;
            
            case IRunnerAnimations.EClip.Idle : 
                return PlayIdle();
                break;
                
            case IRunnerAnimations.EClip.Win : 
                return PlayWin();
                break;
            
            case IRunnerAnimations.EClip.Lose : 
                return PlayLose();
                break;
            default:
                return CurrentState;
        }
        
    }

    private AnimancerState PlayLose()
    {
        var state = Animancer.Play(Lose, 0.25f);
        CurrentState = state;
        return state;

    }

    private AnimancerState PlayWin()
    {
        var state = Animancer.Play(Win, 0.25f);
        CurrentState = state;
        return state;

    }

    private AnimancerState PlayIdle()
    {
        var state = Animancer.Play(Idle, 0.25f);
        CurrentState = state;
        return state;

    }

    private AnimancerState PlayDamage()
    {
        var state = Animancer.Play(Damage, 0.25f);
        state.Speed = 1f;
        state.Time = 0;
        
        CurrentState = state;
        return state;
    }

    private AnimancerState PlayWalk()
    {
        var state = Animancer.Play(Walking, 0.25f);
        CurrentState = state;
        return state;

    }

  
}

public interface IRunnerAnimations
{
    public AnimancerState Play(IRunnerAnimations.EClip clip);
    
    public enum EClip
    {
        Walk,
        Damage,
        Idle,
        Win,
        Lose
    }
}


