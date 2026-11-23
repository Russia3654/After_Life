using System;
using UnityEngine;

namespace AfterLife.Data
{
    /// <summary>
    /// An asset that carries a "something happened" signal between objects that do not know each other.
    /// Create one per signal, for example one per switch-and-door pair.
    /// </summary>
    [CreateAssetMenu(fileName = "NewEventChannel", menuName = "AfterLife/Event Channel")]
    public class GameEventChannel : ScriptableObject
    {
        /// <summary>Raised when <see cref="Raise"/> is called.</summary>
        public event Action Raised;

        /// <summary>Notifies every listener. Can also be triggered from the Inspector context menu.</summary>
        [ContextMenu("Raise")]
        public void Raise()
        {
            Raised?.Invoke();
        }
    }
}
