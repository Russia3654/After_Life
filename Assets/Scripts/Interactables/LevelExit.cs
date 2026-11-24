using System;
using UnityEngine;

namespace AfterLife.Interactables
{
    /// <summary>
    /// The way out of a level. The game manager (Stage 5) listens to <see cref="PlayerReachedExit"/>.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class LevelExit : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("Layers that can use the exit (the Player layer).")]
        [SerializeField]
        private LayerMask playerLayers;

         /// <summary>Raised when the player reaches the exit.</summary>
        public static event Action PlayerReachedExit;

        /// <summary>Makes sure the collider is a trigger so the player walks through it.</summary>
        private void Reset()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        /// <summary>Announces the exit when an object on a player layer enters it.</summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((playerLayers.value & (1 << other.gameObject.layer)) == 0)
            {
                return;
            }

            Debug.Log("Level exit reached"); // Temporary, for testing.
            PlayerReachedExit?.Invoke();
        }
    }
}
