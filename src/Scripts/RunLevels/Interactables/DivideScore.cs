using UnityEngine;

namespace RunLevels.Interactables
{
    public class DivideScore : TriggeredInteractable
    {
        [field: SerializeField] private Scoring.Scoring Scoring { get; set; }
        protected override Collider CollisionBox { get; set; }
        [field: SerializeField] public float ScoreDivisor { get; set; }
        private bool _wasTriggered;

        public override void Interact(GameObject runner)
        {
            if (_wasTriggered) return;

            _wasTriggered = true;
            Scoring.Divide(ScoreDivisor);
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