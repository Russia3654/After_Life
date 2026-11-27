using UnityEngine;
using UnityEngine.SceneManagement;
using AfterLife.Data;
using AfterLife.Interactables;
using System.Collections;
using AfterLife.UI;

namespace AfterLife.Systems
{
    /// <summary>Starts, continues and advances the game by loading level scenes.</summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("The level order.")]
        [SerializeField]
        private GameSettings settings;

        [Tooltip("The level in this scene.")]
        [SerializeField]
        private LevelData currentLevel;

        [Tooltip("Black overlay used between levels.")]
        [SerializeField]
        private ScreenFader fader;

        [Tooltip("The player's health, carried to the next level.")]
        [SerializeField]
        private Health playerHealth;

        [Tooltip("Hit points gained when entering a new level.")]
        [SerializeField, Min(0)]
        private int healthPerLevel = 1;

        [Tooltip("The scene's orb counter, carried to the next level.")]
        [SerializeField]
        private OrbCounter orbCounter;

        private bool isLoading;

        /// <summary>Listens for the level exit.</summary>
        private void OnEnable()
        {
            LevelExit.PlayerReachedExit += OnExitReached;
        }

        /// <summary>Reveals the level with its name.</summary>
        private void Start()
        {
            StartCoroutine(fader.FadeIn(currentLevel.DisplayName));
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
            Load(settings.FirstLevel);
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

            Load(level);
        }

        /// <summary>Saves the next level as the one to continue from, then loads it.</summary>
        private void OnExitReached()
        {
            if (isLoading)
            {
                return;
            }

            LevelData next = settings.GetNext(currentLevel);

            if (next == null)
            {
                NewGame(); // ponytail: restarts after the last level; goes to the main menu in step 6.4.
                return;
            }

            SaveSystem.Save(new SaveData
            {
                LevelId = next.LevelId,
                HitPoints = playerHealth.Current + healthPerLevel,
                CarriedOrbs = orbCounter.Count
            });
            Load(next);
        }

        /// <summary>Leaves the current level for another one.</summary>
        /// <param name="level">The level to load.</param>
        private void Load(LevelData level)
        {
            isLoading = true;
            StartCoroutine(FadeOutAndLoad(level.SceneName));
        }

        /// <summary>Waits for the screen to turn black, then loads the scene.</summary>
        /// <param name="sceneName">Scene to load.</param>
        private IEnumerator FadeOutAndLoad(string sceneName)
        {
            yield return fader.FadeOut();
            SceneManager.LoadScene(sceneName);
        }
    }
}