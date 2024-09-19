using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrusterMovement : MonoBehaviour
{
    public float thrusterForce = 10f;
    public float thrusterEnergyMax = 100f;
    public float thrusterRechargeRate = 10f;
    public float thrusterConsumptionRate = 10f;
    public TextMeshPro thrusterEnergyText;

    private Rigidbody rb;
    private Vector3 movementInput;
    private PlayerMovement playerMovement;
    private float verticalThrust;
    private float leftRotationThrust;
    private float rightRotationThrust;
    private float thrusterEnergy;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        thrusterEnergy = thrusterEnergyMax;
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
        if (thrusterEnergy > 0)
        {
            Vector3 thrusterDirection = transform.right * movementInput.x + transform.forward * movementInput.z;
            thrusterDirection += transform.up * verticalThrust;  // Add vertical thrust

            // Apply thruster force
            rb.AddForce(thrusterDirection.normalized * thrusterForce, ForceMode.Acceleration);

            // Apply rotation thrust
            rb.AddTorque(transform.forward * -(rightRotationThrust - leftRotationThrust) * thrusterForce, ForceMode.Acceleration);

            // Deplete thruster energy
            thrusterEnergy -= thrusterConsumptionRate * Time.deltaTime;
        }
        

        // Recharge thruster energy over time
        if (thrusterEnergy < thrusterEnergyMax)
        {
            thrusterEnergy += thrusterRechargeRate * Time.deltaTime;
        }

        UpdateThrustUI();
    }

    public void DisableThrust()
    {
        verticalThrust = 0f;
        movementInput = Vector3.zero;
    }

    public void UpdateThrustUI()
    {
        thrusterEnergyText.text = thrusterEnergy + "%";
    }
}
