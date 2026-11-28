using TMPro;
using UnityEngine;
using AfterLife.Interactables;

namespace AfterLife.UI
{
    /// <summary>Shows a tutorial line when the player reaches this checkpoint.</summary>
    [RequireComponent(typeof(Checkpoint))]
    public class CheckpointHint : MonoBehaviour
    {
        [Header("Hint")]
        [Tooltip("The screen text that shows hints.")]
        [SerializeField]
        private TMP_Text label;

        [Tooltip("Shown after reaching this checkpoint.")]
        [SerializeField, TextArea]
        private string message;

        private Checkpoint checkpoint;

        /// <summary>Finds the checkpoint on this object.</summary>
        private void Awake()
        {
            checkpoint = GetComponent<Checkpoint>();
        }

        /// <summary>Starts listening for checkpoints.</summary>
        private void OnEnable()
        {
            Checkpoint.Reached += OnReached;
        }

        /// <summary>Stops listening.</summary>
        private void OnDisable()
        {
            Checkpoint.Reached -= OnReached;
        }

        /// <summary>Replaces the hint when this checkpoint is the one reached.</summary>
        /// <param name="reached">The checkpoint just reached.</param>
        private void OnReached(Checkpoint reached)
        {
            if (reached == checkpoint)
            {
                label.text = message;
            }
        }
    }
}