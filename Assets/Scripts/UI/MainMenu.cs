using UnityEngine;
using UnityEngine.UI;
using AfterLife.Systems;

namespace AfterLife.UI
{
    /// <summary>Connects the main menu buttons to the game manager.</summary>
    public class MainMenu : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("The menu scene's game manager.")]
        [SerializeField]
        private GameManager gameManager;

        [Header("Buttons")]
        [Tooltip("Starts from the first level.")]
        [SerializeField]
        private Button newGameButton;

        [Tooltip("Loads the saved level. Greyed out without a save.")]
        [SerializeField]
        private Button continueButton;

        [Tooltip("Closes the game.")]
        [SerializeField]
        private Button quitButton;

        /// <summary>Listens for clicks.</summary>
        private void OnEnable()
        {
            newGameButton.onClick.AddListener(gameManager.NewGame);
            continueButton.onClick.AddListener(gameManager.Continue);
            quitButton.onClick.AddListener(gameManager.QuitGame);
        }

        /// <summary>Enables Continue only with a save, and selects the likeliest button.</summary>
        private void Start()
        {
            continueButton.interactable = SaveSystem.HasSave;
            Button first = continueButton.interactable ? continueButton : newGameButton;
            first.Select();
        }

        /// <summary>Stops listening.</summary>
        private void OnDisable()
        {
            newGameButton.onClick.RemoveListener(gameManager.NewGame);
            continueButton.onClick.RemoveListener(gameManager.Continue);
            quitButton.onClick.RemoveListener(gameManager.QuitGame);
        }
    }
}