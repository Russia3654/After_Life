using System;
using UnityEngine;

namespace AfterLife.Interactables
{
    /// <summary>
    /// A flag the player touches to save their respawn spot. The respawn system (Stage 4) listens to <see cref="Reached"/>.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Checkpoint : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("Unique, never-changing ID used by the save system.")]
        [SerializeField]
        private string uniqueId;
        [Tooltip("Layers that can activate the checkpoint (the Player layer).")]
        [SerializeField]
        private LayerMask playerLayers;
        [Tooltip("Where the player reappears. Uses this object's position when empty.")]
        [SerializeField]
        private Transform respawnPoint;

        [Header("Visuals")]
        [Tooltip("Sprite renderer tinted once the checkpoint is reached.")]
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [Tooltip("Color shown once the checkpoint is reached.")]
        [SerializeField]
        private Color reachedColor = new Color(0.722f, 0.941f, 0.863f);

        /// <summary>Raised when the player reaches a checkpoint for the first time.</summary>
        public static event Action<Checkpoint> Reached;
        /// <summary>Raised when the player walks into a new checkpoint.</summary>
        public static event Action Touched;

        /// <summary>The stable ID of this checkpoint.</summary>
        public string UniqueId => uniqueId;

        /// <summary>True once the player has touched this checkpoint.</summary>
        public bool IsReached { get; private set; }

        /// <summary>The position where the player respawns.</summary>
        public Vector3 RespawnPosition => respawnPoint != null ? respawnPoint.position : transform.position;

        /// <summary>Makes sure the collider is a trigger so the player walks through it.</summary>
        private void Reset()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        /// <summary>Activates the checkpoint when an object on a player layer enters it.</summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsReached || (playerLayers.value & (1 << other.gameObject.layer)) == 0)
            {
                return;
            }

            Activate();
            Touched?.Invoke();
        }

        /// <summary>Marks the checkpoint as reached, tints it and announces it.</summary>
        public void Activate()
        {
            IsReached = true;
            spriteRenderer.color = reachedColor;
            Reached?.Invoke(this);
        }
    }
}
