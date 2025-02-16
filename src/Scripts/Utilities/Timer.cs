using System;
using UnityEngine;


namespace Utilities
{
    public class Timer : MonoBehaviour, ITimer
    {

        public float StartTime  { get; protected set; }
        public float StopTime { get; protected set; }
        [field: SerializeField] public ITimer.EState State { get; set; }
        public float ElapsedTime { get; protected set; }
        private Action _callback;
        private float _callbackTime;
 
        public DateTime StartRealTime { get; set; }
        [field: SerializeField] public ITimer.EDurationType DurationType { get; set; }


        // Update is called once per frame
        protected void Update()
        {
            if (State == ITimer.EState.Running)
            {
                IncrementElapsedTime();
                DoCallback();
            } 
            

        }
        
        public void SetCallbackAt(float elapsedTime, Action callback)
        {
            _callback = callback;
            _callbackTime = elapsedTime;
        }

        public void OverrideTimes(float startTime, float stopTime, float elapsedTime)
        {
            StartTime = startTime;
            StopTime = stopTime;
            ElapsedTime = elapsedTime;
        }
        
        public void OverrideTimes(float startTime, float stopTime, float elapsedTime, DateTime startRealTime)
        {
            StartTime = startTime;
            StopTime = stopTime;
            ElapsedTime = elapsedTime;
            StartRealTime = startRealTime;
        }

        public void ForceSetState(ITimer.EState forcedState)
        {
            State = forcedState;
        }




        private void DoCallback()
        {
            if (ElapsedTime < _callbackTime || _callback is null)
                return;
            
            _callback();
            _callback = null;
        }

        // Returns true on successful state change.
        public bool SetState(ITimer.EState newState)
        {
            switch (newState)
            {
                case ITimer.EState.Stopped:
                    if (State != ITimer.EState.Stopped)
                    {
                        DoStop();
                        return true;
                    }

                    DoStop();
                    break;
                case ITimer.EState.Running:
                    if (State == ITimer.EState.Paused)
                    {
                        PausedToRunning();
                        return true;
                    }

                    if (State == ITimer.EState.Stopped)
                    {
                        StoppedToRunning();
                        return true;
                    }
                    break;
                case ITimer.EState.Paused:
                    if (State == ITimer.EState.Running)
                    {
                        RunningToPause();
                        return true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }

            return false;
        }

        protected void RunningToPause()
        {
            State = ITimer.EState.Paused;
        }

        protected void StoppedToRunning()
        {
            State = ITimer.EState.Running;
            StartTime = Time.time;
            StartRealTime = DateTime.Now;
            ElapsedTime = 0;
        }

        protected void DoStop()
        {
            State = ITimer.EState.Stopped;
            StopTime = ElapsedTime;
        }

        protected void PausedToRunning()
        {
            State = ITimer.EState.Running;
        }

        protected void IncrementElapsedTime()
        {
            if (DurationType == ITimer.EDurationType.GameTime)
                ElapsedTime += Time.deltaTime;

            if (DurationType == ITimer.EDurationType.RealTime)
                ElapsedTime = (float)(DateTime.Now - StartRealTime).TotalSeconds;
        }
    }

    public interface ITimer
    {
        public enum EState
        {
            Stopped,
            Running,
            Paused,
        }

        public enum EDurationType
        {
            RealTime = 0, // Timer will increment regardless of the game running or not and scene being open or not
            GameTime = 1, // Timer will increment while the scene is open
        }

        public bool SetState(EState newState); // Return false on invalid state change
        public float ElapsedTime { get; } 
        public float StartTime { get; }   
        public DateTime StartRealTime { get; }
        public float StopTime { get; } // Elapsed time when Timer was last stopped.
        public EState State { get; set; }
        public EDurationType DurationType { get; set; }

        public void SetCallbackAt(float elapsedTime, System.Action action); // Callback action when the timer hits elapsedTime
        public void OverrideTimes(float startTime, float stopTime, float elapsedTime);
        public void OverrideTimes(float startTime, float stopTime, float elapsedTime, DateTime startRealTime);
        public void ForceSetState(EState forcedState); // Bypasses transitions.


    }
}
