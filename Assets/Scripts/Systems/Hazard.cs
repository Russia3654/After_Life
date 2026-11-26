using UnityEngine;

namespace AfterLife.Systems
{
    /// <summary>
    /// Damages anything with a <see cref="Health"/> component for as long as it touches this trigger.
    /// Health's invulnerability time limits how often the damage lands.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Hazard : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("Hit points removed per hit.")]
        [SerializeField, Min(1)]
        private int damage = 1;

        /// <summary>Makes sure the collider is a trigger so objects can touch it.</summary>
        private void Reset()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        /// <summary>Damages the object each physics step it stays inside the hazard.</summary>
        /// <param name="other">The collider inside the trigger.</param>
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent(out Health health))
            {
                health.TakeDamage(damage);
            }
        }
    }
}