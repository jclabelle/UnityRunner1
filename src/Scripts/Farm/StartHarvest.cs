using System.Collections;
using Progress;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Farm
{
    public class StartHarvest : MonoBehaviour, IStartHarvest
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void ToHarvest(float harvestBonus)
        {
            FindObjectOfType<WorldDataController>().SaveAndCommit();
            FindObjectOfType<Save>().SaveGame();
            StartCoroutine(LoadWhenSaveComplete());
        }

        IEnumerator LoadWhenSaveComplete()
        {
            yield return null;
            SceneManager.LoadScene("RunLevelConstructor");

        }
    }

    public interface IStartHarvest
    {
        public void ToHarvest(float harvestBonus);
    }
}
