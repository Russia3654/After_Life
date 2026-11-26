using System.Collections.Generic;
using UnityEngine;
using AfterLife.Interactables;

namespace AfterLife.Systems
{
    /// <summary>
    /// Remembers which keys the player holds by listening to collected keys.
    /// Put one in the scene. Save/load (Stage 5) will read and restore its IDs.
    /// </summary>
    public class KeyInventory : MonoBehaviour
    {
        private readonly HashSet<string> keyIds = new HashSet<string>();

        /// <summary>IDs of the keys currently held.</summary>
        public IEnumerable<string> HeldKeyIds => keyIds;

        /// <summary>Starts listening for collected items.</summary>
        private void OnEnable()
        {
            Collectible.Collected += OnCollected;
        }

        /// <summary>Stops listening for collected items.</summary>
        private void OnDisable()
        {
            Collectible.Collected -= OnCollected;
        }

        /// <summary>Stores the ID of every collected key.</summary>
        /// <param name="item">The item that was just collected.</param>
        private void OnCollected(Collectible item)
        {
            if (item.Type != CollectibleType.Key)
            {
                return;
            }

            keyIds.Add(item.UniqueId);
        }

        /// <summary>Uses up a key if the player holds it.</summary>
        /// <param name="keyId">The ID of the key needed.</param>
        /// <returns>True when the player had the key and it was used.</returns>
        public bool TryConsume(string keyId)
        {
            return keyIds.Remove(keyId);
        }
    }
}
