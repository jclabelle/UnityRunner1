using UnityEngine;

namespace RunLevels.Interactables
{
    public class Damage : TriggeredInteractable
    {

        [field: SerializeField] public IRunnerState RunnerState { get; set; }
        protected override Collider CollisionBox { get; set; }
        private bool _wasTriggered;

        
        // Start is called before the first frame update
        void Start()
        {
            SetColliderAsTrigger();
            RunnerState ??= FindObjectOfType<RunnerState>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }


        public override void Interact(GameObject runner)
        {
            // Can only trigger once during the run.
            if (_wasTriggered) return;
            
            _wasTriggered = true;
            RunnerState.Set(IRunnerState.EState.Damage);


        }
    }
}
