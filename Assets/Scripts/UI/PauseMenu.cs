using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using AfterLife.Dimensions;
using AfterLife.Player;
using AfterLife.Systems;

namespace AfterLife.UI
{
    /// <summary>Freezes the game and shows the pause panel when the player presses Esc.</summary>
    public class PauseMenu : MonoBehaviour
    {
        [Header("Panel")]
        [Tooltip("Shown while paused.")]
        [SerializeField]
        private GameObject panel;

        [Tooltip("Closes the menu.")]
        [SerializeField]
        private Button resumeButton;

        [Tooltip("Closes the game.")]
        [SerializeField]
        private Button quitButton;

        [Tooltip("Returns to the main menu.")]
        [SerializeField]
        private Button menuButton;

        [Tooltip("The scene's game manager.")]
        [SerializeField]
        private GameManager gameManager;

        [Header("Input blocked while paused")]
        [Tooltip("The player's movement.")]
        [SerializeField]
        private PlayerController controller;

        [Tooltip("The player's interaction.")]
        [SerializeField]
        private PlayerInteractor interactor;

        [Tooltip("The scene's dimension switching.")]
        [SerializeField]
        private DimensionManager dimensionManager;

        private InputAction pauseAction;
        private Behaviour[] blocked;
        private bool[] wasEnabled;
        private bool isPaused;

        /// <summary>Builds the pause input and hides the panel.</summary>
        private void Awake()
        {
            pauseAction = new InputAction("Pause", InputActionType.Button);
            pauseAction.AddBinding("<Keyboard>/escape");
            pauseAction.AddBinding("<Gamepad>/start");

            blocked = new Behaviour[] { controller, interactor, dimensionManager };
            wasEnabled = new bool[blocked.Length];
            panel.SetActive(false);
        }

        /// <summary>Listens for the pause input and the buttons.</summary>
        private void OnEnable()
        {
            pauseAction.performed += OnPausePerformed;
            pauseAction.Enable();
            resumeButton.onClick.AddListener(Resume);
            menuButton.onClick.AddListener(gameManager.ReturnToMenu);
            quitButton.onClick.AddListener(gameManager.QuitGame);
        }

        /// <summary>Stops listening, and never leaves the game frozen.</summary>
        private void OnDisable()
        {
            pauseAction.performed -= OnPausePerformed;
            pauseAction.Disable();
            resumeButton.onClick.RemoveListener(Resume);
            menuButton.onClick.RemoveListener(gameManager.ReturnToMenu);
            quitButton.onClick.RemoveListener(gameManager.QuitGame);

            if (isPaused)
            {
                Time.timeScale = 1f;
            }
        }

        /// <summary>Releases the input action.</summary>
        private void OnDestroy()
        {
            pauseAction.Dispose();
        }

        /// <summary>Freezes the game and opens the menu.</summary>
        public void Pause()
        {
            // A frozen game that we did not pause belongs to the level intro.
            if (isPaused || Time.timeScale <= 0f)
            {
                return;
            }

            isPaused = true;
            Time.timeScale = 0f;

            for (int i = 0; i < blocked.Length; i++)
            {
                wasEnabled[i] = blocked[i].enabled;
                blocked[i].enabled = false;
            }

            panel.SetActive(true);
            resumeButton.Select();
        }

        /// <summary>Closes the menu and continues the game.</summary>
        public void Resume()
        {
            if (!isPaused)
            {
                return;
            }

            isPaused = false;
            Time.timeScale = 1f;

            for (int i = 0; i < blocked.Length; i++)
            {
                blocked[i].enabled = wasEnabled[i];
            }

            panel.SetActive(false);
        }

        /// <summary>Opens or closes the menu.</summary>
        private void OnPausePerformed(InputAction.CallbackContext context)
        {
            if (isPaused)
            {
                Resume();
                return;
            }

            Pause();
        }
    }
}