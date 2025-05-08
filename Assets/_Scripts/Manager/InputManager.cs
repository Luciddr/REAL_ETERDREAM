using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace UnityTutorial.Manager
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput; // แก้ไข Serialzefield เป็น SerializeField

        public Vector2 Move { get; private set; }

        public Vector2 Look { get; private set; }

        public bool Run { get; private set; }


        private InputActionMap _currentMap;

        private InputAction _moveAction;

        private InputAction _lookAction;

        private InputAction _runAction;


        private void Awake()
        {
            if (playerInput == null)
            {
                Debug.LogError("PlayerInput is not assigned in the Inspector!");
                enabled = false; // ปิดการทำงานของสคริปต์หากไม่มี PlayerInput
                return;
            }
            HideCursor();

            _currentMap = playerInput.currentActionMap;
            _moveAction = _currentMap.FindAction("Move");
            _lookAction = _currentMap.FindAction("Look");
            _runAction = _currentMap.FindAction("Run");

            _moveAction.performed += OnMove; // เปลี่ยนชื่อ onMove เป็น OnMove ตามมาตรฐาน C#
            _lookAction.performed += OnLook; // เปลี่ยนชื่อ onLook เป็น OnLook ตามมาตรฐาน C#
            _runAction.performed += OnRun;   // เปลี่ยนชื่อ onRun เป็น OnRun ตามมาตรฐาน C#

            _moveAction.canceled += OnMove;
            _lookAction.canceled += OnLook;
            _runAction.canceled += OnRun;
        }
        private void HideCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
        }

        private void OnRun(InputAction.CallbackContext context)
        {
            Run = context.ReadValueAsButton();
        }

        private void OnEnable()
        {
            if (playerInput != null && playerInput.currentActionMap != null)
            {
                playerInput.currentActionMap.Enable();
            }
            else
            {
                Debug.LogWarning("PlayerInput or its Action Map is null. Input might not be enabled.");
            }
        }

        private void OnDisable()
        {
            if (playerInput != null && playerInput.currentActionMap != null)
            {
                playerInput.currentActionMap.Disable();
            }
        }
    }
}