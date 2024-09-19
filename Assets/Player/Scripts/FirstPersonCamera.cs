using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class FirstPersonCamera: MonoBehaviour
{
    public Transform playerBody;
    public PlayerMovement playerMovement;
    public float mouseSensitivity = 10f;
    public float gamepadSensitivity = 125;

    // Track up/down roation
    private float xRotation = 0f;
    private Vector2 lookInput;
    private float sensitivity;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnLook(InputAction.CallbackContext context) {
        lookInput = context.ReadValue<Vector2>();

        // Determin sensitivity based on input device
        sensitivity = context.control.device.name == "Mouse" ? mouseSensitivity : gamepadSensitivity;
    }

    private void Update()
    {
        ProcessLookInput();
    }

    private void ProcessLookInput()
    {
        float mouseX = lookInput.x * sensitivity * Time.deltaTime;
        float mouseY = lookInput.y * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (!playerMovement.inZeroGravity)
        {
            // Apply rotation to camera
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Rotate the player body around the Y axis
            playerBody.Rotate(Vector3.up * mouseX);
        } else
        {
            // Only rotate the player body around both axes
            playerBody.Rotate(Vector3.up * mouseX);
            playerBody.Rotate(Vector3.right * -mouseY);
        }
    }
}