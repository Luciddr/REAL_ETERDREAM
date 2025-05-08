using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTutorial.Manager;

namespace UnityTutorial.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {

        private Rigidbody _playerRigibody;
        private InputManager _inputManager;
        private Animator _animator; // เปลี่ยนชนิดเป็น Animator
        private bool _hasAnimator;
        private int _xVelHash;
        private int _yVelHash; // เปลี่ยนชื่อ _vVelHash เป็น _yVelHash ให้สื่อถึงแกน Y

        private const float _walkSpeed = 2f;
        private const float _runSpeed = 6f;
        private Vector2 _currentVelocity = Vector2.zero; // Initialized to zero

        private void Start()
        {
            _hasAnimator = TryGetComponent<Animator>(out _animator);
            _playerRigibody = GetComponent<Rigidbody>();
            _inputManager = GetComponent<InputManager>();

            if (_hasAnimator)
            {
                _xVelHash = Animator.StringToHash("X_Velocity");
                _yVelHash = Animator.StringToHash("Y_Velocity");
            }
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (!_hasAnimator) return;

            float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
            if (_inputManager.Move == Vector2.zero) targetSpeed = 0f; // Set target speed to 0 when no input

            _currentVelocity.x = targetSpeed * _inputManager.Move.x;
            _currentVelocity.y = targetSpeed * _inputManager.Move.y;

            // Calculate the velocity change needed to reach the target speed
            Vector3 targetVelocity = transform.TransformVector(new Vector3(_currentVelocity.x, 0, _currentVelocity.y));
            Vector3 velocityChange = targetVelocity - _playerRigibody.velocity;

            // Apply a force to reach the target velocity
            _playerRigibody.AddForce(velocityChange, ForceMode.VelocityChange);

            _animator.SetFloat(_xVelHash, _currentVelocity.x);
            _animator.SetFloat(_yVelHash, _currentVelocity.y);
        }
    }
}