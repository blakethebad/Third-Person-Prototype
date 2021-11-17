using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponComponent weaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        PlayerWeapon playerWeapon = other.gameObject.GetComponent<PlayerWeapon>();

        if (playerWeapon)
        {
            WeaponComponent newWeapon = Instantiate(weaponPrefab);
            playerWeapon.EquipGroundWeapon(newWeapon);
            Destroy(gameObject);
        }
    }
}
