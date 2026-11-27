using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using AfterLife.Data;
using AfterLife.Interactables;
using AfterLife.UI;

namespace AfterLife.Systems
{
    /// <summary>Starts, continues and advances the game by loading scenes.</summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("The level order and menu names.")]
        [SerializeField]
        private GameSettings settings;

        [Tooltip("The level in this scene. Empty in the main menu.")]
        [SerializeField]
        private LevelData currentLevel;

        [Tooltip("Black overlay used between scenes.")]
        [SerializeField]
        private ScreenFader fader;

        [Header("Carried to the next level (levels only)")]
        [Tooltip("The player's health.")]
        [SerializeField]
        private Health playerHealth;

        [Tooltip("Hit points gained when entering a new level.")]
        [SerializeField, Min(0)]
        private int healthPerLevel = 1;

        [Tooltip("The scene's orb counter.")]
        [SerializeField]
        private OrbCounter orbCounter;

        private bool isLoading;

        /// <summary>Listens for the level exit.</summary>
        private void OnEnable()
        {
            LevelExit.PlayerReachedExit += OnExitReached;
        }

        /// <summary>Reveals the scene with its name.</summary>
        private void Start()
        {
            string title = currentLevel != null ? currentLevel.DisplayName : settings.GameTitle;
            StartCoroutine(fader.FadeIn(title));
        }

        /// <summary>Stops listening.</summary>
        private void OnDisable()
        {
            LevelExit.PlayerReachedExit -= OnExitReached;
        }

        /// <summary>Erases the save and starts the first level.</summary>
        [ContextMenu("New Game")]
        public void NewGame()
        {
            SaveSystem.Delete();
            Load(settings.FirstLevel.SceneName);
        }

        /// <summary>Loads the saved level, or starts a new game when there is no usable save.</summary>
        [ContextMenu("Continue")]
        public void Continue()
        {
            LevelData level = SaveSystem.TryLoad(out SaveData data) ? settings.Find(data.LevelId) : null;

            if (level == null)
            {
                NewGame();
                return;
            }

            Load(level.SceneName);
        }

        /// <summary>Goes back to the main menu. Progress stays at the last save.</summary>
        public void ReturnToMenu()
        {
            Load(settings.MenuSceneName);
        }

        /// <summary>Closes the game, or stops Play mode in the editor.</summary>
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>Saves the next level and loads it, or ends the game after the last one.</summary>
        private void OnExitReached()
        {
            if (isLoading)
            {
                return;
            }

            LevelData next = settings.GetNext(currentLevel);

            if (next == null)
            {
                SaveSystem.Delete();
                ReturnToMenu();
                return;
            }

            SaveSystem.Save(new SaveData
            {
                LevelId = next.LevelId,
                HitPoints = playerHealth.Current + healthPerLevel,
                CarriedOrbs = orbCounter.Count
            });
            Load(next.SceneName);
        }

        /// <summary>Leaves the current scene, once.</summary>
        /// <param name="sceneName">The scene to load.</param>
        private void Load(string sceneName)
        {
            if (isLoading)
            {
                return;
            }

            isLoading = true;
            StartCoroutine(FadeOutAndLoad(sceneName));
        }

        /// <summary>Waits for the screen to turn black, then loads the scene.</summary>
        /// <param name="sceneName">The scene to load.</param>
        private IEnumerator FadeOutAndLoad(string sceneName)
        {
            yield return fader.FadeOut();
            SceneManager.LoadScene(sceneName);
        }
    }
}