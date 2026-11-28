using UnityEngine;

namespace AfterLife.UI
{
    /// <summary>Sways a UI element back and forth around the place where it was put.</summary>
    [RequireComponent(typeof(RectTransform))]
    public class UIDrift : MonoBehaviour
    {
        [Header("Motion")]
        [Tooltip("Farthest distance from the start, in canvas units.")]
        [SerializeField]
        private Vector2 distance = new Vector2(0f, 14f);

        [Tooltip("Seconds for one full back-and-forth.")]
        [SerializeField, Min(0.1f)]
        private float period = 3f;

        [Tooltip("Where in the cycle to start, from 0 to 1.")]
        [SerializeField, Range(0f, 1f)]
        private float startPoint;

        private RectTransform rectTransform;
        private Vector2 home;

        /// <summary>Remembers the starting position.</summary>
        private void Awake()
        {
            rectTransform = (RectTransform)transform;
            home = rectTransform.anchoredPosition;
        }

        /// <summary>Moves along a smooth wave.</summary>
        private void Update()
        {
            float cycle = Time.unscaledTime / period + startPoint;
            float wave = Mathf.Sin(cycle * Mathf.PI * 2f);
            rectTransform.anchoredPosition = home + distance * wave;
        }
    }
}