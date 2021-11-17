using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrain : MonoBehaviour
{

    public bool isPlayerMoving = false;
    public bool moveTest = false;
    public Vector2 movementInput;
    public Vector2 aimInput;

    public Cinemachine.AxisState yAxis;
    public Cinemachine.AxisState xAxis;

    public Transform cameraAimTransform;
    public bool isOnAim = false;
    public event Action<Vector3> OnPlayerAim;
    public bool isPlayerFiring = false;

    public event Action OnFiringStart;
    public event Action<Weapon.WeaponSlot> OnWeaponSwitch;

    public event Action OnReload;

    public bool isPlayerJumping = false;
    public event Action OnJump;

    public bool isPlayerSprinting = false;


    private void FixedUpdate()
    {
        AimCameraInput();
    }

    private void Update()
    {
        InputTesting();

        MovementInput();
        AimMovementInput();
        AimInput();
        FireInput();
        WeaponSwitchInput();
        JumpInput();
        CursorInput();
        ReloadInput();
        SprintInput();
    }

    private void InputTesting()
    {
        

    }

    private void MovementInput()
    {
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");


        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            isPlayerMoving = true;
        }
        else
        {
            isPlayerMoving = false;

        }

        Debug.Log("Move Test = " + moveTest);
    }

    private void AimMovementInput()
    {
        aimInput.x = Input.GetAxis("Horizontal");
        aimInput.y = Input.GetAxis("Vertical");
    }

    private void AimCameraInput()
    {

        if (Input.GetButton("Fire2"))
        {
            yAxis.Update(Time.deltaTime);
            xAxis.Update(Time.deltaTime);
        }
        if (Input.GetButtonUp("Fire2"))
        {
            xAxis.Value = 0;
            yAxis.Value = 0;
        }
        
    }

    public void AimInput()
    {
        if (Input.GetButtonDown("Fire2"))
        {
             Vector3 currentVector = cameraAimTransform.position;
             OnPlayerAim?.Invoke(currentVector);

            isOnAim = true;
        }
        if (Input.GetButtonUp("Fire2"))
        {
            isOnAim = false;
        }
    }

    public void AimInputWithKeyboard()
    {
        

        if (Input.GetKeyDown(KeyCode.K))
        {
            Vector3 currentAimVector = cameraAimTransform.position;
            OnPlayerAim?.Invoke(currentAimVector);
            isOnAim = true;
            
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            isOnAim = false;
        }
    }

    private void FireInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isPlayerFiring = true;
            OnFiringStart?.Invoke();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            isPlayerFiring = false;
        }
    }

    private void CursorInput()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void WeaponSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnWeaponSwitch.Invoke(Weapon.WeaponSlot.PrimaryWeapon);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnWeaponSwitch.Invoke(Weapon.WeaponSlot.PrimaryWeapon2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnWeaponSwitch.Invoke(Weapon.WeaponSlot.SecondaryWeapon);
        }
        
    }

    private void ReloadInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isOnAim)
            {
                return;
            }
            else
            {
                OnReload?.Invoke();
                
            }

            
        }


    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJump?.Invoke();
        }
    }

    private void SprintInput()
    {
        if (isPlayerMoving && !isOnAim)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isPlayerSprinting = true;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isPlayerSprinting = false;
            }
        }

        else
        {
            isPlayerSprinting = false;
        }
        
    }


    //private void CrouchInput();
    //private void SprintInput();


    public void SetJumpingBool(bool jumpState)
    {
        isPlayerJumping = jumpState;
    }
}
