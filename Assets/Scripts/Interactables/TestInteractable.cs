using UnityEngine;

namespace AfterLife.Interactables
{
    /// <summary>Temporary interactable that logs a message. Deleted in step 3.4.</summary>
    public class TestInteractable : Interactable
    {
        /// <summary>Logs that the interaction worked.</summary>
        public override void Interact()
        {
            Debug.Log("Interacted!");
        }
    }
}