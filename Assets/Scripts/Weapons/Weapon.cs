using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon 
{
    public enum WeaponSlot
    {
        PrimaryWeapon = 0,
        PrimaryWeapon2 = 1,
        SecondaryWeapon = 2
    };
    public string weaponName;
    public WeaponSlot weaponSlot;
    public int fireRate = 25;
    public float bulletSpeed = 1000f;
    public float bulletDrop = 0.0f;

    public float bulletLifeTime;

    public bool isReloadingWithRight;
    public int ammoCount;
    public int clipSize;

    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    


    public Vector3 GetBulletPosition(Bullet bullet)
    {
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
        
    }

    public Bullet CreateBullet(Vector3 position, Vector3 velocity, TrailRenderer bulletTracer)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = bulletTracer;
        bullet.tracer.AddPosition(position);
        return bullet;
    }

    public void SimulateBullets(List<Bullet> bullets, float deltaTime, Ray ray, RaycastHit hitInfo)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetBulletPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetBulletPosition(bullet);
            RaycastBullets(p0, p1, bullet, ray, hitInfo);
        });
    }

    void RaycastBullets(Vector3 start, Vector3 end, Bullet bullet, Ray ray, RaycastHit hitInfo)
    {

        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;


        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = bulletLifeTime;

        }
        else
        {
            bullet.tracer.transform.position = end;
        }
    }

    public void DestroyBullets(List<Bullet> bullets)
    {
        bullets.RemoveAll(bullet => bullet.time >= bulletLifeTime);
    }

}


public class Bullet
{
    public float time;
    public Vector3 initialPosition;
    public Vector3 initialVelocity;
    public TrailRenderer tracer;
}
