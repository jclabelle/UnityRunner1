using UnityEngine;

namespace RunLevels
{
    public class LevelWinTrigger : TriggeredInteractable
    {
        [field: SerializeField] public IRunnerState RunnerState { get; set; }
        [field: SerializeField] public ILevelState LevelState { get; set; }
        protected override Collider CollisionBox { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            SetColliderAsTrigger();
            LevelState ??= FindObjectOfType<LevelState>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public override void Interact(GameObject runner)
        {
            LevelState.Set(ILevelState.EState.Win);
        }
    }
}
