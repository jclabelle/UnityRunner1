using System;
using System.Collections.Generic;
using RunLevels.Scoring;
using UnityEngine;

namespace Progress
{
    public class PlayerProgress : MonoBehaviour, IPlayerProgress, IPlayerDollars
    {
        public int Dollars { get; set; }
        public int CurrentLevel { get; set; } = 1;
        public int CurrentFarmProgress { get; set; }
        private ISave Save { get; set; }
        public int CurrentDollars => Dollars;
        [field: SerializeField] public FarmLevelsRequirements LevelsRequirements { get; set; }
        public List<IFarmLevelUp> _LevelUpListeners { get; set; } = new List<IFarmLevelUp>();

        public void RegisterForLevelUp(IFarmLevelUp levelUpListener)
        {
            _LevelUpListeners.Add(levelUpListener);
        }

        public void DeregisterForLevelUp(IFarmLevelUp levelUpListener)
        {
            _LevelUpListeners.Remove(levelUpListener);
        }

        private void Awake()
        {
            Save ??= FindObjectOfType<Save>();
            CurrentLevel = Mathf.Max(1, CurrentLevel);
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            // CheckForLevelUp();
        }

  

        private void ActionLevelUp(int newLevel)
        {
            CurrentLevel++;
            for (int i = _LevelUpListeners.Count - 1; i >= 0; i--)
            {
                _LevelUpListeners[i].DoLevelUp();
            }
            Save.SaveGame();
        }

        public void AddScoreToProgress()
        {
            IScoring score = FindObjectOfType<RunLevels.Scoring.Scoring>();
            Dollars += score.Score;
            Save.SaveGame();
        }

        public void ContributeToProgression(int contribution)
        {
            CurrentFarmProgress += contribution;
            if(CurrentLevel < LevelsRequirements.LevelCap &&
                CurrentFarmProgress >= LevelsRequirements.GetRequirement(CurrentLevel))  //CurrentLevel = index of next level in List
                ActionLevelUp(CurrentLevel+1);
            
            Save.SaveGame();
        }


        public void AddDollars(int dollars)
        {
            Dollars += dollars;
            Save.SaveGame();
        }

        public void RemoveDollars(int dollars)
        {
            Dollars -= dollars;
            Save.SaveGame();
        }

    }

  

    public interface IPlayerProgress
    {
        public int Dollars { get; set; }
        public int CurrentLevel { get; set; }
        public int CurrentFarmProgress { get; set; }

        public void AddScoreToProgress();
        public void ContributeToProgression(int contribution);
        public void RegisterForLevelUp(IFarmLevelUp levelUpListener);
        public void DeregisterForLevelUp(IFarmLevelUp levelUpListener);

    }

    public interface IPlayerDollars
    {
        public void AddDollars(int dollars);
        public void RemoveDollars(int dollars); // player dollars are clamped at minimum 0
        public int CurrentDollars { get; }
    }

    public interface IFarmLevelUp
    {
        public void DoLevelUp();
    }

    public interface IFarmProgress
    {
        public int Contribution { get; set; }
        public bool ContributionSent { get; }
        public void SendContribution();

    }
}
