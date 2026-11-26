using System.Collections.Generic;
using UnityEngine;

namespace AfterLife.Data
{
    /// <summary>Everything the game needs to know about one level.</summary>
    [CreateAssetMenu(fileName = "NewLevelData", menuName = "AfterLife/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Level")]
        [Tooltip("Unique, never-changing ID stored in the save file.")]
        [SerializeField]
        private string levelId;
        [Tooltip("Name shown to the player.")]
        [SerializeField]
        private string displayName;
        [Tooltip("Exact name of the scene file, which must be in the build's scene list.")]
        [SerializeField]
        private string sceneName;

        [Header("Clues")]
        [Tooltip("Lines unlocked by collecting orbs.")]
        [SerializeField]
        private List<OrbClue> clues = new List<OrbClue>();

        /// <summary>The stable ID of this level.</summary>
        public string LevelId => levelId;

        /// <summary>Name shown to the player.</summary>
        public string DisplayName => displayName;

        /// <summary>Exact name of the level's scene.</summary>
        public string SceneName => sceneName;

        /// <summary>Returns the clue lines unlocked by the given number of orbs.</summary>
        /// <param name="orbCount">Orbs the player has collected.</param>
        public IEnumerable<OrbClue> GetUnlockedClues(int orbCount)
        {
            foreach (OrbClue clue in clues)
            {
                if (clue.RequiredOrbs <= orbCount)
                {
                    yield return clue;
                }
            }
        }

        /// <summary>Test helper: logs every clue line from the asset's context menu.</summary>
        [ContextMenu("Log All Clues")]
        private void LogAllClues()
        {
            foreach (OrbClue clue in GetUnlockedClues(int.MaxValue))
            {
                Debug.Log($"{clue.RequiredOrbs} orbs: {clue.Text}");
            }
        }
    }
}