using UnityEngine;
using AfterLife.Data;
using AfterLife.Systems;
using AfterLife.Dimensions;

namespace AfterLife.Interactables
{
    /// <summary>
    /// A lock the player opens with the interact key while holding the matching key.
    /// It raises an event channel, so a Door listening to that channel opens.
    /// </summary>
    public class KeyLock : Interactable
    {
        [Header("Setup")]
        [Tooltip("Unique, never-changing ID used by the save system.")]
        [SerializeField]
        private string uniqueId;
        [Tooltip("The ID of the key that opens this lock.")]
        [SerializeField]
        private string requiredKeyId;
        [Tooltip("The scene's key inventory.")]
        [SerializeField]
        private KeyInventory inventory;
        [Tooltip("The channel raised when the lock is opened.")]
        [SerializeField]
        private GameEventChannel channel;
        [Tooltip("Plays the activation animation.")]
        [SerializeField]
        private DimensionAnimation visual;

        /// <summary>The stable ID of this lock.</summary>
        public string UniqueId => uniqueId;

        /// <summary>True once the lock has been opened.</summary>
        public bool IsUnlocked { get; private set; }

        /// <summary>Opens the lock if the player holds the right key.</summary>
        public override void Interact()
        {
            if (IsUnlocked)
            {
                return;
            }

            if (!inventory.TryConsume(requiredKeyId))
            {
                Debug.Log($"Locked: needs key {requiredKeyId}"); // Temporary. Becomes a HUD message in Stage 6.
                return;
            }

            Unlock();
            ReportUse();
        }

        /// <summary>Opens the lock, tints it and raises the channel.</summary>
        public void Unlock()
        {
            IsUnlocked = true;
            visual.Show(true);
            channel.Raise();
        }
    }
}
