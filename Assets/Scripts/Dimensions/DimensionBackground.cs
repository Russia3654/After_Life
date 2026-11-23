using UnityEngine;

namespace AfterLife.Dimensions
{
    /// <summary>Changes the camera background color to match the active dimension.</summary>
    [RequireComponent(typeof(Camera))]
    public class DimensionBackground : MonoBehaviour
    {
        [Header("Colors")]
        [Tooltip("Background in the Light world (cream).")]
        [SerializeField]
        private Color lightColor = new Color(1f, 0.957f, 0.878f);
        [Tooltip("Background in the Shadow world (lavender).")]
        [SerializeField]
        private Color shadowColor = new Color(0.894f, 0.878f, 0.961f);

        private Camera cam;

        /// <summary>Caches the camera.</summary>
        private void Awake()
        {
            cam = GetComponent<Camera>();
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

        /// <summary>Sets the background color for the new dimension.</summary>
        /// <param name="active">The dimension that is now active.</param>
        private void OnDimensionChanged(Dimension active)
        {
            cam.backgroundColor = active == Dimension.Light ? lightColor : shadowColor;
        }
    }
}
