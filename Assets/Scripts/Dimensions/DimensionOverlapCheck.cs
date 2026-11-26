using UnityEngine;
using AfterLife.Systems;

namespace AfterLife.Dimensions
{
    /// <summary>
    /// Detects when a dimension switch leaves the player inside a solid of the new world.
    /// Damage is a placeholder until the health system exists (Stage 4).
    /// </summary>
    [RequireComponent(typeof(Collider2D), typeof(Health))]
    public class DimensionOverlapCheck : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("Layers that count as solid or harmful when the player overlaps them after a switch.")]
        [SerializeField]
        private LayerMask solidLayers;

        [Tooltip("Hit points lost when switching into a solid.")]
        [SerializeField, Min(1)]
        private int overlapDamage = 1;

        [Tooltip("How much smaller than the player the check box is, so merely touching a surface is ignored.")]
        [SerializeField, Min(0f)]
        private float contactTolerance = 0.15f;

        private Collider2D playerCollider;
        private Health health;
        private readonly Collider2D[] overlapResults = new Collider2D[8];

        private bool checkPending;

        /// <summary>Caches the player's collider.</summary>
        private void Awake()
        {
            playerCollider = GetComponent<Collider2D>();
            health = GetComponent<Health>();
        }

        /// <summary>Starts listening for dimension changes.</summary>
        private void OnEnable()
        {
            DimensionManager.DimensionChanged += OnDimensionChanged;
        }

        /// <summary>Stops listening for dimension changes.</summary>
        private void OnDisable()
        {
            DimensionManager.DimensionChanged -= OnDimensionChanged;
        }

        /// <summary>Requests an overlap check for the next physics step, after the worlds have toggled.</summary>
        /// <param name="active">The dimension that is now active.</param>
        private void OnDimensionChanged(Dimension active)
        {
            checkPending = true;
        }

        /// <summary>Runs the pending overlap check once the physics world is up to date.</summary>
        private void FixedUpdate()
        {
            if (!checkPending)
            {
                return;
            }

            checkPending = false;

            if (IsOverlappingSolid())
            {
                ApplyOverlapDamage();
            }
        }

        /// <summary>True when a solid reaches inside the player's body, not merely touching its edge.</summary>
        private bool IsOverlappingSolid()
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(solidLayers);
            filter.useTriggers = false;

            Bounds bounds = playerCollider.bounds;
            Vector2 checkSize = (Vector2)bounds.size - Vector2.one * contactTolerance;

            return Physics2D.OverlapBox(bounds.center, checkSize, 0f, filter, overlapResults) > 0;
        }

        /// <summary>Costs health for switching into a solid.</summary>
        private void ApplyOverlapDamage()
        {
            health.TakeDamage(overlapDamage);
        }
    }
}
