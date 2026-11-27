using UnityEngine;
using AfterLife.Data;
using System.Collections;

namespace AfterLife.Interactables
{
    /// <summary>
    /// A floor plate that raises one channel when something steps on it and another when everything leaves.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class PressurePlate : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("Unique, never-changing ID used by the save system.")]
        [SerializeField]
        private string uniqueId;
        [Tooltip("Layers that can press the plate (the Player layer).")]
        [SerializeField]
        private LayerMask presserLayers;

        [Header("Channels")]
        [Tooltip("Raised when the plate becomes pressed.")]
        [SerializeField]
        private GameEventChannel pressedChannel;
        [Tooltip("Raised when the plate is released.")]
        [SerializeField]
        private GameEventChannel releasedChannel;

        [Header("Visuals")]
        [Tooltip("Sprite renderer tinted while the plate is pressed.")]
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [Tooltip("Color shown while the plate is pressed.")]
        [SerializeField]
        private Color pressedColor = new Color(0.961f, 0.843f, 0.431f);
        [Tooltip("Seconds the plate stays active after the last object leaves.")]
        [SerializeField, Min(0f)]
        private float releaseDelay = 0.5f;

        private Color releasedColor;
        private int presserCount;
        private bool isActive;
        private Coroutine releaseRoutine;

        /// <summary>The stable ID of this plate.</summary>
        public string UniqueId => uniqueId;

        /// <summary>True while at least one object stands on the plate.</summary>
        public bool IsPressed => isActive;

        /// <summary>Raised when any plate becomes pressed.</summary>
        public static event System.Action Pressed;

        /// <summary>Makes sure the collider is a trigger and remembers the idle color.</summary>
        private void Reset()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        /// <summary>Remembers the idle color of the plate.</summary>
        private void Awake()
        {
            releasedColor = spriteRenderer.color;
        }

        /// <summary>Counts a new presser and activates the plate if it was idle.</summary>
        /// <param name="other">The collider that entered the plate.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsPresser(other))
            {
                return;
            }

            presserCount++;
            CancelPendingRelease();

            if (!isActive)
            {
                Activate();
            }
        }

        /// <summary>Removes a presser and starts the release timer when the last one leaves.</summary>
        /// <param name="other">The collider that left the plate.</param>
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!IsPresser(other))
            {
                return;
            }

            presserCount--;

            if (presserCount == 0)
            {
                releaseRoutine = StartCoroutine(ReleaseAfterDelay());
            }
        }

        /// <summary>Waits for the release delay, then deactivates the plate.</summary>
        private IEnumerator ReleaseAfterDelay()
        {
            yield return new WaitForSeconds(releaseDelay);
            releaseRoutine = null;
            Deactivate();
        }

        /// <summary>Stops a release that is waiting for its delay to finish.</summary>
        private void CancelPendingRelease()
        {
            if (releaseRoutine == null)
            {
                return;
            }

            StopCoroutine(releaseRoutine);
            releaseRoutine = null;
        }

        /// <summary>Turns the plate on: tints it and raises the pressed channel.</summary>
        private void Activate()
        {
            isActive = true;
            spriteRenderer.color = pressedColor;
            pressedChannel.Raise();
            Pressed?.Invoke();
        }

        /// <summary>Turns the plate off: restores its color and raises the released channel.</summary>
        private void Deactivate()
        {
            isActive = false;
            spriteRenderer.color = releasedColor;
            releasedChannel.Raise();
        }

        /// <summary>True when the collider belongs to a layer that can press the plate.</summary>
        /// <param name="other">The collider to test.</param>
        private bool IsPresser(Collider2D other)
        {
            return (presserLayers.value & (1 << other.gameObject.layer)) != 0;
        }
    }
}
