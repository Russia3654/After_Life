using UnityEngine;
using AfterLife.Data;
using AfterLife.Dimensions;

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
        [Tooltip("Optional channel that closes this door again when raised (for pressure plates).")]
        [SerializeField]
        private GameEventChannel closeChannel;

        private Collider2D doorCollider;
        private SpriteRenderer doorRenderer;
        private DimensionWorld world;
        private DimensionAnimation doorAnimation;

        /// <summary>The stable ID of this door.</summary>
        public string UniqueId => uniqueId;

        /// <summary>True once the door has been opened.</summary>
        public bool IsOpen { get; private set; }

        /// <summary>Caches the components.</summary>
        private void Awake()
        {
            doorCollider = GetComponent<Collider2D>();
            doorRenderer = GetComponent<SpriteRenderer>();
            world = GetComponentInParent<DimensionWorld>();
            doorAnimation = GetComponent<DimensionAnimation>();
        }

        /// <summary>Starts listening to the open and close channels.</summary>
        private void OnEnable()
        {
            if (openChannel != null)
            {
                openChannel.Raised += Open;
            }

            if (closeChannel != null)
            {
                closeChannel.Raised += Close;
            }

            if (world != null)
            {
                world.Applied += Refresh;
            }
        }


        /// <summary>Stops listening to the open and close channels.</summary>
        private void OnDisable()
        {
            if (openChannel != null)
            {
                openChannel.Raised -= Open;
            }

            if (closeChannel != null)
            {
                closeChannel.Raised -= Close;
            }

            if (world != null)
            {
                world.Applied -= Refresh;
            }
        }

        /// <summary>Opens the door: hides it and removes its collision.</summary>
        public void Open()
        {
            IsOpen = true;
            Refresh();
        }

        /// <summary>Closes the door: it blocks the player again in its own dimension.</summary>
        public void Close()
        {
            IsOpen = false;
            Refresh();
        }

        /// <summary>Keeps the door visible, solid only while closed, and animates it.</summary>
        private void Refresh()
        {
            bool isVisible = world == null || world.IsVisible;
            doorCollider.enabled = isVisible && !IsOpen;
            doorRenderer.enabled = isVisible;
            doorAnimation.Show(IsOpen);
        }
    }
}