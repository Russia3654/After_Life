using System;
using UnityEngine;
using AfterLife.Interactables;

namespace AfterLife.Systems
{
    /// <summary>Counts the spirit orbs collected in this level.</summary>
    public class OrbCounter : MonoBehaviour
    {
        private Collectible[] items;

        /// <summary>Raised when the count changes.</summary>
        public event Action Changed;

        /// <summary>Orbs collected so far.</summary>
        public int Count { get; private set; }

        /// <summary>Orbs placed in the level.</summary>
        public int Total { get; private set; }

        /// <summary>Finds every pickup, including ones already hidden.</summary>
        private void Awake()
        {
            items = FindObjectsByType<Collectible>(FindObjectsInactive.Include);
        }

        /// <summary>Listens for pickups.</summary>
        private void OnEnable()
        {
            Collectible.Collected += OnCollected;
        }

        /// <summary>Takes the first count.</summary>
        private void Start()
        {
            Recount();
        }

        /// <summary>Stops listening.</summary>
        private void OnDisable()
        {
            Collectible.Collected -= OnCollected;
        }

        /// <summary>Counts from the scene. Call this after a save hides collected orbs.</summary>
        public void Recount()
        {
            Count = 0;
            Total = 0;

            foreach (Collectible item in items)
            {
                if (item.Type != CollectibleType.Orb)
                {
                    continue;
                }

                Total++;

                if (!item.gameObject.activeSelf)
                {
                    Count++;
                }
            }

            Changed?.Invoke();
        }

        /// <summary>Adds one when an orb is picked up.</summary>
        /// <param name="item">The pickup that was collected.</param>
        private void OnCollected(Collectible item)
        {
            if (item.Type != CollectibleType.Orb)
            {
                return;
            }

            Count++;
            Changed?.Invoke();
        }
    }
}