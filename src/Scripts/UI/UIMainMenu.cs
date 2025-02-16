using System;
using Progress;
using RunLevels.Scoring;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class UIMainMenu : MonoBehaviour, IUIMainMenu, IFarmLevelUp
    {
        [field: SerializeField] private Texture2D DollarsIcon { get; set; }
        [field: SerializeField] private UIDocument MainMenu { get; set; }
        [field: SerializeField] private IPlayerProgress PlayerProgress { get; set; }

        private Label DollarsLabel { get; set; }
        private Label CurrentLevelLabel { get; set; }

        private void Awake()
        {
            PlayerProgress ??= FindObjectOfType<PlayerProgress>();
            MainMenu ??= GetComponent<UIDocument>();
        }

        // Start is called before the first frame update
        void Start()
        {
            MainMenu.rootVisualElement.Q("DollarsIcon").style.backgroundImage = DollarsIcon;
            DollarsLabel ??= MainMenu.rootVisualElement.Q("DollarsText") as Label;
            CurrentLevelLabel ??= MainMenu.rootVisualElement.Q("CurrentLevelText") as Label;
            DollarsLabel.text = PlayerProgress.Dollars.ToString();
            CurrentLevelLabel.text = "Level " + PlayerProgress.CurrentLevel.ToString();
            
            PlayerProgress.RegisterForLevelUp(this);
        }

        // Update is called once per frame
        void Update()
        {
            DollarsLabel.text = PlayerProgress.Dollars.ToString();
        }

        public void SetDollars(int dollars)
        {
            DollarsLabel ??=  MainMenu.rootVisualElement.Q("DollarsText") as Label;
            DollarsLabel.text = dollars.ToString();
        }

        public void DoLevelUp()
        {
            CurrentLevelLabel.text = "Level " + PlayerProgress.CurrentLevel.ToString();
        }
    }

    public interface IUIMainMenu
    {
        public void SetDollars(int dollars);
    }
}
