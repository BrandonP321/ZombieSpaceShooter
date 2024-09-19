using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;

public class PlayerGroundMovement : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float acceleration = 9f;
    public float deceleration = 9f;
    public float jumpForce = 1500f;

    private Rigidbody rb;
    private PlayerMovement playerMovement;
    private Vector2 moveInput;
    private Vector3 currentVelocity;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(Vector2 input)
    {
        moveInput = input;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Check if player is grounded and jump was pressed, not let go
        if (context.performed && playerMovement.IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void ApplyGroundMovement()
    {
        // Use player's local direction (forward and right) to calculate movement
        Vector3 forwardMovement = transform.forward * moveInput.y;
        Vector3 rightMovement = transform.right * moveInput.x;

        Vector3 moveVelocity = forwardMovement + rightMovement;

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

    public void DisableGroundMovement()
    {
        // clear input
        moveInput = Vector2.zero;
    }
}
