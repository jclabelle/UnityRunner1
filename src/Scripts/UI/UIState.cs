using RunLevels.Scoring;
using UnityEngine;

namespace UI
{
    public class UIState : MonoBehaviour, IUIState
    {
        [field: SerializeField] public IUIState.EState CurrentState { get; set; }
        [field: SerializeField] public RunLevels.Scoring.IUIScoreBar ScoreBar { get; set; }
        [field: SerializeField] public IUIMainMenu MainMenu { get; set; }
        

        // Start is called before the first frame update
        void Start()
        {
            ScoreBar ??= FindObjectOfType<ScoreBar>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Set(IUIState.EState state)
        {
            switch (state)
            {
                case IUIState.EState.MainMenu:
                    DoMainMenu();
                    break;

                case IUIState.EState.RunLevel:
                    DoRunLevel();
                    break;
            }
        }

        private void DoRunLevel()
        {
            ScoreBar.Show();
        }

        private void DoMainMenu()
        {
            ScoreBar.Hide();
            ScoreBar.Reset();
            
            
        }
    }

    public interface IUIState
    {
        public enum EState
        {
            MainMenu,
            RunLevel,
        }
        
        public void Set(IUIState.EState state);
    }
}
