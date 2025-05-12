using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTutorial.Manager;

namespace UnityTutorial.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float AnimBlendSpeed = 8.9f;         // Speed at which animations blend
        [SerializeField] private Transform CameraRoot;              // The root transform for the camera
        [SerializeField] private Transform Camera;                  // The actual camera transform
        [SerializeField] private float UpperLimit = -40f;          // Upper vertical rotation limit for the camera
        [SerializeField] private float BottomLimit = 70f;           // Lower vertical rotation limit for the camera
        [SerializeField] private float MouseSensitivity = 21.9f;    // Mouse sensitivity for camera rotation

        private Rigidbody _playerRigibody;          // Reference to the player's rigidbody component
        private InputManager _inputManager;          // Reference to the custom InputManager
        private Animator _animator;                // Reference to the player's animator component


        private bool _hasAnimator;                // Flag indicating if the player has an animator
        private int _xVelHash;                    // Hash for the "X_Velocity" animator parameter
        private int _yVelHash;                    // Hash for the "Y_Velocity" animator parameter

        private float _xRotation;                  // Current vertical rotation of the camera
        private const float _walkSpeed = 2f;        // Walking speed
        private const float _runSpeed = 6f;         // Running speed
        private Vector2 _currentVelocity;          // Current player velocity
        private Vector3 _playerVelocity;


        private void Start()
        {
            // Get references to necessary components
            _hasAnimator = GetComponent<Animator>(out _animator); //Gets the animator, returns true if there is one
            _playerRigibody = GetComponent<Rigidbody>();
            _inputManager = GetComponent<InputManager>();

            // Store animator parameter hashes for efficiency
            _xVelHash = Animator.StringToHash("X_Velocity");
            _yVelHash = Animator.StringToHash("Y_Velocity");
        }

        private void FixedUpdate()
        {
            // Handle player movement using physics
            Move();
        }

        private void LateUpdate()
        {
            // Handle camera movement after physics
            CamMovement();
        }

        private void Move()
        {
            // Move the player based on input and current state
            if (!_hasAnimator) return; // Don't move if there's no animator

            // Determine the target speed based on whether the player is running
            float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
            if (_inputManager.Move == Vector2.zero) targetSpeed = 0f; // Stop if no movement input

            // Interpolate the player's velocity for smooth acceleration and deceleration
            _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);
            _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);

            // Calculate the velocity difference for applying force
            var xVelDifference = _currentVelocity.x - _playerRigibody.velocity.x;
            var zVelDifference = _currentVelocity.y - _playerRigibody.velocity.z;


            // Apply a force to the rigidbody to achieve the desired velocity
            _playerRigibody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);


            // Update animator parameters with the current velocity
            _animator.SetFloat(_xVelHash, _currentVelocity.x);
            _animator.SetFloat(_yVelHash, _currentVelocity.y);
        }

        private void CamMovement()
        {
            // Control camera rotation based on mouse input
            if (!_hasAnimator) return; // Don't move camera if no animator

            // Get mouse input from the InputManager
            var mouseX = _inputManager.Look.x;
            var mouseY = _inputManager.Look.y;

            // Keep the camera position synced with the CameraRoot
            Camera.position = CameraRoot.position;

            // Calculate the new vertical rotation
            _xRotation -= mouseY * MouseSensitivity * Time.deltaTime;
            _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit); // Clamp vertical rotation

            // Apply the vertical rotation to the camera
            Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);

            // Apply horizontal rotation to the player
            _playerRigibody.MoveRotation(_playerRigibody.rotation * Quaternion.Euler(0, mouseX * MouseSensitivity * Time.smoothDeltaTime, 0));
        }

        //Gets the animator, returns true if there is one
        private bool GetComponent<T>(out T component) where T : Component
        {
            component = base.GetComponent<T>();
            return component != null;
        }
    }
}