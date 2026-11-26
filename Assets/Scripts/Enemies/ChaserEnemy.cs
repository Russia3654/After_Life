using UnityEngine;

namespace AfterLife.Enemies
{
    /// <summary>Follows the player when in range and jumps over obstacles or up toward the player.</summary>
    public class ChaserEnemy : Enemy
    {
        private const float StopDistance = 0.1f;
        private const float ProbeThickness = 0.1f;
        private const float ProbeShrink = 0.8f;
        private const float GroundedMaxRiseSpeed = 0.01f;

        [Header("Chase")]
        [Tooltip("The player to follow.")]
        [SerializeField]
        private Transform target;
        [Tooltip("The solid (non-trigger) collider that stands on the ground.")]
        [SerializeField]
        private Collider2D bodyCollider;
        [Tooltip("Layers the enemy can stand on and jump over (Ground, Pushable).")]
        [SerializeField]
        private LayerMask obstacleLayers;
        [Tooltip("Jump when the player is at least this much higher.")]
        [SerializeField, Min(0f)]
        private float jumpHeightThreshold = 1.5f;

        /// <summary>Walks toward the player while in range and jumps when needed.</summary>
        protected override void Move()
        {
            Vector2 toTarget = (Vector2)target.position - Body.position;

            if (toTarget.magnitude > Data.DetectionRange || Mathf.Abs(toTarget.x) < StopDistance)
            {
                Body.linearVelocity = new Vector2(0f, Body.linearVelocity.y);
                return;
            }

            float direction = Mathf.Sign(toTarget.x);
            Body.linearVelocity = new Vector2(direction * Data.MoveSpeed, Body.linearVelocity.y);

            if (IsGrounded() && (IsBlocked(direction) || toTarget.y > jumpHeightThreshold))
            {
                Body.linearVelocity = new Vector2(Body.linearVelocity.x, Data.JumpVelocity);
            }
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
        /// <param name="direction">1 for right, -1 for left.</param>
        private bool IsBlocked(float direction)
        {
            Bounds bounds = bodyCollider.bounds;
            float frontX = bounds.center.x + direction * (bounds.extents.x + ProbeThickness);
            Vector2 center = new Vector2(frontX, bounds.center.y);
            Vector2 size = new Vector2(ProbeThickness, bounds.size.y * ProbeShrink);

            return Physics2D.OverlapBox(center, size, 0f, obstacleLayers) != null;
        }
    }
}