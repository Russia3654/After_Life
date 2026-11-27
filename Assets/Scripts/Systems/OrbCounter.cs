using System;
using UnityEngine;
using AfterLife.Data;
using AfterLife.Interactables;

namespace AfterLife.Systems
{
    /// <summary>Counts the spirit orbs collected across the whole game.</summary>
    public class OrbCounter : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("The level order, used for the game's orb total.")]
        [SerializeField]
        private GameSettings settings;

        private Collectible[] items;
        private int levelCount;

        /// <summary>Raised when the count changes.</summary>
        public event Action Changed;

        /// <summary>Raised on a pickup, with the new total. Loading a save does not raise it.</summary>
        public event Action<int> Gained;

        /// <summary>Orbs collected in earlier levels.</summary>
        // ponytail: a number is enough while levels are never revisited; store IDs if they can be.
        public int Carried { get; private set; }

        /// <summary>Orbs collected in the whole game.</summary>
        public int Count => Carried + levelCount;

        /// <summary>Orbs in the whole game.</summary>
        public int Total => settings.TotalOrbs;

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

        /// <summary>Applies a save, after it has hidden this level's collected orbs.</summary>
        /// <param name="carried">Orbs collected in earlier levels.</param>
        public void Restore(int carried)
        {
            Carried = carried;
            Recount();
        }

        /// <summary>Counts this level's hidden orbs.</summary>
        private void Recount()
        {
            levelCount = 0;

            foreach (Collectible item in items)
            {
                if (item.Type == CollectibleType.Orb && !item.gameObject.activeSelf)
                {
                    levelCount++;
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

            levelCount++;
            Gained?.Invoke(Count);
            Changed?.Invoke();
        }
    }
}