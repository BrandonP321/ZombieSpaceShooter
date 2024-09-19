using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGravityZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.EnterZeroGravity();
            }
        }
        else if (other.GetComponent<Rigidbody>() != null)
        {
            other.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.ExitZeroGravity();
            }
        }
    }
}
