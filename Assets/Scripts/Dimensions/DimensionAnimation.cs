using UnityEngine;

namespace AfterLife.Dimensions
{
    /// <summary>Animates between an off and an on picture, with separate frames for each world.</summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class DimensionAnimation : MonoBehaviour
    {
        [Header("Frames, from off to on")]
        [Tooltip("Light world frames.")]
        [SerializeField]
        private Sprite[] lightFrames;

        [Tooltip("Shadow world frames. Same count as Light.")]
        [SerializeField]
        private Sprite[] shadowFrames;

        [Header("Timing")]
        [Tooltip("Frames shown each second.")]
        [SerializeField, Min(1f)]
        private float framesPerSecond = 12f;

        private SpriteRenderer spriteRenderer;
        private Dimension activeDimension;
        private float position;
        private float target;

        /// <summary>Finds the renderer to animate.</summary>
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

        /// <summary>Moves one step closer to the wanted frame.</summary>
        private void Update()
        {
            if (Mathf.Approximately(position, target))
            {
                return;
            }

            position = Mathf.MoveTowards(position, target, framesPerSecond * Time.deltaTime);
            Draw();
        }

        /// <summary>Animates toward the on or off picture.</summary>
        /// <param name="isOn">True for the last frame, false for the first.</param>
        public void Show(bool isOn)
        {
            target = isOn ? lightFrames.Length - 1 : 0f;
        }

        /// <summary>Shows the same frame in the new world's art.</summary>
        /// <param name="active">The world now in use.</param>
        private void OnDimensionChanged(Dimension active)
        {
            activeDimension = active;
            Draw();
        }

        /// <summary>Puts the current frame on screen.</summary>
        private void Draw()
        {
            Sprite[] frames = activeDimension == Dimension.Light ? lightFrames : shadowFrames;
            spriteRenderer.sprite = frames[Mathf.RoundToInt(position)];
        }
    }
}