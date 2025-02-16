using Cinemachine;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

namespace RunLevels.Scoring
{
    public class ScoreBar : MonoBehaviour, IUIScoreBar
    {
        [field: SerializeField] private UIDocument Bar { get; set; }
        private VisualElement Root { get; set; }
        private VisualElement ScoreProgressElement { get; set; }
        private VisualElement ScoreBarElement { get; set; }
        [field: SerializeField] private Scoring Scoring { get; set; }

        private void Awake()
        {
            Bar ??= GetComponent<UIDocument>();
            Scoring ??= FindObjectOfType<Scoring>();
        }

        // Start is called before the first frame update
        void Start()
        {
            Root = Bar.rootVisualElement;
            ScoreProgressElement = Root.Q("ScoreProgress");
            ScoreBarElement = Root.Q("ScoreBar");
            
            var player = GameObject.FindWithTag("Player");
            var activeCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            var playerPosition = Root.panel.visualTree.WorldToLocal(player.transform.position);
            var camPos2 = activeCamera.WorldToScreenPoint(playerPosition);
            ScoreBarElement.style.top = new Length((int)((camPos2.y / (float)Screen.height) * 100), LengthUnit.Percent);
            
            Hide();

        }

        // Update is called once per frame
        void Update()
        {
            var score = (float)Scoring.Score / (float)Scoring.MaximumScore;
            ScoreProgressElement.style.width = new Length(score * 100, LengthUnit.Percent);
        }

        public void Reset()
        {
            var scoring = FindObjectOfType<Scoring>();
            var score = (float)Scoring.StartingScore / (float)Scoring.MaximumScore;
            ScoreProgressElement.style.width = new Length(Scoring.StartingScoreRatioOfMax, LengthUnit.Percent);
        }

        public void Hide()
        {
            Root.style.opacity = 0;

        }

        public void Show()
        {
            Root.style.opacity = 1;
        }
    }

    public interface IUIScoreBar
    {
        public void Reset();
        public void Hide();
        public void Show();
    }
}
