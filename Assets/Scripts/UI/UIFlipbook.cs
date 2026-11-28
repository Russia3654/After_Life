using UnityEngine;
using UnityEngine.UI;

namespace AfterLife.UI
{
    /// <summary>Plays a looping list of sprites on a UI image.</summary>
    [RequireComponent(typeof(Image))]
    public class UIFlipbook : MonoBehaviour
    {
        [Header("Animation")]
        [Tooltip("Frames in playing order.")]
        [SerializeField]
        private Sprite[] frames;

        [Tooltip("Frames shown each second.")]
        [SerializeField, Min(1f)]
        private float framesPerSecond = 10f;

        private Image image;

        /// <summary>Finds the image to animate.</summary>
        private void Awake()
        {
            image = GetComponent<Image>();
        }

        /// <summary>Shows the frame that matches the current time.</summary>
        private void Update()
        {
            if (frames.Length == 0)
            {
                return;
            }

            int index = (int)(Time.unscaledTime * framesPerSecond) % frames.Length;
            image.sprite = frames[index];
        }
    }
}