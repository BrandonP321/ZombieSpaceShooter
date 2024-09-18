using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float acceleration = 9f;
    public float deceleration = 9f;
    public float jumpForce = 3f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector3 moveVelocity;
    private Vector3 currentVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        // Check if player is grounded and jump was pressed, not let go
        if (context.started && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        if (IsGrounded())
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        // Use player's local direction (forward and right) to calculate movement
        Vector3 forwardMovement = transform.forward * moveInput.y;
        Vector3 rightMovement = transform.right * moveInput.x;

        moveVelocity = forwardMovement + rightMovement;

        // Normalize the movement vector so that the player moves at the same speed in all directions
        if (moveVelocity.magnitude > 1)
        {
            moveVelocity.Normalize();
        }

        // Multiply the movement vector by the move speed
        moveVelocity *= moveSpeed;

        Vector3 targetVelocity = moveVelocity;
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, (moveVelocity.magnitude > 0 ? acceleration : deceleration) * Time.fixedDeltaTime);
        
        // Maintain vertical velocity (Y) from physics engine for jumping and falling
        currentVelocity.y = rb.velocity.y;

        // Apply movement to the rigidbody
        rb.velocity = currentVelocity;
    }

    bool IsGrounded()
    {
        // Raycast to check if the player is grounded
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);

    }
}
