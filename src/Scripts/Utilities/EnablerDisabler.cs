using Farm;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Utilities
{
    public class EnablerDisabler : MonoBehaviour, IEnableDisable
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void EnableAll()
        {
          
        }

        public void DisableAll()
        {
        }
    }

    public interface IEnableDisable
    {
        public void EnableAll();
        public void DisableAll();
    }

    public interface IHide
    {
        public void Hide();
    }

    public interface IShow
    {
        public void Show();
    }
}
