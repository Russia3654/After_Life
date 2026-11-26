using UnityEngine;
using AfterLife.Data;
using AfterLife.Systems;
using AfterLife.Player;

namespace AfterLife.Enemies
{
    /// <summary>Base class for enemies: damages the player on touch and moves in the physics step.</summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class Enemy : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("Stats for this enemy.")]
        [SerializeField]
        private EnemyData data;

        private Vector3 startPosition;
        private Vector3 startScale;

        /// <summary>Stats for this enemy.</summary>
        protected EnemyData Data => data;

        /// <summary>The enemy's rigidbody.</summary>
        protected Rigidbody2D Body { get; private set; }

        /// <summary>Sets a kinematic body and a trigger collider when the component is added.</summary>
        private void Reset()
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<Collider2D>().isTrigger = true;
        }

        /// <summary>Caches the rigidbody and remembers the starting pose.</summary>
        protected virtual void Awake()
        {
            Body = GetComponent<Rigidbody2D>();
            startPosition = transform.position;
            startScale = transform.localScale;
        }

        /// <summary>Listens for the player respawning.</summary>
        private void OnEnable()
        {
            PlayerRespawn.Respawned += ResetToStart;
        }

        /// <summary>Stops listening.</summary>
        private void OnDisable()
        {
            PlayerRespawn.Respawned -= ResetToStart;
        }

        /// <summary>Puts the enemy back where it started.</summary>
        public virtual void ResetToStart()
        {
            Body.linearVelocity = Vector2.zero;
            Body.position = startPosition;
            transform.position = startPosition;
            transform.localScale = startScale;
        }

        /// <summary>Moves the enemy every physics step.</summary>
        private void FixedUpdate()
        {
            Move();
        }

        /// <summary>Damages anything with health that stays inside the enemy.</summary>
        /// <param name="other">The collider inside the trigger.</param>
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent(out Health health))
            {
                health.TakeDamage(data.ContactDamage);
            }
        }

        /// <summary>Moves the enemy for one physics step.</summary>
        protected abstract void Move();
    }
}