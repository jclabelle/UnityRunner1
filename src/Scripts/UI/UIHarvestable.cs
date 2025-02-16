using System;
using Farm;
using UnityEngine;
using Utilities;
using UnityEngine.UIElements;

namespace UI
{
    // Objects that can trigger a harvest run.
    public class UIHarvestable : MonoBehaviour, IUIHarvestable, IHide, IShow
    {
        [field: SerializeField] private IHarvestable Harvestable { get; set; }
        private Button HarvestButton { get; set; }
        private VisualElement Root { get; set; }
        private Camera ActiveCamera { get; set; }
        private Action HarvestClicked { get; set; }
        private IButtons ButtonsManager { get; set; }
        private Guid ID { get; set; }

        private void Awake()
        {
            Harvestable ??= GetComponent<Harvestable>();
            ActiveCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            HarvestClicked = DoButtonClick;
            ButtonsManager = FindObjectOfType<ButtonsManager>();
            ID = System.Guid.NewGuid();

        }

        // Start is called before the first frame update
        void Start()
        {
            
            State = IUIHarvestable.EState.CooldownNotComplete;
            
            HarvestButton = ButtonMaker.MakeHarvestButton();
            HarvestButton.text = Harvestable.CooldownTimeLeft.ToString();
            HarvestButton.clicked += DoButtonClick;

            ButtonsManager.Add(ID, HarvestButton);


        }

        // Update is called once per frame
        void Update()
        {
            CheckCooldown();
            UpdatePosition();
            
            if (State == IUIHarvestable.EState.CooldownNotComplete)
                HarvestButton.text = ((int)Harvestable.CooldownTimeLeft).ToString();
            

        }

        void DebugClick()
        {
            Debug.Log("DebugClick");
        }

        void DoButtonClick()
        {
            if (Harvestable.IsCooldownComplete)
            {
                Harvestable.StartCooldownTimer();
                FindObjectOfType<StartHarvest>().ToHarvest(Harvestable.HarvestBonus);
            }
            else
            {
                ShowAd();
            }
        }

        private void ShowAd()
        {
            Debug.Log("Showing Ad");
            Harvestable.ForceCooldownComplete();
        }

        private void UpdatePosition()
        {
            
            var harvestablePositionScreen =  ActiveCamera.WorldToScreenPoint(gameObject.transform.position);

            HarvestButton.style.left = new Length((harvestablePositionScreen.x), LengthUnit.Pixel);
            HarvestButton.style.top = new Length((Screen.height-harvestablePositionScreen.y), LengthUnit.Pixel);
        }

        private void CheckCooldown()
        {
            if (Harvestable.IsCooldownComplete && State == IUIHarvestable.EState.CooldownNotComplete)
            {
                SetState(IUIHarvestable.EState.CooldownComplete);
            }
        }

        public IUIHarvestable.EState State { get; set; }
       

        public bool SetState(IUIHarvestable.EState state)
        {
            switch (state)
            {
                case IUIHarvestable.EState.CooldownComplete:
                    if (State == IUIHarvestable.EState.CooldownNotComplete)
                        NotCompleteToComplete();
                    return true;
                    break;
                case IUIHarvestable.EState.CooldownNotComplete:
                    if (State == IUIHarvestable.EState.CooldownComplete)
                        CompleteToNotComplete();
                    return true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            return false;
        }

        private void CompleteToNotComplete()
        {
            State = IUIHarvestable.EState.CooldownNotComplete;
            HarvestButton.text = ((int)Harvestable.CooldownTimeLeft).ToString();

        }

        private void NotCompleteToComplete()
        {
            State = IUIHarvestable.EState.CooldownComplete;
            HarvestButton.text = "Harvest!";

        }

        public void Hide()
        {
            HarvestButton.visible = false;
        }

        public void Show()
        {
            HarvestButton.visible = true;
        }
    }

    public interface IUIHarvestable
    {
        public enum EState
        {
            CooldownComplete,
            CooldownNotComplete,
        }

        public IUIHarvestable.EState State { get; }
        public bool SetState(IUIHarvestable.EState state);
    }
}
