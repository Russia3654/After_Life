using UnityEngine;

namespace AfterLife.Systems
{
    /// <summary>Plays a looping list of sprites on a sprite renderer.</summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteFlipbook : MonoBehaviour
    {
        [Header("Animation")]
        [Tooltip("Frames in playing order.")]
        [SerializeField]
        private Sprite[] frames;

        [Tooltip("Frames shown each second.")]
        [SerializeField, Min(1f)]
        private float framesPerSecond = 10f;

        private SpriteRenderer spriteRenderer;

        /// <summary>Finds the renderer to animate.</summary>
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>Shows the frame that matches the current game time.</summary>
        private void Update()
        {
            if (frames.Length == 0)
            {
                return;
            }

            int index = (int)(Time.time * framesPerSecond) % frames.Length;
            spriteRenderer.sprite = frames[index];
        }
    }
}