using UnityEngine;

namespace AfterLife.Dimensions
{
    /// <summary>Shows a different picture in each world, for objects that exist in both.</summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class DimensionSprite : MonoBehaviour
    {
        [Header("Art")]
        [Tooltip("Picture used in the Light world.")]
        [SerializeField]
        private Sprite lightSprite;

        [Tooltip("Picture used in the Shadow world.")]
        [SerializeField]
        private Sprite shadowSprite;

        private SpriteRenderer spriteRenderer;

        /// <summary>Finds the renderer to change.</summary>
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>Starts listening for world switches.</summary>
        private void OnEnable()
        {
            DimensionManager.DimensionChanged += OnDimensionChanged;
        }

        /// <summary>Stops listening.</summary>
        private void OnDisable()
        {
            DimensionManager.DimensionChanged -= OnDimensionChanged;
        }

        /// <summary>Shows the picture that belongs to the active world.</summary>
        /// <param name="active">The world now in use.</param>
        private void OnDimensionChanged(Dimension active)
        {
            spriteRenderer.sprite = active == Dimension.Light ? lightSprite : shadowSprite;
        }
    }
}