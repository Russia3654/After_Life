using UnityEngine;
using AfterLife.Data;
using AfterLife.Dimensions;
using AfterLife.Interactables;

namespace AfterLife.Systems
{
    /// <summary>Saves a snapshot of the level each time the player reaches a checkpoint.</summary>
    public class LevelSaver : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("The level being played.")]
        [SerializeField]
        private LevelData level;
        [Tooltip("The scene's dimension manager.")]
        [SerializeField]
        private DimensionManager dimensionManager;
        [Tooltip("The scene's key inventory.")]
        [SerializeField]
        private KeyInventory inventory;

        /// <summary>Listens for reached checkpoints.</summary>
        private void OnEnable()
        {
            Checkpoint.Reached += OnCheckpointReached;
        }

        /// <summary>Stops listening.</summary>
        private void OnDisable()
        {
            Checkpoint.Reached -= OnCheckpointReached;
        }

        /// <summary>Writes the save file.</summary>
        /// <param name="checkpoint">The checkpoint just reached.</param>
        private void OnCheckpointReached(Checkpoint checkpoint)
        {
            SaveSystem.Save(Capture(checkpoint));
        }

        /// <summary>Collects the current state of the level.</summary>
        /// <param name="checkpoint">The checkpoint to respawn at.</param>
        private SaveData Capture(Checkpoint checkpoint)
        {
            SaveData data = new SaveData
            {
                LevelId = level.LevelId,
                CheckpointId = checkpoint.UniqueId,
                Dimension = dimensionManager.Current
            };

            data.HeldKeyIds.AddRange(inventory.HeldKeyIds);
            CaptureCollected(data);
            CaptureActivated(data);
            CaptureBlocks(data);
            return data;
        }

        /// <summary>Records collected items, which are the deactivated ones.</summary>
        /// <param name="data">The save being built.</param>
        private void CaptureCollected(SaveData data)
        {
            foreach (Collectible item in FindAll<Collectible>())
            {
                if (!item.gameObject.activeSelf)
                {
                    data.CollectedIds.Add(item.UniqueId);
                }
            }
        }

        /// <summary>Records flipped switches, opened locks and reached checkpoints.</summary>
        /// <param name="data">The save being built.</param>
        private void CaptureActivated(SaveData data)
        {
            foreach (Switch lever in FindAll<Switch>())
            {
                if (lever.IsOn)
                {
                    data.ActivatedIds.Add(lever.UniqueId);
                }
            }

            foreach (KeyLock keyLock in FindAll<KeyLock>())
            {
                if (keyLock.IsUnlocked)
                {
                    data.ActivatedIds.Add(keyLock.UniqueId);
                }
            }

            foreach (Checkpoint flag in FindAll<Checkpoint>())
            {
                if (flag.IsReached)
                {
                    data.ActivatedIds.Add(flag.UniqueId);
                }
            }
        }

        /// <summary>Records where every crate is.</summary>
        /// <param name="data">The save being built.</param>
        private void CaptureBlocks(SaveData data)
        {
            foreach (PushableBlock block in FindAll<PushableBlock>())
            {
                data.Blocks.Add(new BlockState(block.UniqueId, block.transform.position));
            }
        }

        /// <summary>Finds every component of a type in the scene, including inactive objects.</summary>
        private static T[] FindAll<T>() where T : Component
        {
            return FindObjectsByType<T>(FindObjectsInactive.Include);
        }
    }
}