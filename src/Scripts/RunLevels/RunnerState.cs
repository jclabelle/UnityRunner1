using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RunLevels
{
    public class RunnerState : MonoBehaviour, IRunnerState
    {
        [field: SerializeField] private IRunnerAnimations RunnerAnimations { get; set; }
        [field: SerializeField] private IRunnerAudio RunnerAudio { get; set; }
        [field: SerializeField] private IRunnerControl RunnerControl { get; set; }
        [field: SerializeField] private float DamageDuration { get; set; }
        [field: SerializeField] private IRunnerState.EState CurrentState { get; set; }

        private void Awake()
        {
            RunnerAnimations ??= FindObjectOfType<RunnerAnimations>();
            RunnerAudio ??= FindObjectOfType<RunnerAudio>();
            RunnerControl ??= FindObjectOfType<RunnerController>();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    
        public void Set(IRunnerState.EState state)
        {
            if (CurrentState is IRunnerState.EState.Win or IRunnerState.EState.Lose)
                return;
        
            switch (state)
            {
                case IRunnerState.EState.Walk :
                    DoRun();
                    break;
            
                case IRunnerState.EState.Damage :
                    DoDamage();
                    break;
            
                case IRunnerState.EState.Idle :
                    DoIdle();
                    break;
                
                case IRunnerState.EState.Win : 
                    DoWin();
                    break;
            
                case IRunnerState.EState.Lose : 
                    DoLose();
                    break;
            }
        }
    
        public void ForceSet(IRunnerState.EState state)
        {
            switch (state)
            {
                case IRunnerState.EState.Walk :
                    DoRun();
                    break;
            
                case IRunnerState.EState.Damage :
                    DoDamage();
                    break;
            
                case IRunnerState.EState.Idle :
                    DoIdle();
                    break;
                
                case IRunnerState.EState.Win : 
                    DoWin();
                    break;
            
                case IRunnerState.EState.Lose : 
                    DoLose();
                    break;
            }
        }
    
        private void DoRun()
        {
            RunnerAnimations.Play(IRunnerAnimations.EClip.Walk);
            RunnerControl.SetForwardSpeed(RunnerControl.ForwardSpeed, 0.5f);
            RunnerControl.StartLateralMovement();
        }
    
        private void DoDamage()
        {
            RunnerAnimations.Play(IRunnerAnimations.EClip.Damage);
            RunnerAudio.Play(IRunnerAudio.EClip.Damage);
            RunnerControl.SetForwardSpeed(1);
            StartCoroutine(DamageTimer());
        }
    
        private void DoIdle()
        {
            RunnerControl.MoveToSpawnPoint();
            RunnerAnimations.Play(IRunnerAnimations.EClip.Idle);
            RunnerControl.SetForwardSpeed(0, 0.5f);
        }
    
        private void DoWin()
        {
            RunnerAnimations.Play(IRunnerAnimations.EClip.Win);
            RunnerControl.SetForwardSpeed(0, 0.5f);
            RunnerControl.StopLateralMovement();
        }
    
        private void DoLose()
        {
            RunnerAnimations.Play(IRunnerAnimations.EClip.Lose);
            RunnerControl.SetForwardSpeed(0, 0.5f);
            RunnerControl.StopLateralMovement();
        }

        IEnumerator DamageTimer()
        {
            var damageTimer = 0f;

            while (damageTimer < DamageDuration)
            {
                damageTimer += Time.deltaTime;
                yield return null;
            }
        
            Set(IRunnerState.EState.Walk);
        
        }
    
    }
    public interface IRunnerState
    {
        public enum EState
        {
            Walk,
            Damage,
            Idle,
            Win,
            Lose
        }
        
        public void Set(IRunnerState.EState state);
    }
}