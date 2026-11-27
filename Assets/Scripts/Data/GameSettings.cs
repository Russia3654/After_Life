using System.Collections.Generic;
using UnityEngine;

namespace AfterLife.Data
{
    /// <summary>Game-wide settings, including the level order.</summary>
    [CreateAssetMenu(fileName = "GameSettings", menuName = "AfterLife/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Levels")]
        [Tooltip("All levels, in play order.")]
        [SerializeField]
        private List<LevelData> levels = new List<LevelData>();

        /// <summary>The level a new game starts in.</summary>
        public LevelData FirstLevel => levels[0];

        /// <summary>Number of orbs in the whole game.</summary>
        public int TotalOrbs
        {
            get
            {
                int total = 0;

                foreach (LevelData level in levels)
                {
                    total += level.OrbCount;
                }

                return total;
            }
        }

        /// <summary>Finds a level by its ID.</summary>
        /// <param name="levelId">The ID stored in the save.</param>
        /// <returns>The level, or null when unknown.</returns>
        public LevelData Find(string levelId)
        {
            return levels.Find(level => level.LevelId == levelId);
        }

        /// <summary>The level after the given one.</summary>
        /// <param name="current">The level just finished.</param>
        /// <returns>The next level, or null after the last.</returns>
        public LevelData GetNext(LevelData current)
        {
            int next = levels.IndexOf(current) + 1;
            return next > 0 && next < levels.Count ? levels[next] : null;
        }
    }
}