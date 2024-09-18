using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public string weaponName;
    public float fireRate;
    public int ammoCapacity;
    public int currentAmmo;
    public float damage;
    public float range;
    public float projectileSpeed;

    public Transform gunBarrel;
    public GameObject projectilePrefab;

    protected bool canFire = true;

    public abstract void Shoot();
    public abstract void Reload();
}
