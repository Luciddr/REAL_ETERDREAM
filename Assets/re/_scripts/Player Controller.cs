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
        private const float _runSpeed = 8f;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _hasAnimator = _animator != null;

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

            // --- กล้องเป็นตัวกำหนดทิศทางการเคลื่อนที่ ---
            Vector3 inputDir = new Vector3(_inputManager.Move.x, 0f, _inputManager.Move.y);
            Vector3 camForward = Camera.forward;
            Vector3 camRight = Camera.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = (camForward * inputDir.z + camRight * inputDir.x).normalized;
            Vector3 targetVelocity = moveDir * targetSpeed;

            // คำนวณความต่างของความเร็ว
            Vector3 currentVelocity = new Vector3(_playerRigibody.velocity.x, 0f, _playerRigibody.velocity.z);
            Vector3 velocityDiff = targetVelocity - currentVelocity;

            // Apply movement
            _playerRigibody.AddForce(velocityDiff, ForceMode.VelocityChange);

            // อัปเดตค่าให้ Animator (ใช้ local space)
            Vector3 localVelocity = transform.InverseTransformDirection(targetVelocity);
            _animator.SetFloat(_xVelHash, Mathf.Lerp(_animator.GetFloat(_xVelHash), localVelocity.x, AnimBlendSpeed * Time.fixedDeltaTime));
            _animator.SetFloat(_yVelHash, Mathf.Lerp(_animator.GetFloat(_yVelHash), localVelocity.z, AnimBlendSpeed * Time.fixedDeltaTime));
        }

        private void CamMovement()
        {
            if (!_hasAnimator) return;

            var mouseX = _inputManager.Look.x;
            var mouseY = _inputManager.Look.y;

            Camera.position = CameraRoot.position;

            _xRotation -= mouseY * MouseSensitivity * Time.deltaTime;
            _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

            Camera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            _playerRigibody.MoveRotation(_playerRigibody.rotation * Quaternion.Euler(0f, mouseX * MouseSensitivity * Time.deltaTime, 0f));
        }
    }
}
