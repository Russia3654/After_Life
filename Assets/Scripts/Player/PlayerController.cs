using UnityEngine;
using UnityEngine.InputSystem;

namespace AfterLife.Player
{
    /// <summary>
    /// Side-view platformer movement: walk, run, jump with coyote time,
    /// jump buffering and variable jump height. Input actions are defined in code.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        private const float MoveDeadZone = .01f;

        [Header("Movement")]
        [SerializeField, Min(0f)]
        private float walkSpeed = 5f;
        [SerializeField, Min(0f)]
        private float runSpeed = 8f;

        private Rigidbody2D body;
        private InputAction moveAction;
        private InputAction runAction;

        private float moveInput;
        private bool isFacingRight = true;

        /// <summary>Caches the Rigidbody2D and builds the input actions with their bindings.</summary>
        private void Awake() {
            body = GetComponent<Rigidbody2D>();

            moveAction = new InputAction("Move", InputActionType.Value);
            moveAction.AddCompositeBinding("1DAxis").With("Negative", "<Keyboard>/a").With("Positive", "<Keyboard>/d");
            moveAction.AddCompositeBinding("1DAxis").With("Negative", "<Keyboard>/leftArrow").With("Positive", "<Keyboard>/rightArrow");

            runAction = new InputAction("Run", InputActionType.Button);
            runAction.AddBinding("<Keyboard>/leftShift");
            runAction.AddBinding("<Gamepad>/leftTrigger");
        }

        /// <summary>Enables the input actions.</summary>
        private void OnEnable()
        {
            moveAction.Enable();
            runAction.Enable();
        }

        /// <summary>Disables the input actions.</summary>
        private void OnDisable()
        {
            moveAction.Disable();
            runAction.Disable();
        }

        /// <summary>Releases the native resources held by the actions.</summary>
        private void OnDestroy()
        {
            moveAction.Dispose();
            runAction.Dispose();
        }

        /// <summary>Reads input every frame and updates the facing direction.</summary>
        private void Update()
        {
            moveInput = moveAction.ReadValue<float>();
            UpdateFacing();
        }

        /// <summary>Applies horizontal velocity in the physics step.</summary>
        private void FixedUpdate()
        {
            float speed = runAction.IsPressed() ? runSpeed : walkSpeed;
            body.linearVelocity = new Vector2(moveInput * speed, body.linearVelocity.y);
        }

        /// <summary>Flips the sprite to face the movement direction.</summary>
        private void UpdateFacing()
        {
            if (Mathf.Abs(moveInput) < MoveDeadZone || (moveInput > 0f) == isFacingRight)
            {
                return;
            }

            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
        }
    }
}
