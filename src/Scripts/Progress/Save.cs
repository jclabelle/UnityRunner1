using System;
using UnityEngine;

namespace Progress
{
    public class Save : MonoBehaviour, ISave
    {
        [field: SerializeField] public IPlayerProgress Player { get; set; }

        private void Awake()
        {
            Player ??= FindObjectOfType<Progress.PlayerProgress>();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SaveGame()
        {
            PlayerPrefs.SetInt("Dollars", Player.Dollars);
            PlayerPrefs.SetInt("CurrentLevel", Player.CurrentLevel);
            PlayerPrefs.SetInt("FarmProgress", Player.CurrentFarmProgress);
            PlayerPrefs.Save();
            
        }
    }

    public interface ISave
    {
        public void SaveGame();
    }
}
