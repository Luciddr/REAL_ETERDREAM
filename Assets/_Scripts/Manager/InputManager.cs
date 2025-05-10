using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityTutorial.Manager
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput; // Reference to the PlayerInput component in the scene

        // Public properties to expose input values to other scripts
        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public bool Run { get; private set; }

        private InputActionMap _currentMap; // Stores the currently active Input Action Map
        private InputAction _moveAction;    // Stores the "Move" action
        private InputAction _lookAction;    // Stores the "Look" action
        private InputAction _runAction;     // Stores the "Run" action

        private void Awake()
        {
            // Initialize input actions and hide the cursor
            InitializeInputActions();
            HideCursor();
        }

        private void InitializeInputActions()
        {
            // Get the current Action Map from the PlayerInput component.
            _currentMap = playerInput.currentActionMap;

            // Find the specific actions within the current Action Map.  Important: Ensure these actions exist in your Input Actions asset!
            _moveAction = _currentMap.FindAction("Move");
            _lookAction = _currentMap.FindAction("Look");
            _runAction = _currentMap.FindAction("Run");

            // Subscribe to input action events.  We use both performed and canceled to get continuous updates.
            _moveAction.performed += OnMove;
            _moveAction.canceled += OnMove; //  Crucial:  Handle release to stop movement

            _lookAction.performed += OnLook;
            _lookAction.canceled += OnLook;

            _runAction.performed += OnRun;
            _runAction.canceled += OnRun;  // Crucial: Handle release to stop running
        }

        private void HideCursor()
        {
            // Hide and lock the cursor for a typical game experience (e.g., FPS, TPS).
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            // Read the movement input value (e.g., from joystick or WASD keys).
            Move = context.ReadValue<Vector2>();
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            // Read the look input value (e.g., from mouse or right joystick).
            Look = context.ReadValue<Vector2>();
        }

        private void OnRun(InputAction.CallbackContext context)
        {
            // Read the run input value (e.g., from a button press).
            Run = context.ReadValueAsButton();
        }

        private void OnEnable()
        {
            // Enable the current Action Map when the InputManager is enabled.
            _currentMap.Enable();
        }

        private void OnDisable()
        {
            // Disable the current Action Map when the InputManager is disabled.  This is good practice for cleanup.
            _currentMap.Disable();
        }
    }
}
