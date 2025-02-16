using System;
using System.Collections;
using System.Collections.Generic;
using RunLevels;
using RunLevels.Interactables;
using Unity.VisualScripting;
using UnityEngine;

namespace RunLevels.Scoring
{
    public class Scoring : MonoBehaviour, IScoring
    {
        [field: SerializeField] public float StartingScoreRatioOfMax { get; set; }
        [field: SerializeField] public int DefaultAdd { get; set; }
        [field: SerializeField] public int DefaultRemove { get; set; }
        [field: SerializeField] public float DefaultMultiply { get; set; }
        [field: SerializeField] public float DefaultDivide { get; set; }
        [field: SerializeField] public int MaximumScore { get; set; }
        public int StartingScore
        {
            get => (int)(MaximumScore * StartingScoreRatioOfMax);
        }
        [field: SerializeField] public GameObject ScoreTextPrefab { get; set; }
        [field: SerializeField] public AudioClip PickUpGood { get; set; }
        [field: SerializeField] public AudioClip PickUpBad { get; set; }
        [field: SerializeField] public RunnerController Runner { get; set; }
        [field: SerializeField] public ILevelState LevelState { get; set; }
    
        private List<ScoreText> ActiveScoreTexts { get; set; }
    
        private int _score;
        public int Score { get => _score;
            // Bounds score between 0 and MaximumScore
            private set => _score = Mathf.Max(
                0, 
                Mathf.Min(value, MaximumScore)
                );
        }

        private void Awake()
        {
            Reset();
            ActiveScoreTexts = new List<ScoreText>(10);
        }

        private void Start()
        {
            Runner ??= GameObject.FindWithTag("Player").GetComponentInChildren<RunnerController>();
            LevelState ??= FindObjectOfType<LevelState>();

            FillDefaults();
        }

        private void FillDefaults()
        {
            var addScoreObjects = FindObjectsOfType<AddScore>();
            foreach (var add in addScoreObjects)
            {
                if(add.ScoreAdded <= 0)
                    add.ScoreAdded = DefaultAdd;
            }
            
            var removeScoreObjects = FindObjectsOfType<RemoveScore>();
            foreach (var rem in removeScoreObjects)
            {
                if(rem.ScoreRemoved <= 0)
                    rem.ScoreRemoved = DefaultRemove;
            }
            
            var multiplyScoreObjects = FindObjectsOfType<MultiplyScore>();
            foreach (var mul in multiplyScoreObjects)
            {
                if(mul.ScoreMultiplier < 1)
                    mul.ScoreMultiplier = DefaultMultiply;
            }
            
            var divideScoreObjects = FindObjectsOfType<DivideScore>();
            foreach (var div in divideScoreObjects)
            {
                if(div.ScoreDivisor < 1)
                    div.ScoreDivisor = DefaultDivide;
            }

        }
        
    
        private void Update()
        {
            if(Score <=0)
                LevelState.Set(ILevelState.EState.Lose);
            
            Debug.Log(Score);
            
        }
        
        public void Reset()
        {
            _score = (int)(StartingScoreRatioOfMax * MaximumScore);
        }

        public int Add(int scoreChange)
        {
            Score += scoreChange;
            SpawnScoreEvent("+" +scoreChange.ToString(), Color.green, PickUpGood);
            return Score;
        }

        private void SpawnScoreEvent(string scoreChange, Color color, AudioClip pickUpSound)
        {
            var scoreObject = GameObject.Instantiate(ScoreTextPrefab);
            var scoreText = scoreObject.GetComponent<ScoreText>();
            scoreText.Runner = Runner;
            scoreText.Score = scoreChange;
            scoreText.Color = color;
            scoreText.PickUpSound = pickUpSound;
            scoreText.IsTemplate = false;
        
        
            scoreText.RemoveSelfFromActiveTexts = RemoveScoreText; // Called OnDestroy
            ActiveScoreTexts.Add(scoreText);
        }

        public int Remove(int scoreChange)
        {
            Score -= scoreChange;
            SpawnScoreEvent("-" +scoreChange.ToString(), Color.red, PickUpBad);
            return Score;
        }

        public int Multiply(float scoreMultiplier)
        {
            Score = (int)((float)Score  * scoreMultiplier);
            SpawnScoreEvent("X" + scoreMultiplier.ToString(), Color.green, PickUpGood);
            return Score;
        }

        public int Divide(float scoreDivisor)
        {
            Score = (int)((float)Score  / scoreDivisor);
            SpawnScoreEvent("/" + scoreDivisor.ToString(), Color.red, PickUpBad);
            return Score;
        }

        public void ChangeScoreTextRunSurface(RunSurface runSurface, float rotationSpeed)
        {
            foreach(var activeScoreText in ActiveScoreTexts)
                activeScoreText.NewRunSurface(runSurface, rotationSpeed);
        }

        public void RemoveScoreText(ScoreText scoreText) => ActiveScoreTexts.Remove((scoreText));

    }

    public interface IScoring
    {
        public int Add(int scoreChange); // Add to the Score, then returns the Score.
        public int Remove(int scoreChange); // Remove from the Score, then returns the Score.
        public int Multiply(float scoreMultiplier); // Multiply the Score, then returns the Score.
        public int Divide(float scoreDivisor); // Divide the Score, then returns the Score.
        public int Score {get; } // Returns the current score.
        public int MaximumScore {get; } // Returns the current score.
        public int StartingScore {get; } // Returns the starting score.
        public float StartingScoreRatioOfMax {get; } // Returns the starting score.
        public void Reset();

    }
}