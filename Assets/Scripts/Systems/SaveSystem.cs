using System;
using System.IO;
using UnityEngine;
using AfterLife.Data;

namespace AfterLife.Systems
{
    /// <summary>Reads and writes the single JSON save file.</summary>
    public static class SaveSystem
    {
        private const string FileName = "save.json";
        private const string TempSuffix = ".tmp";

        /// <summary>Full path of the save file.</summary>
        public static string FilePath => Path.Combine(Application.persistentDataPath, FileName);

        /// <summary>True when a save file exists (used to enable Continue).</summary>
        public static bool HasSave => File.Exists(FilePath);

        /// <summary>Writes the data, replacing the old save only after the new one is fully written.</summary>
        /// <param name="data">The state to save.</param>
        public static void Save(SaveData data)
        {
            string tempPath = FilePath + TempSuffix;

            try
            {
                File.WriteAllText(tempPath, JsonUtility.ToJson(data, true));
                File.Copy(tempPath, FilePath, true);
                File.Delete(tempPath);
            }
            catch (Exception exception) when (exception is IOException || exception is UnauthorizedAccessException)
            {
                Debug.LogError($"Could not save: {exception.Message}");
            }
        }

        /// <summary>Reads the save file.</summary>
        /// <param name="data">The loaded state, or null when loading fails.</param>
        /// <returns>True when a valid save was read.</returns>
        public static bool TryLoad(out SaveData data)
        {
            data = null;

            if (!HasSave)
            {
                return false;
            }

            try
            {
                data = JsonUtility.FromJson<SaveData>(File.ReadAllText(FilePath));
            }
            catch (Exception exception) when (exception is IOException || exception is UnauthorizedAccessException || exception is ArgumentException)
            {
                Debug.LogWarning($"Save file unreadable: {exception.Message}");
            }

            return data != null;
        }

        /// <summary>Removes the save file (New Game).</summary>
        public static void Delete()
        {
            File.Delete(FilePath);
        }
    }
}