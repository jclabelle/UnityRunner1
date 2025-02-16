using System;
using Progress;
using RunLevels.Scoring;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RunLevels
{
    public class LevelState : MonoBehaviour, ILevelState
    {
        [field: SerializeField] public ILevelState.EState CurrentState { get; set; }
        [field: SerializeField] public IRunnerState RunnerState { get; set; }
        [field: SerializeField] public ILevelMusic LevelMusic { get; set; }
        [field:SerializeField] public  IPlayerProgress PlayerProgress { get; set; }
        [field: SerializeField] private IScoring Scoring { get; set; }


        private void Awake()
        {
            RunnerState ??= FindObjectOfType<RunnerState>();
            LevelMusic ??= FindObjectOfType<LevelMusic>();
            PlayerProgress ??= FindObjectOfType<PlayerProgress>();
            Scoring ??= FindObjectOfType<Scoring.Scoring>();

        }

        // Start is called before the first frame update
        void Start()
        {
            Set(ILevelState.EState.PreRun);
        }
        

        // Update is called once per frame
        void Update()
        {
            if (CurrentState == ILevelState.EState.Run && Scoring.Score == 0)
                DoLose();

        }
        

        private void DoPreRun()
        {
            CurrentState = ILevelState.EState.PreRun;
            RunnerState.Set(IRunnerState.EState.Idle);
            LevelMusic.Set(ILevelMusic.EState.PreRun);
        }
    
        private void DoRun()
        {
            CurrentState = ILevelState.EState.Run;
            RunnerState.Set(IRunnerState.EState.Walk);
            LevelMusic.Set(ILevelMusic.EState.Walk);
        }

        private void DoLose()
        {
            CurrentState = ILevelState.EState.Lose;
            RunnerState.Set(IRunnerState.EState.Lose);
            LevelMusic.Set(ILevelMusic.EState.Lose);
        }

        private void DoWin()
        {
            CurrentState = ILevelState.EState.Win;
            RunnerState.Set(IRunnerState.EState.Win);
            LevelMusic.Set(ILevelMusic.EState.Win);
            PlayerProgress.AddScoreToProgress();
            
            IUIMainMenu mainMenu = FindObjectOfType<UIMainMenu>();
            mainMenu.SetDollars(PlayerProgress.Dollars);
        }

        public void Set(ILevelState.EState newState)
        {
            switch (newState)
            {
                case ILevelState.EState.PreRun:
                    DoPreRun();
                    break;
                case ILevelState.EState.Run:
                    DoRun();
                    break;
                case ILevelState.EState.Lose:
                    if(CurrentState is not ILevelState.EState.PreRun)
                        DoLose();
                    break;
                case ILevelState.EState.Win:
                    DoWin();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }
    }

    public interface ILevelState
    {
        public enum EState
        {
            PreRun,
            Run,
            Lose,
            Win,
        }

        public void Set(EState state);
        public EState CurrentState { get; set; }
    }
}
