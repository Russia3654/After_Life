using System;
using UnityEngine;

namespace AfterLife.Systems
{
    /// <summary>
    /// Tracks hit points. Hazards and enemies call <see cref="TakeDamage"/>; respawn and the HUD listen to the events.
    /// </summary>
    public class Health : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("Hit points when full.")]
        [SerializeField, Min(1)]
        private int maxHealth = 3;
        [Tooltip("Seconds after a hit during which further damage is ignored.")]
        [SerializeField, Min(0f)]
        private float invulnerabilityTime = 1f;

        private float invulnerableUntil;

        /// <summary>Raised when health changes, with the current and maximum values.</summary>
        public event Action<int, int> Changed;

        /// <summary>Raised once when health reaches zero.</summary>
        public event Action Died;

        /// <summary>Current hit points.</summary>
        public int Current { get; private set; }

        /// <summary>True when no hit points remain.</summary>
        public bool IsDead => Current <= 0;

        /// <summary>Starts at full health.</summary>
        private void Awake()
        {
            Current = maxHealth;
        }

        /// <summary>Removes hit points unless dead or still invulnerable from the last hit.</summary>
        /// <param name="amount">Hit points to remove.</param>
        public void TakeDamage(int amount)
        {
            if (amount <= 0 || IsDead || Time.time < invulnerableUntil)
            {
                return;
            }

            invulnerableUntil = Time.time + invulnerabilityTime;
            Current = Mathf.Max(Current - amount, 0);
            Debug.Log($"{name} health: {Current}/{maxHealth}"); // Temporary. The HUD replaces it in Stage 6.
            Changed?.Invoke(Current, maxHealth);

            if (IsDead)
            {
                Died?.Invoke();
            }
        }

        /// <summary>Restores full health, for example after respawning.</summary>
        public void Restore()
        {
            Current = maxHealth;
            Changed?.Invoke(Current, maxHealth);
        }

        /// <summary>Test helper: deals one damage from the component's context menu.</summary>
        [ContextMenu("Take 1 Damage")]
        private void TakeOneDamage()
        {
            TakeDamage(1);
        }
    }
}