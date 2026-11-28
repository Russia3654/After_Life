using System.Collections;
using TMPro;
using UnityEngine;
using AfterLife.Data;
using AfterLife.Systems;

namespace AfterLife.UI
{
    /// <summary>Shows a clue line when the orb total unlocks one.</summary>
    public class ClueDisplay : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("Holds the levels and their clues.")]
        [SerializeField]
        private GameSettings settings;

        [Tooltip("The scene's orb counter.")]
        [SerializeField]
        private OrbCounter orbs;

        [Tooltip("Text that shows the line.")]
        [SerializeField]
        private TMP_Text label;

        [Tooltip("Fades the fog and the text together.")]
        [SerializeField]
        private CanvasGroup group;

        [Header("Timing")]
        [Tooltip("Seconds the line stays fully visible.")]
        [SerializeField, Min(0f)]
        private float showDuration = 4f;

        [Tooltip("Seconds the line takes to fade out.")]
        [SerializeField, Min(0.01f)]
        private float fadeDuration = 1f;

        /// <summary>Listens for orb pickups.</summary>
        private void OnEnable()
        {
            orbs.Gained += OnOrbGained;
        }

        /// <summary>Starts with no text.</summary>
        private void Start()
        {
            group.alpha = 0f;
        }

        /// <summary>Stops listening.</summary>
        private void OnDisable()
        {
            orbs.Gained -= OnOrbGained;
        }

        /// <summary>Shows the clue for this total, if there is one.</summary>
        /// <param name="total">The new orb total.</param>
        private void OnOrbGained(int total)
        {
            if (!settings.TryGetClue(total, out string line))
            {
                return;
            }

            StopAllCoroutines();
            StartCoroutine(Show(line));
        }

        /// <summary>Displays the line, waits, then fades it out.</summary>
        /// <param name="line">The text to show.</param>
        private IEnumerator Show(string line)
        {
            label.text = line;
            group.alpha = 1f;
            yield return new WaitForSeconds(showDuration);

            for (float time = 0f; time < fadeDuration; time += Time.deltaTime)
            {
                group.alpha = 1f - time / fadeDuration;
                yield return null;
            }

            group.alpha = 0f;
        }
    }
}