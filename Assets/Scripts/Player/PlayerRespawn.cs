using System.Collections;
using UnityEngine;
using AfterLife.Dimensions;
using AfterLife.Interactables;
using AfterLife.Systems;

namespace AfterLife.Player
{
    /// <summary>
    /// Returns the player to the last checkpoint (or the start) after death, in the dimension it was reached in.
    /// </summary>
    [RequireComponent(typeof(Health), typeof(Rigidbody2D), typeof(PlayerController))]
    public class PlayerRespawn : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("The scene's dimension manager.")]
        [SerializeField]
        private DimensionManager dimensionManager;
        [Tooltip("Seconds between death and respawn.")]
        [SerializeField, Min(0f)]
        private float respawnDelay = 0.8f;
        [Tooltip("Crates that return to their start position on respawn.")]
        [SerializeField]
        private PushableBlock[] blocksToReset;

        private Health health;
        private Rigidbody2D body;
        private PlayerController controller;
        private Vector3 respawnPosition;
        private Dimension respawnDimension;

        /// <summary>Caches components and uses the start position as the first respawn spot.</summary>
        private void Awake()
        {
            health = GetComponent<Health>();
            body = GetComponent<Rigidbody2D>();
            controller = GetComponent<PlayerController>();
            respawnPosition = transform.position;
        }

        /// <summary>Listens for death and for reached checkpoints.</summary>
        private void OnEnable()
        {
            health.Died += OnDied;
            Checkpoint.Reached += OnCheckpointReached;
        }

        /// <summary>Stops listening.</summary>
        private void OnDisable()
        {
            health.Died -= OnDied;
            Checkpoint.Reached -= OnCheckpointReached;
        }

        /// <summary>Remembers the starting dimension.</summary>
        private void Start()
        {
            respawnDimension = dimensionManager.Current;
        }

        /// <summary>Stores the checkpoint position and the current dimension.</summary>
        /// <param name="checkpoint">The checkpoint just reached.</param>
        private void OnCheckpointReached(Checkpoint checkpoint)
        {
            respawnPosition = checkpoint.RespawnPosition;
            respawnDimension = dimensionManager.Current;
        }

        /// <summary>Starts the respawn sequence.</summary>
        private void OnDied()
        {
            StartCoroutine(Respawn());
        }

        /// <summary>Freezes the player, waits, then restores position, dimension, crates and health.</summary>
        private IEnumerator Respawn()
        {
            controller.enabled = false;
            body.simulated = false;

            yield return new WaitForSeconds(respawnDelay);

            transform.position = respawnPosition;
            body.linearVelocity = Vector2.zero;
            dimensionManager.SetDimension(respawnDimension);

            foreach (PushableBlock block in blocksToReset)
            {
                block.ResetToStart();
            }

            body.simulated = true;
            controller.enabled = true;
            health.Restore();
        }
    }
}