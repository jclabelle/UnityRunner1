using Progress;
using RunLevels;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Utilities.Cheats
{
    
    public class RunCheats : MonoBehaviour
    {
        private KeyControl _win = Keyboard.current.wKey;
        private KeyControl _addDollars = Keyboard.current.dKey;
        private KeyControl _showProgress = Keyboard.current.pKey;
        private KeyControl _showLevel = Keyboard.current.lKey;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(_win.wasPressedThisFrame)
                FindObjectOfType<LevelWinTrigger>().Interact(GameObject.FindGameObjectWithTag("Player"));
            
            if(_addDollars.wasPressedThisFrame)
                FindObjectOfType<PlayerProgress>().AddDollars(1000);
            
            if(_showProgress.wasPressedThisFrame)
                Debug.Log("Farm Progress(xp): " + FindObjectOfType<PlayerProgress>().CurrentFarmProgress);
            
            if(_showLevel.wasPressedThisFrame)
                Debug.Log("Farm Level: " +FindObjectOfType<PlayerProgress>().CurrentLevel);
        }
    }
}
