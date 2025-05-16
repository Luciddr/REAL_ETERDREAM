using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTutorial.Manager;

namespace UnityTutorial.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float AnimBlendSpeed = 8.9f;        
        [SerializeField] private Transform CameraRoot;             
        [SerializeField] private Transform Camera;                  
        [SerializeField] private float UpperLimit = -40f;          
        [SerializeField] private float BottomLimit = 70f;           
        [SerializeField] private float MouseSensitivity = 21.9f;    

        private Rigidbody _playerRigibody;        
        private InputManager _inputManager;         
        private Animator _animator;               

        private bool _hasAnimator;            
        private int _xVelHash;                  
        private int _yVelHash;                

        private float _xRotation;                 
        private const float _walkSpeed = 2f;       
        private const float _runSpeed = 6f;      
        private Vector2 _currentVelocity;       
        private Vector3 _playerVelocity;


        private void Start()
        {
           
            _hasAnimator = GetComponent<Animator>(out _animator); 
            _playerRigibody = GetComponent<Rigidbody>();
            _inputManager = GetComponent<InputManager>();

            
            _xVelHash = Animator.StringToHash("X_Velocity");
            _yVelHash = Animator.StringToHash("Y_Velocity");
        }

        private void FixedUpdate()
        {
            
            Move();
        }

        private void LateUpdate()
        {
            
            CamMovement();
        }

        private void Move()
        {
            
            if (!_hasAnimator) return; 

           
            float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
            if (_inputManager.Move == Vector2.zero) targetSpeed = 0f; 

            
            _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);
            _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);

           
            var xVelDifference = _currentVelocity.x - _playerRigibody.velocity.x;
            var zVelDifference = _currentVelocity.y - _playerRigibody.velocity.z;


            
            _playerRigibody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);


           
            _animator.SetFloat(_xVelHash, _currentVelocity.x);
            _animator.SetFloat(_yVelHash, _currentVelocity.y);
        }

        private void CamMovement()
        {
           
            if (!_hasAnimator) return;

            var mouseX = _inputManager.Look.x;
            var mouseY = _inputManager.Look.y;

            
            Camera.position = CameraRoot.position;

            _xRotation -= mouseY * MouseSensitivity * Time.deltaTime;
            _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

           
            Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);

           
            _playerRigibody.MoveRotation(_playerRigibody.rotation * Quaternion.Euler(0, mouseX * MouseSensitivity * Time.smoothDeltaTime, 0));
        }


        private bool GetComponent<T>(out T component) where T : Component
        {
            component = base.GetComponent<T>();
            return component != null;
        }
    }
}