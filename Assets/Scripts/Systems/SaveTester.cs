using UnityEngine;
using AfterLife.Data;

namespace AfterLife.Systems
{
    /// <summary>Temporary: tests the save file from the context menu. Deleted in step 5.3.</summary>
    public class SaveTester : MonoBehaviour
    {
        /// <summary>Writes a sample save.</summary>
        [ContextMenu("Test Save")]
        private void TestSave()
        {
            SaveData data = new SaveData { LevelId = "level_test" };
            data.CollectedIds.Add("orb_001");
            SaveSystem.Save(data);
            Debug.Log($"Saved to {SaveSystem.FilePath}");
        }

        /// <summary>Reads the save back and logs it.</summary>
        [ContextMenu("Test Load")]
        private void TestLoad()
        {
            bool loaded = SaveSystem.TryLoad(out SaveData data);
            Debug.Log(loaded ? $"Loaded {data.LevelId}, items: {data.CollectedIds.Count}" : "No save found");
        }
    }
}