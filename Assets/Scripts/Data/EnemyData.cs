using UnityEngine;

namespace AfterLife.Data
{
    /// <summary>Tunable stats shared by every enemy of one kind.</summary>
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "AfterLife/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [Header("Combat")]
        [Tooltip("Hit points the player loses on touch.")]
        [SerializeField, Min(1)]
        private int contactDamage = 1;

        [Header("Movement")]
        [Tooltip("Units per second.")]
        [SerializeField, Min(0f)]
        private float moveSpeed = 2f;
        [Tooltip("Distance travelled to each side of the start position.")]
        [SerializeField, Min(0f)]
        private float patrolDistance = 3f;

        /// <summary>Hit points the player loses on touch.</summary>
        public int ContactDamage => contactDamage;

        /// <summary>Units per second.</summary>
        public float MoveSpeed => moveSpeed;

        /// <summary>Distance travelled to each side of the start position.</summary>
        public float PatrolDistance => patrolDistance;
    }
}