using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Farm.Controls
{
    public class CameraScroll : MonoBehaviour
    {
        private GameObject CameraMan { get; set; } 
        private float CameraRotY { get; set; } 
        private Matrix4x4 IsoMatrix { get; set; }
        [field:SerializeField] public float ScrollSpeedLateral { get; set; }
        [field:SerializeField] public float ScrollSpeedUp { get; set; }
        [field:SerializeField] public float ScrollSpeedDown { get; set; }
        private Vector3 IsoRight { get; set; }
        private Vector3 IsoLeft { get; set; }
        private Vector3 IsoUp { get; set; }
        private Vector3 IsoDown { get; set; }

        private Vector3 _controlsInput;
        


        // Start is called before the first frame update
        void Start()
        
        {
            CameraMan ??= GameObject.FindGameObjectWithTag("CameraMan");
            CameraRotY = CameraMan.transform.rotation.y;
            
            // Prep the transform matrix to make controls behave according to the user's left/up/right/down reference
            IsoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, CameraRotY, 0)); 

            // Save a reference to the pre-Iso transformation directions
            IsoRight = Vector3.right + Vector3.back;
            IsoLeft = Vector3.left + Vector3.forward;
            IsoDown = Vector3.left + Vector3.back;
            IsoUp = Vector3.right + Vector3.forward;


            var playerInput = FindObjectOfType<PlayerInput>();
        }

        // Update is called once per frame
        void Update()
        {
            Scroll();
            MouseScroll();

            DebugStuff();
        }

        private void DebugStuff()
        {
            if(Keyboard.current.tKey.wasPressedThisFrame)
                FindObjectOfType<WorldDataController>().PrintState();
            
            if(Keyboard.current.yKey.wasPressedThisFrame)
                FindObjectOfType<WorldDataController>().PrintScenes();
            
            if(Keyboard.current.uKey.wasPressedThisFrame)
                FindObjectOfType<WorldDataController>().PrintPObjects();
        }

        private void MouseScroll()
        {
            var mouse = Mouse.current;
            
            if(mouse.leftButton.wasPressedThisFrame)
                Debug.Log("Left Button");
            
            if(mouse.rightButton.wasPressedThisFrame)
                Debug.Log("Right Button");
            
            if(mouse.middleButton.wasPressedThisFrame)
                Debug.Log("Middle Button");
            
            
            var uiInput = FindObjectOfType<InputSystemUIInputModule>();

        }

        void Scroll()
        {
            _controlsInput = new Vector3();
            var frameDeltaTime = Time.deltaTime;
            
            if (Keyboard.current.leftArrowKey.isPressed)
            {
                _controlsInput += (Vector3.left + Vector3.forward) * (ScrollSpeedLateral * frameDeltaTime);
            }
            
            if (Keyboard.current.rightArrowKey.isPressed)
            {
                _controlsInput +=  (Vector3.right + Vector3.back) * (ScrollSpeedLateral * frameDeltaTime);
            }
            
            if (Keyboard.current.upArrowKey.isPressed)
            {
                _controlsInput += (Vector3.right + Vector3.forward) * (ScrollSpeedUp * frameDeltaTime);
            }
            
            if (Keyboard.current.downArrowKey.isPressed)
            {
                _controlsInput += (Vector3.left + Vector3.back) * (ScrollSpeedDown * frameDeltaTime);
            }
            
            // Transform the inputs using the Matrix and move the CameraPivot
            var isoInput = IsoMatrix.MultiplyPoint3x4(_controlsInput);
            CameraMan.transform.position += isoInput;
            
        }

    }
}
