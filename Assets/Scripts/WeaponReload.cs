using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReload : MonoBehaviour
{
    public Animator rigController;
    public PlayerBrain playerBrain;
    public PlayerWeapon playerWeapon;
    public Transform rightHand;
    public Transform leftHand;
    public GameObject magOnHand;
    public WeaponAnimationEvents animationEvents;

    public bool isReloading = false;

    private void Start()
    {
        playerBrain = GetComponent<PlayerBrain>();
        playerWeapon = GetComponent<PlayerWeapon>();
        animationEvents.weaponAnimationEvent.AddListener(OnAnimationEvent);

        playerBrain.OnReload += Reload;
       
    }


    private void Update()
    {
        WeaponComponent currentWeapon = playerWeapon.GetActiveWeapon();

        if (currentWeapon)
        {
            if(currentWeapon.thisWeapon.ammoCount <= 0)
            {
                rigController.SetTrigger("weapon_reload");
                isReloading = true;
            }
        }
    }

    private void Reload()
    {
        WeaponComponent currentWeapon = playerWeapon.GetActiveWeapon();

        if (currentWeapon)
        {
            if (currentWeapon.thisWeapon.ammoCount != currentWeapon.thisWeapon.clipSize)
            {
                rigController.SetTrigger("weapon_reload");
                isReloading = true;
            }
            
        }
    }

    private void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "detach_mag":
                DetachMagazine();
                break;

            case "drop_mag":
                DropMagazine();
                break;

            case "refill_mag":
                RefillMagazine();
                break;

            case "attach_mag":
                AttachMagazine();
                break;
        }
            
    }


    void DetachMagazine()
    {
        WeaponComponent currentWeapon = playerWeapon.GetActiveWeapon();
        currentWeapon.magazine.SetActive(false);

        if (currentWeapon.thisWeapon.isReloadingWithRight)
        {
            magOnHand = Instantiate(currentWeapon.magazine, rightHand, true);
            magOnHand.SetActive(true);
        }
        else
        {
            magOnHand = Instantiate(currentWeapon.magazine, leftHand, true);
            magOnHand.SetActive(true);
        }
        
        
    }

    void DropMagazine()
    {
        GameObject droppedMagazine = Instantiate(magOnHand, magOnHand.transform.position, magOnHand.transform.rotation);

        droppedMagazine.AddComponent<Rigidbody>();
        droppedMagazine.GetComponent<BoxCollider>().enabled = true;

        magOnHand.SetActive(false);
    }

    void RefillMagazine()
    {
        magOnHand.SetActive(true);
    }

    void AttachMagazine()
    {
        WeaponComponent currentWeapon = playerWeapon.GetActiveWeapon();
        currentWeapon.magazine.SetActive(true);
        currentWeapon.thisWeapon.ammoCount = currentWeapon.thisWeapon.clipSize;
        rigController.ResetTrigger("weapon_reload");

        Destroy(magOnHand);

        isReloading = false;
    }
}
