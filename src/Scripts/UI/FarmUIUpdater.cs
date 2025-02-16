using System;
using System.Collections;
using Progress;
using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace UI
{
    public class FarmUIUpdater : MonoBehaviour, IFarmUI
    {
        private InputSystemUIInputModule _inputSystemUIInputModule;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(InitUI());   // Fill in Level and Dollars information
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
      
        public void RefreshUI()
        {
            var mainMenuUI = FindObjectOfType<UIMainMenu>() as IUIMainMenu;
            var playerProgress = FindObjectOfType<PlayerProgress>() as IPlayerProgress;;
            mainMenuUI.SetDollars(playerProgress.Dollars);
        }
        
        IEnumerator InitUI()
        {
            yield return null;
            RefreshUI();
        }
    }

    public interface IFarmUI
    {
        public void RefreshUI();
    }
}
