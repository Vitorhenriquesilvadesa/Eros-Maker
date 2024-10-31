using System;
using Descriptors;
using Event;
using EventSystem;
using Mapping;
using Model;
using Provider;
using UnityEngine;

namespace Controller
{
    public class MovementController : MonoBehaviour
    {
        private Player _player;
        [SerializeField] private CharacterController characterController;
        private CameraDataDescriptor _cameraDataDescriptor;
        private CameraConfiguration _cameraConfiguration;
        [SerializeField] private float fallSpeed = 0f;
        [SerializeField] private float gravity = 9.8f;

        public bool IsMoving { get; private set; }
        public bool IsRunning { get; private set; }

        private void Start()
        {
            _player = SingletonModelProvider.Get<Player>();
            _cameraDataDescriptor = new CameraDataDescriptor();
            _cameraConfiguration = new CameraConfiguration();
        }

        private void Update()
        {
            CheckIsRunning();
            Move();
            ApplyGravity();
        }

        private void CheckIsRunning()
        {
            if (InputController.GetAction(KeyboardAction.Run) && IsMoving)
            {
                IsRunning = true;
                EventAPI.DispatchEvent(new SetFieldOfViewEvent(_cameraConfiguration.RunFov));
            }

            if (InputController.GetTriggerRelease(KeyboardAction.Run) || !IsMoving)
            {
                IsRunning = false;
                EventAPI.DispatchEvent(new SetFieldOfViewEvent(_cameraConfiguration.WalkFov));
            }
        }

        private void Move()
        {
            Vector3 direction = GetNormalizedMovementVector();
            Vector3 transformedDirection = _cameraDataDescriptor.Transform.TransformDirection(direction);
            transformedDirection.y = 0f;
            transformedDirection = Vector3.Normalize(transformedDirection);

            if (IsRunning)
            {
                characterController.Move(_player.MovementSpeed * _player.RunSpeedMultiplier * Time.deltaTime *
                                         transformedDirection);
            }
            else
            {
                characterController.Move(_player.MovementSpeed * _player.WalkSpeedMultiplier * Time.deltaTime *
                                         transformedDirection);
            }
        }

        private Vector3 GetNormalizedMovementVector()
        {
            Vector3 rawMovementVector = Vector3.zero;

            if (InputController.GetAction(KeyboardAction.Up)) rawMovementVector += Vector3.forward;
            if (InputController.GetAction(KeyboardAction.Down)) rawMovementVector += Vector3.back;
            if (InputController.GetAction(KeyboardAction.Left)) rawMovementVector += Vector3.left;
            if (InputController.GetAction(KeyboardAction.Right)) rawMovementVector += Vector3.right;

            IsMoving = rawMovementVector != Vector3.zero;

            return Vector3.Normalize(rawMovementVector);
        }
        
        private void ApplyGravity()
        {
            if (characterController.isGrounded)
            {
                fallSpeed = 0f;
            }
            else
            {
                fallSpeed -= gravity * Time.deltaTime;
            }

            Vector3 gravityMovement = new Vector3(0, fallSpeed, 0);
            characterController.Move(gravityMovement * Time.deltaTime);
        }

    }
}