using System;
using UnityEngine;

namespace AfterLife.Dimensions
{
    /// <summary>
    /// Shows its object (and all children) only while the matching dimension is active.
    /// Put it on the LightWorld and ShadowWorld root objects.
    /// </summary>
    public class DimensionWorld : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("The dimension in which this world is visible and solid.")]
        [SerializeField]
        private Dimension dimension;

        /// <summary>Raised after this world has shown or hidden its objects.</summary>
        public event Action Applied;

        /// <summary>True while this world's dimension is active.</summary>
        public bool IsVisible { get; private set; } = true;

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

        /// <summary>Enables or disables every renderer and collider below this object.</summary>
        /// <param name="active">The dimension that is now active.</param>
        private void OnDimensionChanged(Dimension active)
        {
            bool isActive = active == dimension;
            IsVisible = isActive;

            foreach (Renderer childRenderer in GetComponentsInChildren<Renderer>(true))
            {
                childRenderer.enabled = isActive;
            }

            foreach (Collider2D childCollider in GetComponentsInChildren<Collider2D>(true))
            {
                childCollider.enabled = isActive;
            }

            foreach (Rigidbody2D childBody in GetComponentsInChildren<Rigidbody2D>(true))
            {
                childBody.simulated = isActive;
            }
            Applied?.Invoke();
        }
    }
}