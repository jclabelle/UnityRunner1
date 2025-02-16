using System;
using UnityEngine;

namespace RunLevels
{
    [RequireComponent(typeof(AudioSource))]
    public class LevelMusic : MonoBehaviour, ILevelMusic
    {
        [field: SerializeField] private AudioClip Walk { get; set; }
        [field: SerializeField] private AudioClip PreRun { get; set; }
        [field: SerializeField] private AudioClip Win { get; set; }
        [field: SerializeField] private AudioClip Lose { get; set; }
        [field: SerializeField] private AudioSource AudioSource { get; set; }
        public ILevelMusic.EState CurrentState { get; set; }

        private void Awake()
        {
            AudioSource ??= GetComponent<AudioSource>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Set(ILevelMusic.EState newState)
        {
            switch (newState)
            {
                case ILevelMusic.EState.PreRun:
                    DoPreRun();
                    break;
                case ILevelMusic.EState.Walk:
                    DoWalk();
                    break;
                case ILevelMusic.EState.Lose:
                    DoLose();
                    break;
                case ILevelMusic.EState.Win:
                    DoWin();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }
        
        private void DoPreRun()
        {
            AudioSource.clip = PreRun;
            AudioSource.loop = true;
            AudioSource.Play();
        }
    
        private void DoWalk()
        {
            AudioSource.clip = Walk;
            AudioSource.loop = true;
            AudioSource.Play();
        }

        private void DoLose()
        {
            AudioSource.clip = Lose;
            // AudioSource.loop = true;
            AudioSource.Play();
        }

        private void DoWin()
        {
            AudioSource.clip = Win;
            AudioSource.loop = true;
            AudioSource.Play();
        }

    }

    public interface ILevelMusic
    {
        public enum EState
        {
            PreRun,
            Walk,
            Win,
            Lose,
        }

        public void Set(EState newState);
        public EState CurrentState { get; set; }
    }
}
