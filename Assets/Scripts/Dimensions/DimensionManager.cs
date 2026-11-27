using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AfterLife.Dimensions
{
    /// <summary>
    /// Owns the current dimension and switches it when the player presses the switch button.
    /// Other scripts react by subscribing to <see cref="DimensionChanged"/>.
    /// </summary>
    public class DimensionManager : MonoBehaviour
    {
        [Header("Setup")]
        [Tooltip("The world the player starts in.")]
        [SerializeField]
        private Dimension startingDimension = Dimension.Light;

        [Tooltip("Seconds before the player can switch again.")]
        [SerializeField, Min(0f)]
        private float switchCooldown = 0.5f;

        private InputAction switchAction;
        private float nextSwitchTime;

        /// <summary>Raised whenever the active dimension is set, including once at startup.</summary>
        public static event Action<Dimension> DimensionChanged;

        /// <summary>Raised only when the player presses the switch button, not on respawn or load.</summary>
        public static event Action PlayerSwitched;

        /// <summary>The dimension the player is currently in.</summary>
        public Dimension Current { get; private set; }

        /// <summary>Sets the starting dimension and builds the switch input action.</summary>
        private void Awake()
        {
            Current = startingDimension;

            switchAction = new InputAction("Switch", InputActionType.Button);
            switchAction.AddBinding("<Keyboard>/q");
            switchAction.AddBinding("<Gamepad>/buttonNorth");
        }

        /// <summary>Enables the input and listens for the switch button.</summary>
        private void OnEnable()
        {
            switchAction.performed += OnSwitchPerformed;
            switchAction.Enable();
        }

        /// <summary>Announces the starting dimension so listeners can apply it.</summary>
        private void Start()
        {
            DimensionChanged?.Invoke(Current);
        }

        /// <summary>Stops listening and disables the input.</summary>
        private void OnDisable()
        {
            switchAction.performed -= OnSwitchPerformed;
            switchAction.Disable();
        }

        /// <summary>Releases the native resources held by the action.</summary>
        private void OnDestroy()
        {
            switchAction.Dispose();
        }

        /// <summary>Switches to the other dimension.</summary>
        public void Switch()
        {
            SetDimension(Current == Dimension.Light ? Dimension.Shadow : Dimension.Light);
        }

        /// <summary>Sets the given dimension and notifies all listeners.</summary>
        /// <param name="dimension">The dimension to enter.</param>
        public void SetDimension(Dimension dimension)
        {
            Current = dimension;
            DimensionChanged?.Invoke(Current);
        }

        /// <summary>Called by the input system when the switch button is pressed. Ignored during the cooldown.</summary>
        private void OnSwitchPerformed(InputAction.CallbackContext context)
        {
            if (Time.time < nextSwitchTime)
            {
                return;
            }

            nextSwitchTime = Time.time + switchCooldown;
            Switch();
            PlayerSwitched?.Invoke();
        }
    }
}
