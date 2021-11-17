using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public WeaponComponent[] equippedWeapons = new WeaponComponent[3];
    public int currentWeaponIndex;

    public Transform crosshairTarget;
    public bool isEquiped;
    public Transform[] weaponTransforms;
    public Animator rigController;
    private PlayerAnimations playerAnimatons;
    private PlayerBrain playerBrain;
    private WeaponReload wepReload;
    public int holsterIndex;


    private void Start()
    {
        playerBrain = GetComponent<PlayerBrain>();
        playerAnimatons = GetComponent<PlayerAnimations>();
        wepReload = GetComponent<WeaponReload>();

        playerBrain.OnWeaponSwitch += SetActiveWeapon;
        playerBrain.OnFiringStart += ControlWeaponFiring;
    }


    private void Update()
    {
        if (CheckCurrentWeapon())
        {
            if (!wepReload.isReloading)
            {
                if (playerBrain.isOnAim && playerBrain.isPlayerFiring)
                {
                    GetActiveWeapon().UpdateFiring(Time.deltaTime);
                }
                else
                {

                    GetActiveWeapon().StopFiring();


                }
            }
            GetActiveWeapon().UpdateBullets(Time.deltaTime);

        }
    }

    public void ControlWeaponFiring()
    {

        if (!wepReload.isReloading)
        {
            if (CheckCurrentWeapon())
            {
                if (playerBrain.isOnAim)
                {
                    GetActiveWeapon().StartFiring();
                }
            }
        }
        
    }

    public void EquipGroundWeapon(WeaponComponent newWeapon)
    {
        int weaponSlotId = (int)newWeapon.thisWeapon.weaponSlot;
        var weapon = GetWeapon(weaponSlotId);

        if (weapon)
        {
            Destroy(weapon.gameObject);
        }

        newWeapon.weaponRecoil.rigController = rigController;

        weapon = newWeapon;
        weapon.raycastDestination = crosshairTarget;
        weapon.transform.parent = weaponTransforms[weaponSlotId];
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        equippedWeapons[weaponSlotId] = weapon;

        SetActiveWeapon(weapon.thisWeapon.weaponSlot);
        playerAnimatons.WeaponSwitchAnimation(weapon.thisWeapon.weaponSlot);

    }

    private void SetActiveWeapon(Weapon.WeaponSlot weaponSlot)
    {
        holsterIndex = currentWeaponIndex;
        int activateIndex = (int)weaponSlot;

        if (holsterIndex == activateIndex)
        {
            holsterIndex = -1;
        }

        currentWeaponIndex = activateIndex;


        
    }

    public bool CheckCurrentWeapon()
    {
        var currentWeapon = GetWeapon(currentWeaponIndex);

        if (currentWeapon)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public WeaponComponent GetActiveWeapon()
    {
        return GetWeapon(currentWeaponIndex);
    }

    public WeaponComponent GetWeapon(int index)
    {
        if (index < 0 || index >= equippedWeapons.Length)
        {
            return null;
        }

        return equippedWeapons[index];
    }


}
