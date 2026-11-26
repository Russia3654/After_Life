using System;
using UnityEngine;

namespace AfterLife.Data
{
    /// <summary>A vague line about the fox's past, unlocked after collecting enough orbs.</summary>
    [Serializable]
    public class OrbClue
    {
        [Tooltip("Orbs needed to unlock this line.")]
        [SerializeField, Min(1)]
        private int requiredOrbs = 1;
        [Tooltip("The text shown to the player.")]
        [SerializeField, TextArea]
        private string text;

        /// <summary>Orbs needed to unlock this line.</summary>
        public int RequiredOrbs => requiredOrbs;

        /// <summary>The text shown to the player.</summary>
        public string Text => text;
    }
}