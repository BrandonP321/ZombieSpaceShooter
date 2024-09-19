using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerGroundMovement playerGroundMovement;
    private PlayerThrusterMovement playerThrusterMovement;

    Rigidbody rb;
    bool inZeroGravity = false;

    void Start()
    {
        playerGroundMovement = GetComponent<PlayerGroundMovement>();
        playerThrusterMovement = GetComponent<PlayerThrusterMovement>();
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (inZeroGravity)
        {
            playerThrusterMovement.OnMove(context.ReadValue<Vector2>());
        }
        else
        {
            playerGroundMovement.OnMove(context.ReadValue<Vector2>());
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (inZeroGravity)
        {
            playerThrusterMovement.OnThrustUp(context);
        }
        else
        {
            playerGroundMovement.OnJump(context);
        }
    }

    public void OnThrustDown(InputAction.CallbackContext context)
    {
        if (inZeroGravity)
        {
            playerThrusterMovement.OnThrustDown(context);
        }
    }

    private void FixedUpdate()
    {
        if (inZeroGravity)
        {
            playerThrusterMovement.ApplyThrusterMovement();
        }
        else if (IsGrounded())
        {
            playerGroundMovement.ApplyGroundMovement();
        }
    }

    // Trigger to enter zero-gravity mode
    public void EnterZeroGravity()
    {
        inZeroGravity = true;
        rb.useGravity = false;
        playerGroundMovement.DisableGroundMovement();

        InputAction moveAction = GetComponent<PlayerInput>().actions["Move"];
        playerThrusterMovement.OnMove(moveAction.ReadValue<Vector2>());
    }

    // Trigger to exit zero-gravity mode and return to normal ground movement
    public void ExitZeroGravity()
    {
        inZeroGravity = false;
        rb.useGravity = true;
        playerThrusterMovement.DisableThrust();

        InputAction moveAction = GetComponent<PlayerInput>().actions["Move"];
        playerGroundMovement.OnMove(moveAction.ReadValue<Vector2>());
    }

    public bool IsGrounded()
    {
        // Raycast to check if the player is grounded
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);

    }
}
