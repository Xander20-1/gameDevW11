
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 120f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Keyboard.current == null) return;

        // 1. Get keyboard input
        float moveInput = 0f;
        float turnInput = 0f;

        if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.upArrowKey.isPressed)
            moveInput = 1f;

        if (Keyboard.current.sKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed)
            moveInput = -1f;

        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
            turnInput = -1f;

        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
            turnInput = 1f;

        // 2. Rotate tank
        transform.Rotate(
            Vector3.up,
            turnInput * turnSpeed * Time.deltaTime,
            Space.World
        );

        // 3. Calculate forward/backward movement
        Vector3 moveDirection =
            transform.forward * moveInput * moveSpeed;

        // 4. Apply gravity
        if (controller.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        // 5. Jump
        if (controller.isGrounded &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            velocity.y = Mathf.Sqrt(
                jumpHeight * -2f * gravity
            );
        }

        velocity.y += gravity * Time.deltaTime;

        // 6. Apply movement
        Vector3 finalMovement =
            moveDirection + velocity;

        controller.Move(
            finalMovement * Time.deltaTime
        );
    }
}
