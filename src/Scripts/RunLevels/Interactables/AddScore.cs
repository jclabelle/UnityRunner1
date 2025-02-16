using UnityEngine;

namespace RunLevels.Interactables
{
    public class AddScore : TriggeredInteractable
    {
        [field: SerializeField] private Scoring.Scoring Scoring { get; set; }
        protected override Collider CollisionBox { get; set; }
        [field: SerializeField] public int ScoreAdded { get; set; }
        private bool _wasTriggered;


        public void Start()
        {
            SetColliderAsTrigger();
            Scoring ??= FindObjectOfType<Scoring.Scoring>();
        }


        public override void Interact(GameObject runner)
        {
            if (_wasTriggered) return;

            _wasTriggered = true;
            Scoring.Add(ScoreAdded);
            if(GetComponent<DestroyOnInteract>() is {} destroyOnInteract)
                destroyOnInteract.DestroySelf();
        }
        
        
    }
}
