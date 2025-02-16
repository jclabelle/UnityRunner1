using UnityEngine;

namespace RunLevels.Interactables
{
    public class MultiplyScore : TriggeredInteractable
    {
        [field: SerializeField] private Scoring.Scoring Scoring { get; set; }
        protected override Collider CollisionBox { get; set; }
        [field: SerializeField] public float ScoreMultiplier { get; set; }
        private bool _wasTriggered;

        public override void Interact(GameObject runner)
        {
            if (_wasTriggered) return;

            _wasTriggered = true;
            Scoring.Multiply(ScoreMultiplier);
            if(GetComponent<DestroyOnInteract>() is {} destroyOnInteract)
                destroyOnInteract.DestroySelf();
        }

        // Start is called before the first frame update
        void Start()
        {
            SetColliderAsTrigger();
            Scoring ??= FindObjectOfType<Scoring.Scoring>();
        }

   
    }
}
