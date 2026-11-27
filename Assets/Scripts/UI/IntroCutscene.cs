using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using AfterLife.Systems;

namespace AfterLife.UI
{
    /// <summary>Tells the opening story as fading text lines, then starts a new game.</summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class IntroCutscene : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("Text that shows one line at a time.")]
        [SerializeField]
        private TMP_Text lineText;

        [Tooltip("Starts the game when the story ends.")]
        [SerializeField]
        private GameManager gameManager;

        [Header("Story")]
        [Tooltip("Lines shown in order.")]
        [SerializeField, TextArea]
        private string[] lines =
        {
            "A small light wakes in the dark.",
            "You were a fox. You don't remember dying.",
            "Two worlds lie over each other: one bright, one hushed.",
            "Somewhere between them, there is a way out."
        };

        [Header("Timing")]
        [Tooltip("Seconds one fade takes.")]
        [SerializeField, Min(0.01f)]
        private float fadeDuration = 1f;

        [Tooltip("Seconds a line stays fully visible.")]
        [SerializeField, Min(0f)]
        private float holdDuration = 2.5f;

        private CanvasGroup panel;
        private CanvasGroup lineGroup;
        private InputAction skipAction;

        /// <summary>Hides the panel and creates the skip input.</summary>
        private void Awake()
        {
            panel = GetComponent<CanvasGroup>();
            panel.alpha = 0f;
            panel.blocksRaycasts = false;

            lineGroup = lineText.gameObject.AddComponent<CanvasGroup>();
            lineGroup.alpha = 0f;

            skipAction = new InputAction("Skip", InputActionType.Button);
            skipAction.AddBinding("<Keyboard>/space");
            skipAction.AddBinding("<Keyboard>/escape");
            skipAction.AddBinding("<Gamepad>/start");
            skipAction.performed += OnSkip;
        }

        /// <summary>Stops listening for the skip input.</summary>
        private void OnDisable()
        {
            skipAction.Disable();
        }

        /// <summary>Covers the menu and starts the story.</summary>
        public void Play()
        {
            panel.blocksRaycasts = true;
            EventSystem.current.SetSelectedGameObject(null); // Keys must not reach the buttons underneath.
            skipAction.Enable();
            StartCoroutine(TellStory());
        }

        /// <summary>Ends the story early.</summary>
        /// <param name="context">The input that triggered the skip.</param>
        private void OnSkip(InputAction.CallbackContext context)
        {
            StopAllCoroutines();
            Finish();
        }

        /// <summary>Fades to black, shows each line in turn, then starts the game.</summary>
        private IEnumerator TellStory()
        {
            yield return Fade(panel, 1f);

            foreach (string line in lines)
            {
                lineText.text = line;
                yield return Fade(lineGroup, 1f);
                yield return new WaitForSecondsRealtime(holdDuration);
                yield return Fade(lineGroup, 0f);
            }

            Finish();
        }

        /// <summary>Blends a group to the target opacity.</summary>
        /// <param name="group">What to fade.</param>
        /// <param name="target">Final opacity, 0 to 1.</param>
        private IEnumerator Fade(CanvasGroup group, float target)
        {
            float start = group.alpha;

            for (float time = 0f; time < fadeDuration; time += Time.unscaledDeltaTime)
            {
                group.alpha = Mathf.Lerp(start, target, time / fadeDuration);
                yield return null;
            }

            group.alpha = target;
        }

        /// <summary>Starts the new game.</summary>
        private void Finish()
        {
            skipAction.Disable();
            gameManager.NewGame();
        }
    }
}