using UnityEngine;
using AfterLife.Data;
using AfterLife.Dimensions;

namespace AfterLife.Interactables
{
    /// <summary>
    /// A lever the player flips with the interact key. It raises its event channel once.
    /// Anything listening to that channel (a door, for example) reacts.
    /// </summary>
    public class Switch : Interactable
    {
        [Header("Setup")]
        [Tooltip("Unique, never-changing ID used by the save system.")]
        [SerializeField]
        private string uniqueId;
        [Tooltip("The channel raised when the switch is flipped.")]
        [SerializeField]
        private GameEventChannel channel;
        [Tooltip("Plays the activation animation.")]
        [SerializeField]
        private DimensionAnimation visual;

        /// <summary>The stable ID of this switch.</summary>
        public string UniqueId => uniqueId;

        /// <summary>True once the switch has been flipped.</summary>
        public bool IsOn { get; private set; }

        /// <summary>Flips the switch the first time the player uses it.</summary>
        public override void Interact()
        {
            if (IsOn)
            {
                return;
            }

            TurnOn();
            ReportUse();
        }

        /// <summary>Turns the switch on, tints it and raises the channel.</summary>
        public void TurnOn()
        {
            IsOn = true;
            visual.Show(true);
            channel.Raise();
        }
    }
}
