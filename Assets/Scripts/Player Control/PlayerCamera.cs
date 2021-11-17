using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class PlayerCamera : MonoBehaviour
{
    public CinemachineVirtualCamera aimCamera;
    public CinemachineVirtualCamera normalCamera;
    public CinemachineVirtualCamera sprintCamera;
    public Camera mainCamera;
    public Transform cameraImpulseSource;

    private PlayerBrain playerBrain;

    public CinemachineImpulseSource sprintCameraShake;
    private float shakeTimer;

    private CinemachineComponentBase component;


    public bool isCameraLocked = false;


    private void Start()
    {

        playerBrain = GetComponent<PlayerBrain>();
        mainCamera = Camera.main;
    }


    private void FixedUpdate()
    {

        if (playerBrain.isPlayerSprinting)
        {
            OpenSprintCamera();
        }
    }

    private void Update()
    {
        if (playerBrain.isOnAim)
        {
            SwitchToAimCamera();
        }
        else
        {
            SwitchToNormalCamera();
        }

        if (playerBrain.isPlayerSprinting)
        {
            OpenSprintCamera();
        }
        else
        {
            CloseSprintcamera();
        }

    }

    private void OpenSprintCamera()
    {
        sprintCamera.Priority = 11;
    }

    private void CloseSprintcamera()
    {
        sprintCamera.Priority = 2;
    }

    public void SwitchToNormalCamera()
    {
        aimCamera.Priority = 1;
        
    }

    public void SwitchToAimCamera()
    {
        aimCamera.Priority = 20;
    }
}
