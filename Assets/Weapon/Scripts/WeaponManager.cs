using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    public Weapon weapon;
    public TextMeshProUGUI ammoText;

    private void Start()
    {
        UpdateAmmoUI();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            weapon.Shoot();
            UpdateAmmoUI();
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            weapon.Reload();
            UpdateAmmoUI();
        }
    }

    public void UpdateAmmoUI()
    {
        ammoText.text = weapon.currentAmmo + " / " + weapon.ammoCapacity;
    }
}
