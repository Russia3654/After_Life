using UnityEngine;
using AfterLife.Data;
using AfterLife.Systems;

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

        /// <summary>Caches the rigidbody.</summary>
        protected virtual void Awake()
        {
            Body = GetComponent<Rigidbody2D>();
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