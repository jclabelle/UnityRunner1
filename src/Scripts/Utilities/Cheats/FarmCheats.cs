using System;
using Farm;
using Progress;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Utilities.Cheats
{
    public class FarmCheats : MonoBehaviour
    {
        private KeyControl _addDollars;
        private KeyControl _showProgress;
        private KeyControl _showLevel;
        private KeyControl _forceSave;
        private KeyControl _showTimers;
        private KeyControl _loadLevel;

        private void Awake()
        {
         _addDollars = Keyboard.current.dKey;
         _showProgress = Keyboard.current.oKey;
         _showLevel = Keyboard.current.lKey;
         _forceSave = Keyboard.current.sKey;
         _showTimers = Keyboard.current.kKey;
         _loadLevel = Keyboard.current.pKey;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if(_addDollars.wasPressedThisFrame)
                FindObjectOfType<PlayerProgress>().AddDollars(1000);
            
            if(_showProgress.wasPressedThisFrame)
                Debug.Log("Farm Progress(xp): " +FindObjectOfType<PlayerProgress>().CurrentFarmProgress);
            
            if(_showLevel.wasPressedThisFrame)
                Debug.Log("Farm Level: " +FindObjectOfType<PlayerProgress>().CurrentLevel);
            
            if(_forceSave.wasPressedThisFrame)
                FindObjectOfType<WorldDataController>().SaveAndCommit();

            if (_showTimers.wasPressedThisFrame)
            {
                var persistentTimers = FindObjectsOfType<PersistentTimer>();
                foreach (var timer in persistentTimers)
                {
                    Debug.Log($"Start Time: {timer.StartTime}\nStart Real Time: {timer.StartRealTime}\nStop Time: {timer.StopTime}\nElapsed Time: {timer.ElapsedTime}\nDuration Type: {timer.DurationType}\nState: {timer.State}");
                }
            }

            if (_loadLevel.wasPressedThisFrame)
            {
                GameObject.FindObjectOfType<StartHarvest>().ToHarvest(1.0f);
            }
        }
    }
}
