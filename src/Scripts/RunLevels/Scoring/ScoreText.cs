using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace RunLevels.Scoring
{
    [RequireComponent(typeof(TextMeshPro), typeof(AudioSource))]
    public class ScoreText : MonoBehaviour
    {
        // [field: SerializeField] private GameObject Camera { get; set; }
        public RunnerController Runner { get; set; }
        [field: SerializeField] private float FontSizeTweenDuration { get; set; }
        [field: SerializeField] private float HeightTweenDuration { get; set; }
        [field: SerializeField] private float StartFontSize { get; set; }
        [field: SerializeField] private float EndFontSize { get; set; }
        [field: SerializeField] private float DistanceFromSubject { get; set; }
        [field: SerializeField] private float StartHeight { get; set; }
        [field: SerializeField] private float EndHeight { get; set; }
        private float _currentHeightOffset;
        [field: SerializeField] private float LeftOffset { get; set; }
        private Vector3 _currentOffset;
        [field: SerializeField] public Vector3 OffsetRotY0 { get; set; }

        [field: SerializeField] private float FadeDuration { get; set; }
        [field: SerializeField] private float FadeStart { get; set; }
        private float _textTransparency;


        public Action<ScoreText> RemoveSelfFromActiveTexts;
    

        private float CurrentHeight { get => transform.position.y; }
        [field: SerializeField] private float LifetimeDuration { get; set; }
        private float ElapsedTime { get; set; }
    
        // Set by Scoring when instantiated
        public Color Color { get; set; }
        public string Score { get; set; }
        [field: SerializeField] public bool IsTemplate { get; set; }
        public AudioClip PickUpSound { get; set; }

        private TextMeshPro _text;


        private void Awake()
        {
            _currentOffset = OffsetRotY0;
            _currentHeightOffset = 0;
            _textTransparency = 1f;
        }

        // Start is called before the first frame update
        void Start()
        {
        
            if (IsTemplate) return;

            Runner ??= GameObject.FindWithTag("Player").GetComponentInChildren<RunnerController>();


            _text = GetComponent<TextMeshPro>();
            _text.fontSize = StartFontSize;
            _text.faceColor = Color;
            _text.text = Score;
            _text.outlineColor = Color.black;
            _text.outlineWidth = 0.1f;
            DOTween.To(()=> _text.fontSize, x=> _text.fontSize = x , EndFontSize, FontSizeTweenDuration);
        
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(PickUpSound);
        
            SetRunSurface(Runner.CurrentRunSurface);
            transform.position = Runner.transform.position + _currentOffset;
        
            DOTween.To(()=> _currentHeightOffset, x => _currentHeightOffset = x, EndHeight-StartHeight, HeightTweenDuration);


        }

        // Update is called once per frame
        void Update()
        {
            if (IsTemplate) return;
        
            ElapsedTime += Time.deltaTime;
            if(ElapsedTime > LifetimeDuration)
                Destroy(gameObject);
        
            transform.position = Runner.transform.position + _currentOffset + new Vector3(0, _currentHeightOffset, 0);
        
            if(ElapsedTime > FadeStart && _textTransparency >= 1f)
                DOTween.To(()=> _textTransparency, x => _textTransparency = x, 0, FadeDuration);

            var alpha = (byte)(255 * _textTransparency);
            var newFaceColor = new Color32(_text.faceColor.r, _text.faceColor.g, _text.faceColor.b, alpha);
            var newOutlineColor = new Color32(_text.outlineColor.r, _text.outlineColor.g, _text.outlineColor.b, alpha);
            // Debug.Log(newOutlineColor);

            _text.faceColor = newFaceColor;
            _text.outlineColor = newOutlineColor;

        }

        private void OnDestroy()
        {
            RemoveSelfFromActiveTexts(this);
        }
    
        public void NewRunSurface(RunSurface runSurface, float rotationSpeed)
        {
        
            var newRotation = RunSurface.Rotation(runSurface);
            transform.DORotate(newRotation, rotationSpeed);
        
            var newOffset = RunSurface.Offsets(runSurface, CurrentHeight, DistanceFromSubject, LeftOffset);
            DOTween.To(()=> _currentOffset, x=> _currentOffset = x, newOffset, rotationSpeed);
        
        }
    
        public void SetRunSurface(RunSurface runSurface)
        {
        
            transform.Rotate(RunSurface.Rotation(runSurface));
        
            _currentOffset = RunSurface.Offsets(runSurface, StartHeight, DistanceFromSubject, LeftOffset);
        
        
        }
    }
}
