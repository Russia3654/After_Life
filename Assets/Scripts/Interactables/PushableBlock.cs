using UnityEngine;

namespace AfterLife.Interactables
{
    /// <summary>
    /// A crate the player pushes by walking into it. It never rotates and can be reset to its start position.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PushableBlock : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("Unique, never-changing ID used by the save system.")]
        [SerializeField]
        private string uniqueId;

        private Rigidbody2D body;
        private Vector3 startPosition;

        /// <summary>The stable ID of this block.</summary>
        public string UniqueId => uniqueId;

        /// <summary>Sets sensible defaults when the component is added: heavy and unable to rotate.</summary>
        private void Reset()
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.mass = 3f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        /// <summary>Caches the Rigidbody2D and remembers where the block started.</summary>
        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            startPosition = transform.position;
        }

        /// <summary>Puts the block back where it started and stops it moving.</summary>
        public void ResetToStart()
        {
            body.linearVelocity = Vector2.zero;
            transform.position = startPosition;
        }
    }
}