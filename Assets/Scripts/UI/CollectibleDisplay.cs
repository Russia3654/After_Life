using TMPro;
using UnityEngine;
using AfterLife.Systems;

namespace AfterLife.UI
{
    /// <summary>Shows the held keys and collected orbs as text.</summary>
    public class CollectibleDisplay : MonoBehaviour
    {
        [Header("Sources")]
        [Tooltip("The scene's key inventory.")]
        [SerializeField]
        private KeyInventory keys;

        [Tooltip("The scene's orb counter.")]
        [SerializeField]
        private OrbCounter orbs;

        [Header("Texts")]
        [Tooltip("Text for the key count.")]
        [SerializeField]
        private TMP_Text keyText;

        [Tooltip("Text for the orb count.")]
        [SerializeField]
        private TMP_Text orbText;

        [Header("Format")]
        [Tooltip("{0} is the number of keys.")]
        [SerializeField]
        private string keyFormat = "Keys {0}";

        [Tooltip("{0} is collected orbs, {1} is the total.")]
        [SerializeField]
        private string orbFormat = "Orbs {0}/{1}";

        /// <summary>Listens for both counts.</summary>
        private void OnEnable()
        {
            keys.Changed += ShowKeys;
            orbs.Changed += ShowOrbs;
        }

        /// <summary>Draws the starting values.</summary>
        private void Start()
        {
            ShowKeys();
            ShowOrbs();
        }

        /// <summary>Stops listening.</summary>
        private void OnDisable()
        {
            keys.Changed -= ShowKeys;
            orbs.Changed -= ShowOrbs;
        }

        /// <summary>Writes the key count.</summary>
        private void ShowKeys()
        {
            keyText.text = string.Format(keyFormat, keys.Count);
        }

        /// <summary>Writes the orb count.</summary>
        private void ShowOrbs()
        {
            orbText.text = string.Format(orbFormat, orbs.Count, orbs.Total);
        }
    }
}