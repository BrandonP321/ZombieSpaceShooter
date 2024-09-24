using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrusterMovement : MonoBehaviour
{
    public float thrusterForce = 5f;
    public float thrusterEnergyMax = 100f;
    public float thrusterRechargeRate = 50f;
    private float thrusterRechargeDelaySec = 1f;
    public float thrusterConsumptionRate = 15f;
    public float rotationSpeed = 40f;
    public float thrusterMinForMovement = 15f;
    public TextMeshPro thrusterEnergyText;

    private Rigidbody rb;
    private Vector3 movementInput;
    private PlayerMovement playerMovement;
    private float verticalThrust;
    private float leftRotationThrust;
    private float rightRotationThrust;
    private float thrusterEnergy;
    private bool isThrusterActive = false;
    private float lastThrusterDeactivatedTime;

    public delegate void OnThrustEnergyChanged(float currentEnergy, bool isDisabled);
    public static event OnThrustEnergyChanged ThrustEnergyChanged;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        thrusterEnergy = thrusterEnergyMax;
        UpdateThrustUI();
    }

    public void OnMove(Vector2 input)
    {
        movementInput = new Vector3(input.x, 0, input.y);
    }

    public void OnThrustUp(InputAction.CallbackContext context)
    {
        UpdateVerticalThrust(context, 1f);
    }

    public void OnThrustDown(InputAction.CallbackContext context)
    {
        UpdateVerticalThrust(context, -1f);
    }

    public void OnThrustRotateLeft(InputAction.CallbackContext context)
    {
        if (context.performed && playerMovement.inZeroGravity)
        {
            leftRotationThrust = 1f;
        }
        else if (context.canceled)
        {
            leftRotationThrust = 0f;
        }
    }

    public void OnThrustRotateRight(InputAction.CallbackContext context)
    {
        if (context.performed && playerMovement.inZeroGravity)
        {
            rightRotationThrust = 1f;
        }
        else if (context.canceled)
        {
            rightRotationThrust = 0f;
        }
    }

    private void UpdateVerticalThrust(InputAction.CallbackContext context, float thrust)
    {
        if (context.performed && thrusterEnergy > 0)
        {
            verticalThrust = thrust;
        }
        else if (context.canceled)
        {
            verticalThrust = 0f;
        }
    }

    public void ApplyThrusterMovement()
    {
        bool isThrusterInput = movementInput != Vector3.zero || verticalThrust != 0 || leftRotationThrust != 0 || rightRotationThrust != 0;
        if (thrusterEnergy > 0 && isThrusterInput)
        {
            isThrusterActive = true;
            lastThrusterDeactivatedTime = 0;

            Vector3 thrusterDirection = transform.right * movementInput.x + transform.forward * movementInput.z;
            thrusterDirection += transform.up * verticalThrust;  // Add vertical thrust

            // Apply thruster force
            rb.AddForce(thrusterDirection.normalized * thrusterForce, ForceMode.Acceleration);

            // Apply continuous rotation based on the left and right thrusts
            float rotationThrust = (rightRotationThrust - leftRotationThrust) * -rotationSpeed;
            if (rotationThrust != 0)
            {
                transform.Rotate(Vector3.forward * rotationThrust * Time.deltaTime);
            }

            // Deplete thruster energy
            UpdateThrusterEngery(-thrusterConsumptionRate * Time.deltaTime);
        } else if (lastThrusterDeactivatedTime == 0)
        {
            isThrusterActive = false;
            lastThrusterDeactivatedTime = Time.time;
        }

        UpdateThrustUI();
    }

    private void Update()
    {
        // Recharge thruster energy over time
        if (thrusterEnergy < thrusterEnergyMax && Time.time - lastThrusterDeactivatedTime > thrusterRechargeDelaySec && !isThrusterActive)
        {
            UpdateThrusterEngery(thrusterRechargeRate * Time.deltaTime);
        }
    }

    private void UpdateThrusterEngery(float deltaEnergy)
    {
        thrusterEnergy += deltaEnergy;
        thrusterEnergy = Mathf.Clamp(thrusterEnergy, 0, thrusterEnergyMax);
    }

    public void DisableThrust()
    {
        verticalThrust = 0f;
        movementInput = Vector3.zero;
    }

    public void UpdateThrustUI()
    {
        bool isThrusterDisabled = thrusterEnergy < thrusterMinForMovement && !isThrusterActive;
        ThrustEnergyChanged?.Invoke(thrusterEnergy, isThrusterDisabled);
    }
}
