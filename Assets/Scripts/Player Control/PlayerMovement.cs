using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Camera mainCamera;
    private PlayerCamera playerCamera;
    private PlayerBrain playerBrain;
    private Animator animator;
    private CharacterController controller;
    public Transform aimDirection;

    private Vector3 playerAimVector;

    private Vector3 targetDirection;
    private Quaternion targetRotation;
    private Vector3 rootMotion;
    public Vector3 velocity;

    private float turnSpeed = 10;

    public float jumpHeight;
    public float gravity;
    public float stepDown;
    public float airControl;
    public float jumpDamp;
    private float groundSpeed;
    public float moveSpeed;
    public float sprintSpeed;
    public bool isJumping;

    private void Start()
    {
        mainCamera = Camera.main;
        playerBrain = GetComponent<PlayerBrain>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        playerBrain.OnJump += Jump;
        playerBrain.OnPlayerAim += OnPlayerAim;
    }

    private void FixedUpdate()
    {
        Debug.Log(targetDirection);
        Debug.Log(targetRotation);

        playerBrain.SetJumpingBool(isJumping);

        if (!playerBrain.isOnAim)
        {
            HandleNormalDirection(playerBrain.movementInput);
        }
        if (playerBrain.isOnAim)
        {
            HandleAimDirection();

        }
        

        if (isJumping)
        {
            MovementInAir();
            
        }
        else 
        {
            MovementOnGround();
            
        }

        if (playerBrain.isPlayerSprinting)
        {
            Sprint();
        }
        else
        {
            groundSpeed = moveSpeed;
        }


    }

    public void HandleNormalDirection(Vector2 input)
    {
        var forward = mainCamera.transform.TransformDirection(Vector3.forward);
        forward.y = 0;

        var right = mainCamera.transform.TransformDirection(Vector3.right);
        targetDirection = input.x * right + input.y * forward;

        if (input != Vector2.zero && targetDirection.magnitude > 0.1f)
        {
            Vector3 lookDirection = targetDirection.normalized;
            targetRotation = Quaternion.LookRotation(lookDirection, transform.up);
            var newRotation = targetRotation.eulerAngles.y - transform.eulerAngles.y;
            var eulerY = transform.eulerAngles.y;

            if (newRotation < 0 || newRotation > 0)
            {
                eulerY = targetRotation.eulerAngles.y;
            }

            var euler = new Vector3(0, eulerY, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), turnSpeed * Time.deltaTime);

        }


    }

    public void HandleAimDirection()
    {
        transform.eulerAngles = playerAimVector + new Vector3(0, playerBrain.xAxis.Value, 0);
    }

    private void OnPlayerAim(Vector3 aimVector)
    {
        var lookDir = aimVector - transform.position;

        lookDir.y = 0;

        transform.rotation = Quaternion.LookRotation(lookDir);
        //transform.forward = lookDir;


        playerAimVector = transform.eulerAngles;

    }

    private void MovementOnGround()
    {
        Vector3 stepForwardAmount = rootMotion * groundSpeed;
        Vector3 stepDownAmount = Vector3.down * stepDown;
        controller.Move(stepForwardAmount + stepDownAmount);
        rootMotion = Vector3.zero;


        if (!controller.isGrounded)
        {
            SetInAir(0);

        }
    }

    private void MovementInAir()
    {
        velocity.y -= gravity * Time.fixedDeltaTime;
        Vector3 displacement = velocity * Time.fixedDeltaTime;
        displacement += GetAirControl();

        controller.Move(displacement);
        isJumping = !controller.isGrounded;
        rootMotion = Vector3.zero;
    }

    public void Jump()
    {
        if (!isJumping)
        {
            float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
            SetInAir(jumpVelocity);

        }
    }

    public void Sprint()
    {
        groundSpeed = sprintSpeed;
    }

    private void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = animator.velocity * jumpDamp * groundSpeed;

        velocity.y = jumpVelocity;
    }

    public Vector3 GetAirControl()
    {
        var forward = mainCamera.transform.TransformDirection(Vector3.forward);
        forward.y = 0;

        var right = mainCamera.transform.TransformDirection(Vector3.right);

        Vector3 cameraForward = ((forward * playerBrain.movementInput.y) + (right * playerBrain.movementInput.x)) * (airControl / 100);
        Vector3 playerForward = ((transform.forward * playerBrain.movementInput.y) + (transform.right * playerBrain.movementInput.x)) * (airControl / 100);
        return cameraForward;
    }


    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
    }
}
