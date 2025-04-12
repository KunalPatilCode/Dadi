using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsSwitch : MonoBehaviour
{
    public GameObject[] weapons; // Add your weapons in the Inspector
    private int currentWeaponIndex = 0;

    void Start()
    {
        // Disable all weapons initially
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }

        // Activate the first weapon by default if available
        if (weapons.Length > 0)
        {
            weapons[currentWeaponIndex].SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Deactivate current weapon
            weapons[currentWeaponIndex].SetActive(false);

            // Increment index and loop back to 0 if necessary
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;

            // Activate the new weapon
            weapons[currentWeaponIndex].SetActive(true);
        }
    }
}
