using UnityEngine;

namespace AfterLife.Interactables
{
    /// <summary>
    /// Base class for anything the player can use with the interact key (switch, lever, sign).
    /// Needs a trigger collider that marks the area where the player can reach it.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public abstract class Interactable : MonoBehaviour
    {
        /// <summary>Makes sure the collider is a trigger so the player walks through it.</summary>
        private void Reset()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        /// <summary>Called by the player when they press the interact key in range.</summary>
        public abstract void Interact();
    }
}
