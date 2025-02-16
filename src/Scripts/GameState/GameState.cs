using RunLevels;
using RunLevels.Scoring;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace GameState
{
    public class GameState : MonoBehaviour, IGameState
    {
        [field: SerializeField] private IGameState.EState CurrentState { get; set; }
        [field: SerializeField] private RunLevels.IRunnerState RunnerState { get; set; }
        [field: SerializeField] private RunLevels.ILevelState LevelState { get; set; }
        [field: SerializeField] private UI.IUIState UIState { get; set; }
        [field: SerializeField] private IScoring Scoring { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            RunnerState ??= FindObjectOfType<RunLevels.RunnerState>();
            LevelState ??= FindObjectOfType<RunLevels.LevelState>();
            UIState ??= FindObjectOfType<UI.UIState>();
            Scoring ??= FindObjectOfType<Scoring>();
        }

        // Update is called once per frame
        void Update()
        {
            if (CurrentState == IGameState.EState.MainMenu &&
                (Keyboard.current.leftArrowKey.wasPressedThisFrame ||
                 Keyboard.current.rightArrowKey.wasPressedThisFrame))
            {
                Set(IGameState.EState.RunLevel);
            }


            if (CurrentState == IGameState.EState.RunLevel &&
                LevelState.CurrentState is ILevelState.EState.Win or ILevelState.EState.Lose &&
                (Keyboard.current.leftArrowKey.wasPressedThisFrame ||
                 Keyboard.current.rightArrowKey.wasPressedThisFrame))
            {
                SceneToFarm();
            }


        }

        public void SceneToFarm()
        {
            SceneManager.LoadScene("PrototypeFarm");
        }

        public void Set(IGameState.EState state)
        {


            switch (state)
            {
                case IGameState.EState.MainMenu:
                    DoMainMenu();
                    break;

                case IGameState.EState.RunLevel:
                    DoRun();
                    break;
            }
        }



        public void ForceSet(IGameState.EState state)
        {
            switch (state)
            {
                case IGameState.EState.MainMenu:
                    DoMainMenu();
                    break;

                case IGameState.EState.RunLevel:
                    DoRun();
                    break;
            }
        }
        
        private void DoRun()
        {
            CurrentState = IGameState.EState.RunLevel;
            LevelState.Set(ILevelState.EState.Run);
            UIState.Set(IUIState.EState.RunLevel);
            Scoring.Reset();

        }

        private void DoMainMenu()
        {
            CurrentState = IGameState.EState.MainMenu;
            LevelState.Set(ILevelState.EState.PreRun);
            UIState.Set(IUIState.EState.MainMenu);

        }
    }

    public interface IGameState
    {
        public enum EState
        {
            MainMenu,
            RunLevel,
        }
        
        public void Set(IGameState.EState state);

    }
}
