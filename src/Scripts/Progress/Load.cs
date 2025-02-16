using UnityEngine;

namespace Progress
{
    public class Load : MonoBehaviour, ILoad
    {
        [field: SerializeField] public IPlayerProgress Player { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            Player ??= FindObjectOfType<Progress.PlayerProgress>();
            LoadGame();
        }

        // Update is called once per frame
        void Update()
        {
        
        }


        public void LoadGame()
        {
            if(PlayerPrefs.HasKey("Dollars"))
                Player.Dollars = PlayerPrefs.GetInt("Dollars");
            
            if(PlayerPrefs.HasKey("CurrentLevel"))
                Player.CurrentLevel = PlayerPrefs.GetInt("CurrentLevel");
            
            if(PlayerPrefs.HasKey("FarmProgress"))
                Player.CurrentFarmProgress = PlayerPrefs.GetInt("FarmProgress");
        }
    }
    
    public interface ILoad
    {
        public void LoadGame();
    }
}
