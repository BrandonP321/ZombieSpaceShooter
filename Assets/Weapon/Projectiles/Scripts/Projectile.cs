using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 270f;
    public float damage = 10f;
    public float lifetime = 5f;

    private Rigidbody rb;

    private void Start()
    {
        // Destroy the projectile after a certain amount of time
        Destroy(gameObject, lifetime);

        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit: " + collision.gameObject.name);
        Destroy(gameObject);
    }
}
