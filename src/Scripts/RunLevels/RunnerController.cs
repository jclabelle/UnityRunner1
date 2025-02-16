using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

namespace RunLevels
{
    public class RunnerController : MonoBehaviour, IRunnerControl
    {
        private Vector3 _move;
        private Vector3 _moveLeft;    
        private Vector3 _moveRight;    

        [field: SerializeField] private float MaxLateralDistance { get; set; }
        [field: SerializeField] public RunSurface CurrentRunSurface { get; set; }
        private float CurrentForwardSpeed { get; set; }
        
  

        [field: SerializeField] public float ForwardSpeed { get; set; }
        [field: SerializeField] public float LateralSpeed { get; set; }

        [field: SerializeField] public float RotationSpeed { get; set; }
    
        [field: SerializeField] public ChaseCam ChaseCam { get; set; }
        [field: SerializeField] public AnimationState AnimationStates { get; set; }
        [field: SerializeField] private Scoring.Scoring Scoring { get; set; }

        public Transform ParentTransform { get; set; }
        public bool canMoveLeftOrRight = true;

        private float _defaultMaxLateralDistance;
        private float _defaultForwardSpeed;
        private float _defaultLateralSpeed;
        private float _defaultRotationSpeed;
        private GameObject _playerSpawnPoint;


        private void Awake()
        {
            _playerSpawnPoint = GameObject.FindWithTag("PlayerSpawnPoint");
            ParentTransform = transform.parent;

            SetDefaults();
            CurrentForwardSpeed = 0;
        }

        // Start is called before the first frame update
        void Start()
        {

            Scoring ??= FindObjectOfType<Scoring.Scoring>();
        
            Assert.IsNotNull(Scoring, "Scoring missing from Scene");
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void SetDefaults()
        {
            _defaultMaxLateralDistance = MaxLateralDistance;
            _defaultForwardSpeed = ForwardSpeed;
            _defaultLateralSpeed = LateralSpeed;
            _defaultRotationSpeed = RotationSpeed;
        }

        public void RestoreDefaults()
        {
            MaxLateralDistance = _defaultMaxLateralDistance;
            ForwardSpeed = _defaultForwardSpeed;
            LateralSpeed = _defaultLateralSpeed;
            RotationSpeed = _defaultRotationSpeed;

        }

        public void MoveToSpawnPoint()
        {
            ParentTransform.position = _playerSpawnPoint.transform.position;
        }

        private void FixedUpdate()
        {
            ParentTransform.position += _move * (Time.deltaTime * CurrentForwardSpeed);

            if (canMoveLeftOrRight)
            {
                if (Keyboard.current.leftArrowKey.isPressed)
                    ParentTransform.position += _moveLeft;

                if (Keyboard.current.rightArrowKey.isPressed)
                    ParentTransform.position += _moveRight;
            }

            StayOnRunSurface();
        }

       

        private void StayOnRunSurface()
        {
            var runner = ParentTransform.position;
            var maxX = CurrentRunSurface.bounds.center.x + MaxLateralDistance;
            var minX = CurrentRunSurface.bounds.center.x - MaxLateralDistance;
            var maxZ = CurrentRunSurface.bounds.center.z + MaxLateralDistance;
            var minZ = CurrentRunSurface.bounds.center.z - MaxLateralDistance;
        
            switch (CurrentRunSurface.Type)
            {
                case RunSurface.EType.Y0:
                {
                    if(runner.x < minX)
                        SetRunnerX(minX);
                    else if( runner.x > maxX)
                        SetRunnerX(maxX);
                    break;
                }
            
                case RunSurface.EType.Y90:
                {
                    if(runner.z < minZ)
                        SetRunnerZ(minZ);
                    else if( runner.z > maxZ)
                        SetRunnerZ(maxZ);
                    break;
                }
            
                case RunSurface.EType.Y180:
                {
                    if(runner.x < minX)
                        SetRunnerX(minX);
                    else if( runner.x > maxX)
                        SetRunnerX(maxX);
                    break;
                }
            
                case RunSurface.EType.Y270:
                {
                    if(runner.z < minZ)
                        SetRunnerZ(minZ);
                    else if( runner.z > maxZ)
                        SetRunnerZ(maxZ);
                    break;
                }
            }
        }

        private void SetRunnerX(float x)
        {
            var position = ParentTransform.position;
            ParentTransform.position = new Vector3(x, position.y, position.z);
        }
    
        private void SetRunnerZ(float z)
        {
            var position = ParentTransform.position;
            ParentTransform.position = new Vector3(position.x, position.y, z);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<RunSurface>() is RunSurface runSurface)
                ChangeRunSurface(runSurface);
        
            if(other.GetComponent<RunnerInteractable>() is IRunnerInteractable interactable && interactable.IsEnabled)
                interactable.Interact(ParentTransform.gameObject);

            var colliderInteractables = other.GetComponents<IRunnerInteractable>();
            if(colliderInteractables.Length > 0)
                foreach (var colliderInteractable in colliderInteractables)
                    if(colliderInteractable.IsEnabled)
                        colliderInteractable.Interact(ParentTransform.gameObject);

        }

        private void ChangeRunSurface(RunSurface runSurface)
        {
            var newMove = RunSurface.MoveForward(runSurface);
            
            DOTween.To(()=> _move, x=> _move = x , newMove, RotationSpeed);

            SetMoves(runSurface);
            CurrentRunSurface = runSurface;
            var newRotation = RunSurface.Rotation((runSurface));
            ParentTransform.DORotate(newRotation, RotationSpeed);
            ChaseCam.NewRunSurface(runSurface, RotationSpeed);
            Scoring.ChangeScoreTextRunSurface(runSurface, RotationSpeed);
        }

        private void SetMoves(RunSurface runSurface)
        {
            _moveLeft = RunSurface.MoveLeft(runSurface);
            _moveLeft *= Time.deltaTime * LateralSpeed;
            _moveRight = RunSurface.MoveRight(runSurface);
            _moveRight *= Time.deltaTime * LateralSpeed;
        }

        public void SetForwardSpeed(float newSpeed, float easeDuration = 0f) =>
            DOTween.To(() => CurrentForwardSpeed, x => CurrentForwardSpeed = x, newSpeed, easeDuration);

        
        public void SetLateralSpeed(float newSpeed, float easeDuration = 0f) =>             
            DOTween.To(() => LateralSpeed, x => LateralSpeed = x, newSpeed, easeDuration);

        public void StopLateralMovement()
            => canMoveLeftOrRight = false;

        public void StartLateralMovement()
            => canMoveLeftOrRight = true;
    }

    public interface IRunnerControl
    {
        public void SetForwardSpeed(float newSpeed, float easeDuration = 0f);
        public void SetLateralSpeed(float newSpeed, float easeDuration = 0f);
        public float ForwardSpeed { get; }
        public float LateralSpeed { get; }
        public void StopLateralMovement();
        public void StartLateralMovement();
        public void RestoreDefaults();
        public void MoveToSpawnPoint();
    }
}