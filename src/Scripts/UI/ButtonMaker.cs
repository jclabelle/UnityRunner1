using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class ButtonMaker : MonoBehaviour
    {
        public static Color buttonGreen = new Color(0.2f, 0.8f, 0.1f, 255f);
        public static Color buttonPurple = new Color(0.53f, 0.08f, 0.69f, 255f);
        public static float ButtonBorderSize { get; set; } = 3.0f;
        public static string HarvestText { get; set; } = "Harvest!";
        public static string UnlockText { get; set; } = "Unlock!";
    
    
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public static Button MakeHarvestButton()
        {
            var button = new Button();
        
            button.style.position = Position.Absolute;
            button.style.backgroundColor = buttonGreen;
            button.style.width = Screen.width * 0.2f;
            button.style.height = Screen.height * 0.03f;
        
            button.style.borderBottomColor = Color.black;
            button.style.borderLeftColor = Color.black;
            button.style.borderRightColor = Color.black;
            button.style.borderTopColor = Color.black;

            button.style.borderLeftWidth = new StyleFloat(ButtonBorderSize);
            button.style.borderRightWidth = new StyleFloat(ButtonBorderSize);
            button.style.borderTopWidth = new StyleFloat(ButtonBorderSize);
            button.style.borderBottomWidth = new StyleFloat(ButtonBorderSize);

            button.text = HarvestText;
            button.style.fontSize = 48;

            return button;
        }
        
        public static Button MakeUnlockButton()
        {
            var button = new Button();
        
            button.style.position = Position.Absolute;
            button.style.backgroundColor = buttonPurple;
            button.style.width = Screen.width * 0.2f;
            button.style.height = Screen.height * 0.03f;
        
            button.style.borderBottomColor = Color.black;
            button.style.borderLeftColor = Color.black;
            button.style.borderRightColor = Color.black;
            button.style.borderTopColor = Color.black;

            button.style.borderLeftWidth = new StyleFloat(ButtonBorderSize);
            button.style.borderRightWidth = new StyleFloat(ButtonBorderSize);
            button.style.borderTopWidth = new StyleFloat(ButtonBorderSize);
            button.style.borderBottomWidth = new StyleFloat(ButtonBorderSize);

            button.text = UnlockText;
            button.style.fontSize = 48;

            return button;
        }

        public static Button MakeButton(Color color, string text)
        {
            var button = new Button();
        
            button.style.position = Position.Absolute;
            button.style.backgroundColor = color;
            button.style.width = Screen.width * 0.3f;
            button.style.height = Screen.height * 0.03f;
        
            button.style.borderBottomColor = Color.black;
            button.style.borderLeftColor = Color.black;
            button.style.borderRightColor = Color.black;
            button.style.borderTopColor = Color.black;

            button.style.borderLeftWidth = new StyleFloat(ButtonBorderSize);
            button.style.borderRightWidth = new StyleFloat(ButtonBorderSize);
            button.style.borderTopWidth = new StyleFloat(ButtonBorderSize);
            button.style.borderBottomWidth = new StyleFloat(ButtonBorderSize);

            button.text = text;
            button.style.fontSize = 48;

            return button;
        }

    }
}
