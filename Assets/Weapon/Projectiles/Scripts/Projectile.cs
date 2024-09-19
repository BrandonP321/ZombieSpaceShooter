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

    //private void Update()
    //{
    //    transform.Translate(Vector3.forward * speed * Time.deltaTime);
    //}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit: " + other.name);
        Destroy(gameObject);
    }
}
