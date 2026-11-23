using UnityEngine;
using AfterLife.Data;

namespace AfterLife.Interactables
{
    /// <summary>
    /// A door that opens when its event channel is raised. It hides and stops blocking the player.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Door : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("Unique, never-changing ID used by the save system.")]
        [SerializeField]
        private string uniqueId;
        [Tooltip("The channel that opens this door when raised.")]
        [SerializeField]
        private GameEventChannel openChannel;

        private Collider2D doorCollider;
        private SpriteRenderer doorRenderer;

        /// <summary>The stable ID of this door.</summary>
        public string UniqueId => uniqueId;

        /// <summary>True once the door has been opened.</summary>
        public bool IsOpen { get; private set; }

        /// <summary>Caches the components.</summary>
        private void Awake()
        {
            doorCollider = GetComponent<Collider2D>();
            doorRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>Starts listening to the open channel.</summary>
        private void OnEnable()
        {
            if (openChannel != null)
            {
                openChannel.Raised += Open;
            }
        }

        /// <summary>Stops listening to the open channel.</summary>
        private void OnDisable()
        {
            if (openChannel != null)
            {
                openChannel.Raised -= Open;
            }
        }

        /// <summary>Opens the door: hides it and removes its collision.</summary>
        public void Open()
        {
            IsOpen = true;
            doorCollider.enabled = false;
            doorRenderer.enabled = false;
        }
    }
}