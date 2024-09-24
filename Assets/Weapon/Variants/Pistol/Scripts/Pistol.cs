using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public delegate void OnAmmoChanged(int currentAmmo, int maxAmmo);
    public static event OnAmmoChanged AmmoChanged;

    public PlayerMovement playerMovement;

    void Start()
    {
        weaponName = "Pistol";
        fireRate = 0.5f;
        ammoCapacity = 10;
        currentAmmo = ammoCapacity;
        damage = 10f;
        range = 100f;
        projectileSpeed = 20f;

        AmmoChanged?.Invoke(currentAmmo, ammoCapacity);
    }

    public override void Shoot() {
        if (currentAmmo > 0 && canFire && !playerMovement.inGameMenu.isVisible)
        {
            GameObject projectile = Instantiate(projectilePrefab, gunBarrel.position, transform.rotation);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = transform.forward * projectileSpeed;
            }

            currentAmmo--;
            canFire = false;
            Invoke("ResetShoot", fireRate);
            AmmoChanged?.Invoke(currentAmmo, ammoCapacity);
        }

        if (currentAmmo <= 0)
        {
            Reload();
        }
    }

    public override void Reload() {
        currentAmmo = ammoCapacity;
        AmmoChanged?.Invoke(currentAmmo, ammoCapacity);
    }

    private void ResetShoot()
    {
        canFire = true;
    }
}
