using RunLevels.Scoring;
using UnityEngine;

namespace RunLevels
{
    public class TriggeredObstacle : TriggeredInteractable
    {
        [field: SerializeField] protected override Collider CollisionBox { get; set; }
        [field: SerializeField] private bool DestroyOnCollision { get; set; }
        [field: SerializeField] public IRunnerState RunnerState { get; set; }
        [field: SerializeField] private int ScoreRemoved { get; set; }
        [field: SerializeField] private IScoring Scoring { get; set; }
        private bool _wasTriggered;

        void Start()
        {
            SetColliderAsTrigger();
            RunnerState ??= FindObjectOfType<RunnerState>();
            Scoring ??= FindObjectOfType<Scoring.Scoring>();
        }
    
        public override void Interact(GameObject runner)
        {
            // Can only trigger once during the run.
            if (_wasTriggered) return;

            _wasTriggered = true;
            RunnerState.Set(IRunnerState.EState.Damage);
            Scoring.Remove(ScoreRemoved);
        
            if(DestroyOnCollision)
                DestroySelf();
        }
   

  
    }
}
