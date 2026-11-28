using UnityEngine;

namespace AfterLife.Systems
{
    /// <summary>Scrolls a repeating background more slowly than the level to suggest distance.</summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [DefaultExecutionOrder(AfterCameraOrder)]
    public class ParallaxLayer : MonoBehaviour
    {
        private const int AfterCameraOrder = 1000;
        private const float Half = 0.5f;

        [Header("Setup")]
        [Tooltip("The main camera.")]
        [SerializeField]
        private Transform cameraTransform;

        [Header("Depth")]
        [Tooltip("Sideways: 0 stays in the level, 1 stays on the screen.")]
        [SerializeField, Range(0f, 1f)]
        private float horizontalFollow = 0.8f;

        [Tooltip("Up and down: 0 stays in the level, 1 stays on the screen.")]
        [SerializeField, Range(0f, 1f)]
        private float verticalFollow = 0.9f;

        private float tileWidth;
        private float homeY;

        /// <summary>Remembers the width of one picture and the starting height.</summary>
        private void Awake()
        {
            tileWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            homeY = transform.position.y;
        }

        /// <summary>Places the layer after the camera has moved.</summary>
        private void LateUpdate()
        {
            Vector3 view = cameraTransform.position;
            float scrolled = Mathf.Repeat(view.x * (1f - horizontalFollow), tileWidth);
            float x = view.x + tileWidth * Half - scrolled;
            float y = Mathf.Lerp(homeY, view.y, verticalFollow);
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}