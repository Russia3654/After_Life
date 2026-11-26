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

        /// <summary>True when the player's collider overlaps an enabled collider on a solid layer.</summary>
        private bool IsOverlappingSolid()
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(solidLayers);
            filter.useTriggers = false;

            return playerCollider.Overlap(filter, overlapResults) > 0;
        }

        /// <summary>Costs health for switching into a solid.</summary>
        private void ApplyOverlapDamage()
        {
            health.TakeDamage(overlapDamage);
        }
    }
}
