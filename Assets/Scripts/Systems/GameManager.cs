using UnityEngine;
using UnityEngine.SceneManagement;
using AfterLife.Data;
using AfterLife.Interactables;

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

        private bool isLoading;

        /// <summary>Listens for the level exit.</summary>
        private void OnEnable()
        {
            LevelExit.PlayerReachedExit += OnExitReached;
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

            SaveSystem.Save(new SaveData { LevelId = next.LevelId });
            Load(next);
        }

        /// <summary>Loads a level's scene.</summary>
        /// <param name="level">The level to load.</param>
        private void Load(LevelData level)
        {
            isLoading = true;
            SceneManager.LoadScene(level.SceneName);
        }
    }
}