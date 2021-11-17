using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WeaponRecoil : MonoBehaviour
{
    PlayerBrain playerBrain;
    CinemachineImpulseSource cameraShake;
    Camera mainCamera;
    public Animator rigController;

    public Vector2[] recoilPattern;

    private float time;
    public float duration;
    public float verticalRecoil;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerBrain = FindObjectOfType<PlayerBrain>();
        cameraShake = GetComponent<CinemachineImpulseSource>();
    }

    public void GenerateRecoil(string weaponName)
    {
        cameraShake.GenerateImpulse(mainCamera.transform.forward);
        time = duration;

        rigController.Play("weapon_recoil_" + weaponName, 1, 0.0f);
    }

    private void Update()
    {
        if(time > 0)
        {
            (playerBrain.yAxis.Value) -= ((verticalRecoil / 1000) * Time.deltaTime) / duration;
            time -= Time.deltaTime;
        }
    }
}
