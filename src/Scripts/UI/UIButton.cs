using UnityEngine;
using UnityEngine.UIElements;
using System;
using Utilities;

namespace UI
{
    public class UIButton : MonoBehaviour, IShow, IHide
    {
    
        private Button Button { get; set; }
        private VisualElement Root { get; set; }
        private Camera ActiveCamera { get; set; }

        private IButtons ButtonsManager { get; set; }
        private Guid ID { get; set; }

        [field: SerializeField] public IClickable ClickableObject{
            get;
            set;
        }

        private void Awake()
        {
            ClickableObject ??= (IClickable)gameObject.GetComponent(typeof(IClickable));
            ButtonsManager = FindObjectOfType<ButtonsManager>();
            ActiveCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

            ID = System.Guid.NewGuid();


        }

        // Start is called before the first frame update
        void Start()
        {
            Button ??=ButtonMaker.MakeButton( ClickableObject.ButtonColor, ClickableObject.ButtonText);
            Button.clicked += ClickableObject.Click;
            ButtonsManager.Add(ID, Button);


        }

        // Update is called once per frame
        void Update()
        {
            UpdatePosition();
            UpdateStyle();
        }
        
        private void UpdatePosition()
        {
            
            var positionOnScreen =  ActiveCamera.WorldToScreenPoint(gameObject.transform.position);

            Button.style.left = new Length((positionOnScreen.x), LengthUnit.Pixel);
            Button.style.top = new Length((Screen.height-positionOnScreen.y), LengthUnit.Pixel);
        }

        private void UpdateStyle()
        {
            Button.text = ClickableObject.ButtonText;
            Button.style.backgroundColor = ClickableObject.ButtonColor;
        }


        private void OnDestroy()
        {
            ButtonsManager.Remove(ID);
        }

        public void Hide()
        {
            Button ??=ButtonMaker.MakeButton( ClickableObject.ButtonColor, ClickableObject.ButtonText);
            Button.visible = false;
        }

        public void Show()
        {
            Button.visible = true;
        }
    }
    
    public interface IClickable
    {
        public void Click();

        public Color ButtonColor { get; }
        public string ButtonText { get; }
    }
}
