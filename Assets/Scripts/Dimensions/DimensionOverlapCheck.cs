using UnityEngine;

namespace AfterLife.Dimensions
{
    /// <summary>
    /// Detects when a dimension switch leaves the player inside a solid of the new world.
    /// Damage is a placeholder until the health system exists (Stage 4).
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class DimensionOverlapCheck : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("Layers that count as solid or harmful when the player overlaps them after a switch.")]
        [SerializeField]
        private LayerMask solidLayers;

        private Collider2D playerCollider;
        private readonly Collider2D[] overlapResults = new Collider2D[8];

        /// <summary>Caches the player's collider.</summary>
        private void Awake()
        {
            playerCollider = GetComponent<Collider2D>();
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

        /// <summary>Checks for overlap right after the world has switched.</summary>
        /// <param name="active">The dimension that is now active.</param>
        private void OnDimensionChanged(Dimension active)
        {
            if (!IsOverlappingSolid())
            {
                return;
            }

            ApplyOverlapDamage();
        }

        /// <summary>True when the player's collider overlaps an enabled collider on a solid layer.</summary>
        private bool IsOverlappingSolid()
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(solidLayers);
            filter.useTriggers = false;

            return playerCollider.Overlap(filter, overlapResults) > 0;
        }

        /// <summary>Placeholder damage. Replaced by the health system in Stage 4.</summary>
        private void ApplyOverlapDamage()
        {
            Debug.Log("Overlap damage: switched into a solid.");
        }
    }
}
