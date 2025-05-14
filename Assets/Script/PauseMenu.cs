using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityTutorial.Manager
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenuUI;
        [SerializeField] private PlayerInput playerInput;

        private InputActionMap _actionMap;
        private InputAction _pauseAction;

        private bool _isPaused = false;

        private void Awake()
        {
            _actionMap = playerInput.currentActionMap;
            _pauseAction = _actionMap.FindAction("Pause"); // ใช้ Left Alt หรือ Right Alt

            _pauseAction.performed += OnPause;
        }

        private void OnEnable()
        {
            _actionMap.Enable();
        }

        private void OnDisable()
        {
            _actionMap.Disable();
        }

        private void OnPause(InputAction.CallbackContext context)
        {
            if (_isPaused)
                Resume();
            else
                Pause();
        }

        private void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _isPaused = true;
        }

        private void Resume()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _isPaused = false;
        }

        public void QuitGame()
        {
            Debug.Log("Quit Game");
            Application.Quit();
        }
    }
}
