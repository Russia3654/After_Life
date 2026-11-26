using UnityEngine;
using AfterLife.Data;
using AfterLife.Dimensions;
using AfterLife.Interactables;
using AfterLife.Player;

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

        [Tooltip("The player's respawn component.")]
        [SerializeField]
        private PlayerRespawn playerRespawn;

        private bool isRestoring;

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

        /// <summary>Restores the save if it belongs to this level.</summary>
        private void Start()
        {
            if (SaveSystem.TryLoad(out SaveData data) && data.LevelId == level.LevelId)
            {
                Restore(data);
            }
        }

        /// <summary>Writes the save file, except while a save is being restored.</summary>
        /// <param name="checkpoint">The checkpoint just reached.</param>
        private void OnCheckpointReached(Checkpoint checkpoint)
        {
            if (isRestoring)
            {
                return;
            }

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

        /// <summary>Puts the level back into its saved state.</summary>
        /// <param name="data">The loaded save.</param>
        private void Restore(SaveData data)
        {
            isRestoring = true;

            RestoreCollected(data);
            RestoreActivated(data);
            RestoreBlocks(data);
            inventory.Restore(data.HeldKeyIds);
            RestorePlayer(data);

            isRestoring = false;
        }

        /// <summary>Hides the items already collected.</summary>
        /// <param name="data">The loaded save.</param>
        private void RestoreCollected(SaveData data)
        {
            foreach (Collectible item in FindAll<Collectible>())
            {
                if (data.CollectedIds.Contains(item.UniqueId))
                {
                    item.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>Re-applies switches, locks and checkpoints.</summary>
        /// <param name="data">The loaded save.</param>
        private void RestoreActivated(SaveData data)
        {
            foreach (Switch lever in FindAll<Switch>())
            {
                if (data.ActivatedIds.Contains(lever.UniqueId))
                {
                    lever.TurnOn();
                }
            }

            foreach (KeyLock keyLock in FindAll<KeyLock>())
            {
                if (data.ActivatedIds.Contains(keyLock.UniqueId))
                {
                    keyLock.Unlock();
                }
            }

            foreach (Checkpoint flag in FindAll<Checkpoint>())
            {
                if (data.ActivatedIds.Contains(flag.UniqueId))
                {
                    flag.Activate();
                }
            }
        }

        /// <summary>Moves crates to their saved positions.</summary>
        /// <param name="data">The loaded save.</param>
        private void RestoreBlocks(SaveData data)
        {
            foreach (PushableBlock block in FindAll<PushableBlock>())
            {
                BlockState state = data.Blocks.Find(saved => saved.Id == block.UniqueId);

                if (state != null)
                {
                    block.MoveTo(state.Position);
                }
            }
        }

        /// <summary>Puts the player at the saved checkpoint.</summary>
        /// <param name="data">The loaded save.</param>
        private void RestorePlayer(SaveData data)
        {
            foreach (Checkpoint flag in FindAll<Checkpoint>())
            {
                if (flag.UniqueId == data.CheckpointId)
                {
                    playerRespawn.RestoreTo(flag.RespawnPosition, data.Dimension);
                    return;
                }
            }
        }

        /// <summary>Finds every component of a type in the scene, including inactive objects.</summary>
        private static T[] FindAll<T>() where T : Component
        {
            return FindObjectsByType<T>(FindObjectsInactive.Include);
        }
    }
}