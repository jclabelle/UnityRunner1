using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class ButtonsManager : MonoBehaviour, IButtons
    {
        [field: SerializeField] private UIDocument ButtonsRoot { get; set; }
        private VisualElement Root { get; set; }
        private Dictionary<System.Guid, Button> ActiveButtons { get; set; } = new Dictionary<System.Guid, Button>();

        private void Awake()
        {
            ButtonsRoot ??= GetComponent<UIDocument>();

        }

        // Start is called before the first frame update
        void Start()
        {
            Root ??= ButtonsRoot.rootVisualElement;

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Add(System.Guid id, Button button)
        {
            Root ??= ButtonsRoot.rootVisualElement;
        
            Root.Add(button);
            ActiveButtons.Add(id, button);
        }

        public void Remove(Guid id)
        {
            Root ??= ButtonsRoot.rootVisualElement;
            if (ActiveButtons.TryGetValue(id, out var buttonToDestroy))
            {
                Root.Remove(buttonToDestroy);
                ActiveButtons.Remove(id);
            }

        }
    }

    public interface IButtons
    {
        public void Add(System.Guid id, Button button);
        public void Remove(System.Guid id);

    }
}
