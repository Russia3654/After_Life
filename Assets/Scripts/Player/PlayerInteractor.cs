using UnityEngine;
using UnityEngine.InputSystem;
using AfterLife.Interactables;

namespace AfterLife.Player
{
    /// <summary>
    /// Lets the player use the nearest interactable in range with the interact key.
    /// </summary>
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("How far from the player an interactable can be used.")]
        [SerializeField, Min(0f)]
        private float interactRadius = 1.2f;
        [Tooltip("Layers that contain interactables.")]
        [SerializeField]
        private LayerMask interactableLayers;

        private InputAction interactAction;
        private readonly Collider2D[] results = new Collider2D[8];

        /// <summary>Builds the interact input action.</summary>
        private void Awake()
        {
            interactAction = new InputAction("Interact", InputActionType.Button);
            interactAction.AddBinding("<Keyboard>/e");
            interactAction.AddBinding("<Gamepad>/buttonWest");
        }

        /// <summary>Enables the input and listens for the interact key.</summary>
        private void OnEnable()
        {
            interactAction.performed += OnInteractPerformed;
            interactAction.Enable();
        }

        /// <summary>Stops listening and disables the input.</summary>
        private void OnDisable()
        {
            interactAction.performed -= OnInteractPerformed;
            interactAction.Disable();
        }

        /// <summary>Releases the native resources held by the action.</summary>
        private void OnDestroy()
        {
            interactAction.Dispose();
        }

        /// <summary>Uses the closest interactable in range, if any.</summary>
        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            Interactable target = FindClosestInteractable();

            if (target == null)
            {
                return;
            }

            target.Interact();
        }

        /// <summary>Finds the closest interactable within the interact radius.</summary>
        /// <returns>The closest interactable, or null when none is in range.</returns>
        private Interactable FindClosestInteractable()
        {
            int count = Physics2D.OverlapCircleNonAlloc(transform.position, interactRadius, results, interactableLayers);
            Interactable closest = null;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                if (!results[i].TryGetComponent(out Interactable candidate))
                {
                    continue;
                }

                float distance = Vector2.Distance(transform.position, results[i].transform.position);

                if (distance < closestDistance)
                {
                    closest = candidate;
                    closestDistance = distance;
                }
            }

            return closest;
        }

        /// <summary>Draws the interact range in the Scene view when the player is selected.</summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, interactRadius);
        }
    }
}
