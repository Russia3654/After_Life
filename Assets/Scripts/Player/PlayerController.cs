using UnityEngine;
using UnityEngine.InputSystem;

namespace AfterLife.Player
{
    /// <summary>
    /// Side-view movement: walk, run, jump and ground detection.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        private const float MoveDeadZone = .01f;
        private const float GroundedMaxRiseSpeed = 0.01f;

        [Header("Movement")]
        [SerializeField, Min(0f)]
        private float walkSpeed = 5f;
        [SerializeField, Min(0f)]
        private float runSpeed = 8f;

        [Header("Jump")]
        [Tooltip("Upward speed applied when jumping.")]
        [SerializeField, Min(0f)]
        private float jumpVelocity = 13f;
        [Tooltip("Seconds after leaving a ledge during which a jump is still allowed.")]
        [SerializeField, Min(0f)]
        private float coyoteTime = 0.1f;
        [Tooltip("Seconds a jump press is remembered before landing.")]
        [SerializeField, Min(0f)]
        private float jumpBufferTime = 0.1f;
        [Tooltip("Upward speed multiplier when the jump button is released early.")]
        [SerializeField, Range(0f, 1f)]
        private float jumpCutMultiplier = 0.5f;

        [Header("Ground Check")]
        [SerializeField]
        private Transform groundCheck;
        [SerializeField]
        private Vector2 groundCheckSize = new Vector2(0.45f, 0.1f);
        [SerializeField]
        private LayerMask groundLayers;

        private Rigidbody2D body;
        /// <summary>Raised when the player jumps.</summary>
        public static event System.Action Jumped;
        private InputAction moveAction;
        private InputAction runAction;
        private InputAction jumpAction;

        private float moveInput;
        private float coyoteTimer;
        private float jumpBufferTimer;
        private bool isFacingRight = true;

        /// <summary>True while the player stands on a ground layer.</summary>
        public bool IsGrounded { get; private set; }

        /// <summary>Caches the Rigidbody2D and builds the input actions with their bindings.</summary>
        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();

            moveAction = new InputAction("Move", InputActionType.Value);
            moveAction.AddCompositeBinding("1DAxis").With("Negative", "<Keyboard>/a").With("Positive", "<Keyboard>/d");
            moveAction.AddCompositeBinding("1DAxis").With("Negative", "<Keyboard>/leftArrow").With("Positive", "<Keyboard>/rightArrow");

            runAction = new InputAction("Run", InputActionType.Button);
            runAction.AddBinding("<Keyboard>/leftShift");
            runAction.AddBinding("<Gamepad>/leftTrigger");

            jumpAction = new InputAction("Jump", InputActionType.Button);
            jumpAction.AddBinding("<Keyboard>/space");
            jumpAction.AddBinding("<Gamepad>/buttonSouth");
        }

        /// <summary>Enables the input actions.</summary>
        private void OnEnable()
        {
            moveAction.Enable();
            runAction.Enable();
            jumpAction.Enable();
        }

        /// <summary>Disables the input actions.</summary>
        private void OnDisable()
        {
            moveAction.Disable();
            runAction.Disable();
            jumpAction.Disable();
        }

        /// <summary>Releases the native resources held by the actions.</summary>
        private void OnDestroy()
        {
            moveAction.Dispose();
            runAction.Dispose();
            jumpAction.Dispose();
        }

        /// <summary>Reads input every frame and updates the facing direction.</summary>
        private void Update()
        {
            moveInput = moveAction.ReadValue<float>();
            UpdateFacing();
            UpdateGroundedState();
            UpdateJumpTimers();
            TryJump();
            ApplyJumpCut();
        }

        /// <summary>Applies horizontal velocity in the physics step.</summary>
        private void FixedUpdate()
        {
            float speed = runAction.IsPressed() ? runSpeed : walkSpeed;
            body.linearVelocity = new Vector2(moveInput * speed, body.linearVelocity.y);
        }

        /// <summary>Checks for ground below the player, ignoring frames where it is still rising.</summary>
        private void UpdateGroundedState()
        {
            bool touchingGround = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayers);
            IsGrounded = touchingGround && body.linearVelocity.y <= GroundedMaxRiseSpeed;
        }

        /// <summary>Refreshes the coyote and jump-buffer timers.</summary>
        private void UpdateJumpTimers()
        {
            coyoteTimer = IsGrounded ? coyoteTime : coyoteTimer - Time.deltaTime;
            jumpBufferTimer = jumpAction.WasPressedThisFrame() ? jumpBufferTime : jumpBufferTimer - Time.deltaTime;
        }

        /// <summary>Jumps when a buffered press meets a valid coyote window.</summary>
        private void TryJump()
        {
            if (jumpBufferTimer <= 0f || coyoteTimer <= 0f)
            {
                return;
            }

            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpVelocity);
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
            Jumped?.Invoke();
        }

        /// <summary>Shortens the jump when the button is released while still rising.</summary>
        private void ApplyJumpCut()
        {
            if (!jumpAction.WasReleasedThisFrame() || body.linearVelocity.y <= 0f)
            {
                return;
            }

            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y * jumpCutMultiplier);
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

        /// <summary>Draws the ground check box in the Scene view when the player is selected.</summary>
        private void OnDrawGizmosSelected()
        {
            if (groundCheck == null)
            {
                return;
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}
