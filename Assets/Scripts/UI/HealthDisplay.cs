using UnityEngine;
using UnityEngine.UI;
using AfterLife.Systems;

namespace AfterLife.UI
{
    /// <summary>Shows the player's health as a row of icons.</summary>
    public class HealthDisplay : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("The player's health.")]
        [SerializeField]
        private Health health;

        [Tooltip("One icon per hit point, from left to right.")]
        [SerializeField]
        private Image[] icons;

        [Header("Look")]
        [Tooltip("Color of a hit point the player still has.")]
        [SerializeField]
        private Color fullColor = new Color32(0xE8, 0x70, 0x5F, 0xFF);

        [Tooltip("Color of a lost hit point.")]
        [SerializeField]
        private Color emptyColor = new Color(0f, 0f, 0f, 0.25f);

        /// <summary>Listens for health changes.</summary>
        private void OnEnable()
        {
            health.Changed += OnHealthChanged;
        }

        /// <summary>Draws the starting health. Runs after Health has set it in Awake.</summary>
        private void Start()
        {
            Show(health.Current);
        }

        /// <summary>Stops listening.</summary>
        private void OnDisable()
        {
            health.Changed -= OnHealthChanged;
        }

        /// <summary>Redraws when the player is hurt or restored.</summary>
        /// <param name="current">Hit points left.</param>
        /// <param name="max">Hit points when full.</param>
        private void OnHealthChanged(int current, int max)
        {
            Show(current);
        }

        /// <summary>Colors one icon for each remaining hit point.</summary>
        /// <param name="current">Hit points left.</param>
        private void Show(int current)
        {
            for (int i = 0; i < icons.Length; i++)
            {
                icons[i].color = i < current ? fullColor : emptyColor;
            }
        }
    }
}