using UnityEngine;

namespace AfterLife.Enemies
{
    /// <summary>Walks back and forth horizontally around its start position.</summary>
    public class PatrolEnemy : Enemy
    {
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