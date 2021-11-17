using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimations : MonoBehaviour
{
    public Animator anim;
    public Animator rigController;
    public Rig aimBodyRig;

    private PlayerBrain playerBrain;
    private PlayerWeapon playerWeapon;

    private void Start()
    {
        playerWeapon = GetComponent<PlayerWeapon>();
        playerBrain = GetComponent<PlayerBrain>();
        playerBrain.OnWeaponSwitch += WeaponSwitchAnimation;
    }

    private void Update()
    {
        rigController.SetBool("isHolstering", !playerBrain.isOnAim);

        SetAimState(playerBrain.isOnAim);
        SetJumpState(playerBrain.isPlayerJumping);
        SetSprintState(playerBrain.isPlayerSprinting);

        AimWeaponAnimation();

        if (playerBrain.isOnAim)
        {
            AimMovementAnimation(playerBrain.aimInput);
        }
        else
        {
            NormalMovementAnimation(playerBrain.movementInput);
        }
    }

    public void NormalMovementAnimation(Vector2 input)
    {
        float speed;
        speed = Mathf.Abs(input.x) + Mathf.Abs(input.y);
        speed = Mathf.Clamp(speed, 0f, 1f);

        anim.SetFloat("Speed", speed);

        anim.SetBool("isMoving", playerBrain.isPlayerMoving);
        
    }


    public void AimMovementAnimation(Vector2 aimInput)
    {
        anim.SetFloat("InputX", aimInput.x);
        anim.SetFloat("InputY", aimInput.y);
    }

    public void AimWeaponAnimation()
    {
        if (playerBrain.isOnAim)
        {
            aimBodyRig.weight = 1;
        }
        else
        {
            aimBodyRig.weight = 0;
        }
    }

    public void SetAimState(bool aimState)
    {
        anim.SetBool("isAiming", aimState);
    }

    public void SetJumpState(bool jumpState)
    {
        anim.SetBool("isJumping", jumpState);
    }

    public void SetSprintState(bool sprintState)
    {
        anim.SetBool("isSprinting", sprintState);
    }


    public void WeaponSwitchAnimation(Weapon.WeaponSlot weaponSlot)
    {
        if (playerBrain.isOnAim)
        {
            int activateIndex = (int)weaponSlot;

            StartCoroutine(SwitchWeapon(playerWeapon.holsterIndex, activateIndex));
        }
        if (!playerBrain.isOnAim)
        {
            Debug.Log("Weapon Switched");
        }
        
    }

    IEnumerator SwitchWeapon(int holsterIndex, int activateIndex)
    {
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activateIndex));
    }


    IEnumerator HolsterWeapon(int index)
    {
        var weapon = playerWeapon.GetWeapon(index);
        if (weapon)
        {
            rigController.SetBool("isHolstering", true);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }

    IEnumerator ActivateWeapon(int index)
    {
        var weapon = playerWeapon.GetWeapon(index);
        if (weapon)
        {
            rigController.SetBool("isHolstering", false);
            rigController.Play("equip_" + weapon.thisWeapon.weaponName);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }

}
