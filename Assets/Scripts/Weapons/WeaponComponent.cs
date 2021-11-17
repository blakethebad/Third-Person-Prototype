using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    public Weapon thisWeapon;
    public WeaponRecoil weaponRecoil;
    public Transform raycastOrigin;
    public Transform raycastDestination;
    public GameObject magazine;

    public bool isFiring;

    Ray ray;
    RaycastHit hitInfo;
    float accumulatetTime = 0.0f;
    private List<Bullet> bullets = new List<Bullet>();

    private void Awake()
    {
        weaponRecoil = GetComponent<WeaponRecoil>();
    }

    public void StartFiring()
    {
        isFiring = true;
        accumulatetTime = 0.0f;
        FireBullet();
    }

    private void FireBullet()
    {
        if(thisWeapon.ammoCount <= 0)
        {
            return;
        }
        else
        {
            thisWeapon.ammoCount--;
        }

        thisWeapon.muzzleFlash.Emit(1);

        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * thisWeapon.bulletSpeed;
        var instantiatedBullet = Instantiate(thisWeapon.tracerEffect, raycastOrigin.position, Quaternion.identity);
        var bullet = thisWeapon.CreateBullet(raycastOrigin.position, velocity, instantiatedBullet);
        bullets.Add(bullet);

        weaponRecoil.GenerateRecoil(thisWeapon.weaponName);
    }


    public void UpdateFiring(float deltaTime)
    {
        accumulatetTime += deltaTime;
        float fireInterval = 1.0f / thisWeapon.fireRate;
        while (accumulatetTime >= 0.0f)
        {
            FireBullet();
            accumulatetTime -= fireInterval;
        }
    }

    public void UpdateBullets(float deltaTime)
    {
        thisWeapon.SimulateBullets(bullets,deltaTime,ray, hitInfo);
        thisWeapon.DestroyBullets(bullets);
    }

    public void StopFiring()
    {
        isFiring = false;
    }


}
