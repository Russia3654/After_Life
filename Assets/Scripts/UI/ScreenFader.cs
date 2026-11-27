using System.Collections;
using TMPro;
using UnityEngine;

namespace AfterLife.UI
{
    /// <summary>A black overlay that fades in with the level name and fades out before leaving.</summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class ScreenFader : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("Text that shows the level name on the black screen.")]
        [SerializeField]
        private TMP_Text title;

        [Header("Timing")]
        [Tooltip("Seconds the level name stays before the fade starts.")]
        [SerializeField, Min(0f)]
        private float titleDuration = 1.2f;

        [Tooltip("Seconds one fade takes.")]
        [SerializeField, Min(0.01f)]
        private float fadeDuration = 0.6f;

        private CanvasGroup canvasGroup;

        /// <summary>Starts fully black.</summary>
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;
        }

                /// <summary>Shows a title on a frozen black screen, then reveals the scene.</summary>
        /// <param name="levelName">Name to show. Empty skips the pause.</param>
        public IEnumerator FadeIn(string levelName)
        {
            title.text = levelName;

            if (!string.IsNullOrEmpty(levelName))
            {
                Time.timeScale = 0f;
                yield return new WaitForSecondsRealtime(titleDuration);
            }

            Time.timeScale = 1f;
            yield return Fade(0f);
        }

        /// <summary>Fades to a plain black screen.</summary>
        public IEnumerator FadeOut()
        {
            title.text = string.Empty;
            yield return Fade(1f);
        }

        /// <summary>Blends the overlay to the target opacity, even while the game is frozen.</summary>
        /// <param name="target">Final opacity, 0 to 1.</param>
        private IEnumerator Fade(float target)
        {
            float start = canvasGroup.alpha;

            for (float time = 0f; time < fadeDuration; time += Time.unscaledDeltaTime)
            {
                canvasGroup.alpha = Mathf.Lerp(start, target, time / fadeDuration);
                yield return null;
            }

            canvasGroup.alpha = target;
        }
    }
}