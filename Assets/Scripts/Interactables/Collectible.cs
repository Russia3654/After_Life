using System;
using UnityEngine;

namespace AfterLife.Interactables
{
    /// <summary>
    /// An item collected by touching it. It announces itself through <see cref="Collected"/> and disappears.
    /// To make it exist in one world only, add a DimensionWorld component to the same object.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Collectible : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("Unique, never-changing ID used by the save system.")]
        [SerializeField]
        private string uniqueId;
        [Tooltip("What kind of item this is.")]
        [SerializeField]
        private CollectibleType type = CollectibleType.Orb;
        [Tooltip("Layers that can pick this item up (the Player layer).")]
        [SerializeField]
        private LayerMask collectorLayers;

        /// <summary>Raised when any collectible is picked up.</summary>
        public static event Action<Collectible> Collected;

        /// <summary>The stable ID of this item.</summary>
        public string UniqueId => uniqueId;

        /// <summary>The kind of item this is.</summary>
        public CollectibleType Type => type;

        /// <summary>Makes sure the collider is a trigger so the player walks through it.</summary>
        private void Reset()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        /// <summary>Collects the item when an object on a collector layer enters it.</summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((collectorLayers.value & (1 << other.gameObject.layer)) == 0)
            {
                return;
            }

            Collect();
        }

        /// <summary>Announces the pickup and hides the item.</summary>
        public void Collect()
        {
            Collected?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}