using UnityEngine;

namespace AfterLife.Enemies
{
    /// <summary>Walks back and forth horizontally around its start position.</summary>
    public class PatrolEnemy : Enemy
    {
        [Header("Obstacles")]
        [Tooltip("Layers that make the enemy turn around, for example Pushable.")]
        [SerializeField]
        private LayerMask blockingLayers;
        private float startX;
        private float direction = 1f;

        /// <summary>Remembers the start position.</summary>
        protected override void Awake()
        {
            base.Awake();
            startX = transform.position.x;
        }

        /// <summary>Steps along the patrol path and turns around at either end.</summary>
        protected override void Move()
        {
            if ((Body.position.x - startX) * direction >= Data.PatrolDistance)
            {
                TurnAround();
            }

            Vector2 step = Vector2.right * (direction * Data.MoveSpeed * Time.fixedDeltaTime);
            Body.MovePosition(Body.position + step);
        }

        /// <summary>Turns around when a blocking object appears in front of the enemy.</summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            bool isBlocking = (blockingLayers.value & (1 << other.gameObject.layer)) != 0;
            bool isAhead = (other.bounds.center.x - Body.position.x) * direction > 0f;

            if (isBlocking && isAhead)
            {
                TurnAround();
            }
        }

        /// <summary>Reverses direction and mirrors the sprite.</summary>
        private void TurnAround()
        {
            direction = -direction;
            Vector3 scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
        }
    }
}