using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI thrusterText;

    private void OnEnable()
    {
        Pistol.AmmoChanged += UpdateAmmoUI;
        PlayerThrusterMovement.ThrustEnergyChanged += UpdateThrusterUI;
    }

    private void OnDisable()
    {
        Pistol.AmmoChanged -= UpdateAmmoUI;
    }

    private void UpdateAmmoUI(int currentAmmo, int maxAmmo)
    {
        ammoText.text = currentAmmo + " / " + maxAmmo;
    }

    private void UpdateThrusterUI(float currentEnergy)
    {
        thrusterText.text = currentEnergy + "%";
    }
}
