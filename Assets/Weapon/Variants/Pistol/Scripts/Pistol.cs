using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    void Start()
    {
        weaponName = "Pistol";
        fireRate = 0.5f;
        ammoCapacity = 10;
        currentAmmo = ammoCapacity;
        damage = 10f;
        range = 100f;
        projectileSpeed = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Shoot() {
        if (currentAmmo > 0 && canFire)
        {
            GameObject projectile = Instantiate(projectilePrefab, gunBarrel.position, transform.rotation);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = transform.forward * projectileSpeed;
            }

            //RaycastHit hit;
            //Vector3 rayOrigin = transform.position;
            //Vector3 rayDirection = transform.forward;

            //if (Physics.Raycast(transform.position, transform.forward, out hit, range))
            //{
            //    Debug.Log("Hit: " + hit.transform.name);
            //}
            currentAmmo--;
            canFire = false;
            Invoke("ResetShoot", fireRate);
        }

        if (currentAmmo <= 0)
        {
            Reload();
        }
    }

    public override void Reload() {
        currentAmmo = ammoCapacity;
    }

    private void ResetShoot()
    {
        canFire = true;
    }
}
