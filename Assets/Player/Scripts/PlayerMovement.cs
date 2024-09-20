using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerGroundMovement playerGroundMovement;
    private PlayerThrusterMovement playerThrusterMovement;

    Rigidbody rb;
    public bool inZeroGravity = false;
    public float smoothRotationDuration = 0.5f;

    public bool isRotatingToUpright = false;
    private Quaternion targetUprightRotation;

    public Transform cameraTransform;
    public GameObject playerUIPrefab;
    public GameObject InGameMenuUIPrefab;
    public InGameMenu inGameMenu;

    void Start()
    {
        playerGroundMovement = GetComponent<PlayerGroundMovement>();
        playerThrusterMovement = GetComponent<PlayerThrusterMovement>();
        rb = GetComponent<Rigidbody>();
        Instantiate(playerUIPrefab);
        inGameMenu = Instantiate(InGameMenuUIPrefab).GetComponent<InGameMenu>();
    }

    public void OnToggleInGameMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inGameMenu.ToggleMenu();
        }
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
        if (!inGameMenu.isVisible)
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

        if (isRotatingToUpright)
        {
            SmoothRotateToUpright();
        }
    }

    // This method is triggered when the player exits a zero-gravity zone
    public void StartUprightRotation()
    {
        // Set the target rotation to upright while maintaining the current y-axis rotation (facing direction)
        targetUprightRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        // Start the smooth rotation
        isRotatingToUpright = true;
    }

    private void SmoothRotateToUpright()
    {
        // Get the current rotation and isolate the Y-axis (horizontal rotation for aiming)
        Quaternion currentRotation = transform.rotation;
        float currentYRotation = currentRotation.eulerAngles.y;  // Preserve Y-axis rotation

        // Get the target upright rotation for X and Z axes, keeping the Y-axis as it is
        Quaternion uprightRotation = Quaternion.Euler(0, currentYRotation, 0);  // Keep Y-axis the same, zero X and Z

        // Smoothly interpolate the X and Z axes rotation over time
        transform.rotation = Quaternion.Slerp(currentRotation, uprightRotation, Time.deltaTime / smoothRotationDuration);

        // Check if the rotation is close enough to the target, then stop rotating
        if (Quaternion.Angle(transform.rotation, uprightRotation) < 0.1f)
        {
            isRotatingToUpright = false;  // Stop the transition once it's close enough
            transform.rotation = uprightRotation;  // Snap to the exact upright rotation
        }
    }

    // Trigger to enter zero-gravity mode
    public void EnterZeroGravity()
    {
        inZeroGravity = true;
        rb.useGravity = false;
        playerGroundMovement.DisableGroundMovement();

        // Reset the camera's pitch (x-axis rotation) to 0 to align the camera forward
        Vector3 cameraEulerAngles = cameraTransform.localEulerAngles;
        cameraEulerAngles.x = 0;  // Set the pitch to 0
        cameraTransform.localEulerAngles = cameraEulerAngles;  // Apply the new camera rotation

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

        // Reset player rotation so they are standing upright without any impact to the camera
        StartUprightRotation();
    }

    public bool IsGrounded()
    {
        // Raycast to check if the player is grounded
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);

    }
}
