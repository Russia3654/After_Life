using UnityEngine;

namespace AfterLife.Enemies
{
    /// <summary>Roams the level looking for the player, then chases them, jumping over obstacles.</summary>
    public class ChaserEnemy : Enemy
    {
        private const float StopDistance = 0.1f;
        private const float ProbeThickness = 0.1f;
        private const float ProbeShrink = 0.8f;
        private const float GroundedMaxRiseSpeed = 0.01f;
        private const float StuckStepFraction = 0.25f;

        [Header("Chase")]
        [Tooltip("The player to follow.")]
        [SerializeField]
        private Transform target;
        [Tooltip("The solid (non-trigger) collider that stands on the ground.")]
        [SerializeField]
        private Collider2D bodyCollider;
        [Tooltip("The sprite to turn toward the walking direction. The art faces right.")]
        [SerializeField]
        private SpriteRenderer visual;
        [Tooltip("Layers the enemy can stand on and jump over (Ground, Pushable).")]
        [SerializeField]
        private LayerMask obstacleLayers;
        [Tooltip("Jump when the player is at least this much higher.")]
        [SerializeField, Min(0f)]
        private float jumpHeightThreshold = 1.5f;

        [Header("Roam")]
        [Tooltip("Fraction of the move speed used while searching.")]
        [SerializeField, Range(0f, 1f)]
        private float roamSpeedMultiplier = 0.6f;
        [Tooltip("Deepest drop the enemy walks down while searching. Deeper drops make it turn around.")]
        [SerializeField, Min(0f)]
        private float maxDrop = 3f;
        [Tooltip("Seconds without progress before the enemy turns around while searching.")]
        [SerializeField, Min(0f)]
        private float stuckTime = 1f;

        private float direction = 1f;
        private float stuckTimer;
        private float lastX;

        /// <summary>Chases the player when in range, otherwise searches.</summary>
        protected override void Move()
        {
            Vector2 toTarget = (Vector2)target.position - Body.position;

            if (toTarget.magnitude <= Data.DetectionRange)
            {
                Chase(toTarget);
                return;
            }

            Roam();
        }

        /// <summary>Walks toward the player and jumps over obstacles or up toward them.</summary>
        /// <param name="toTarget">Vector from the enemy to the player.</param>
        private void Chase(Vector2 toTarget)
        {
            stuckTimer = 0f;

            if (Mathf.Abs(toTarget.x) < StopDistance)
            {
                Walk(0f);
                return;
            }

            direction = Mathf.Sign(toTarget.x);
            Walk(Data.MoveSpeed);

            if (IsGrounded() && (IsBlocked() || toTarget.y > jumpHeightThreshold))
            {
                Jump();
            }
        }

        /// <summary>Walks in the current direction, turning around at deep drops and dead ends.</summary>
        private void Roam()
        {
            float speed = Data.MoveSpeed * roamSpeedMultiplier;

            if (IsGrounded())
            {
                if (!HasGroundAhead())
                {
                    TurnAround();
                }
                else if (IsBlocked())
                {
                    Jump();
                }
            }

            UpdateStuckTimer(speed);
            Walk(speed);
        }

        /// <summary>Turns around when the enemy has made no horizontal progress for too long.</summary>
        /// <param name="speed">The speed the enemy is trying to move at.</param>
        private void UpdateStuckTimer(float speed)
        {
            float moved = Mathf.Abs(Body.position.x - lastX);
            lastX = Body.position.x;

            bool isStuck = moved < speed * Time.fixedDeltaTime * StuckStepFraction;
            stuckTimer = isStuck ? stuckTimer + Time.fixedDeltaTime : 0f;

            if (stuckTimer >= stuckTime)
            {
                TurnAround();
            }
        }

        /// <summary>Sets the horizontal velocity in the current direction.</summary>
        /// <param name="speed">Units per second.</param>
        private void Walk(float speed)
        {
            Body.linearVelocity = new Vector2(direction * speed, Body.linearVelocity.y);
            visual.flipX = direction < 0f;
        }

        /// <summary>Launches the enemy upward.</summary>
        private void Jump()
        {
            Body.linearVelocity = new Vector2(Body.linearVelocity.x, Data.JumpVelocity);
        }

        /// <summary>Reverses the search direction.</summary>
        private void TurnAround()
        {
            direction = -direction;
            stuckTimer = 0f;
        }

        /// <summary>True when the enemy stands on an obstacle layer and isn't rising.</summary>
        private bool IsGrounded()
        {
            Bounds bounds = bodyCollider.bounds;
            Vector2 center = new Vector2(bounds.center.x, bounds.min.y - ProbeThickness * 0.5f);
            Vector2 size = new Vector2(bounds.size.x * ProbeShrink, ProbeThickness);

            return Body.linearVelocity.y <= GroundedMaxRiseSpeed
                && Physics2D.OverlapBox(center, size, 0f, obstacleLayers) != null;
        }

        /// <summary>True when an obstacle is right in front of the enemy.</summary>
        private bool IsBlocked()
        {
            Bounds bounds = bodyCollider.bounds;
            Vector2 center = new Vector2(FrontX(bounds), bounds.center.y);
            Vector2 size = new Vector2(ProbeThickness, bounds.size.y * ProbeShrink);

            return Physics2D.OverlapBox(center, size, 0f, obstacleLayers) != null;
        }

        /// <summary>True when there is ground within the allowed drop in front of the enemy.</summary>
        private bool HasGroundAhead()
        {
            Bounds bounds = bodyCollider.bounds;
            Vector2 origin = new Vector2(FrontX(bounds), bounds.min.y + ProbeThickness);

            return Physics2D.Raycast(origin, Vector2.down, maxDrop + ProbeThickness, obstacleLayers);
        }

        /// <summary>The x position just beyond the enemy's leading edge.</summary>
        /// <param name="bounds">Bounds of the solid collider.</param>
        private float FrontX(Bounds bounds)
        {
            return bounds.center.x + direction * (bounds.extents.x + ProbeThickness);
        }

        /// <summary>Also restarts the search.</summary>
        public override void ResetToStart()
        {
            base.ResetToStart();
            direction = 1f;
            stuckTimer = 0f;
            lastX = transform.position.x;
        }
    }
}