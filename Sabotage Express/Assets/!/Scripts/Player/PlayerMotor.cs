using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMotor : NetworkBehaviour
{
    private CharacterController controller;
    private PlayerLook playerLook;
    private Camera cam;
    private Vector3 playerVelocity;
    //private bool isGrounded;
    private Animator anim;
    bool crouching = false;
    float crouchTimer = 1;
    bool lerpCrouch = false;
    bool sprinting = false;
    public float speed = 5f;
    public float croachSpeed = 2f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    private Vector2 currentInput;
    private bool isFalling;
    private float jumpTimeoutDuration = 0.35f;
    private float timeSinceLastJump = 0;
    public bool bhop;
    public float bhopSpeedMultiplier = 1.5f; 
    public float maxBhopSpeed = 10f; 
    private float normalSpeed;
    private bool isBhopping = false;
    public float transitionSpeed = 5f; 
    public float crouchCenterY = 0.5f; 
    public float standCenterY = 1f;
    
    public float crouchCameraYOffset = -0.5f; 
    public float standCameraYOffset = 0.0f; 

    
    [SerializeField] private Transform arms;

    [SerializeField] private Vector3 armPosition;
    void Start()
    {
        anim = gameObject.GetComponent<Animator> ();
        controller = GetComponent<CharacterController>();
        playerLook = GetComponent<PlayerLook>();
        if (playerLook != null)
        {
            cam = playerLook.cam;
        }
    }
    private Transform AssignCharactersCamera()
    {
        var t = transform;
        arms.SetPositionAndRotation(t.position, t.rotation);
        return arms;
    }
    public void SetArmsTransform(Transform armsTransform)
    {
        arms = armsTransform;
    }
    
    void Update()
    {
        //if (!IsOwner) return;
        
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / transitionSpeed;
            p = Mathf.Clamp01(p); 

            float targetHeight = crouching ? 1.5f : 2f;
            float targetCenterY = crouching ? crouchCenterY : standCenterY;

            controller.height = Mathf.Lerp(controller.height, targetHeight, p);
            Vector3 newCenter = controller.center;
            newCenter.y = Mathf.Lerp(newCenter.y, targetCenterY, p);
            controller.center = newCenter;
            
            if (cam != null)
            {
                Vector3 cameraPosition = cam.transform.localPosition;
                float targetYOffset = crouching ? crouchCameraYOffset : standCameraYOffset;
                cameraPosition.y = Mathf.Lerp(cameraPosition.y, targetYOffset, p);
                cam.transform.localPosition = cameraPosition;
            }

            if (p >= 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }

        isFalling = !controller.isGrounded && playerVelocity.y < 0;
        UpdateJumpPhase();
        if (controller.isGrounded)
        {
            timeSinceLastJump += Time.deltaTime; 
        }
        
        //isGrounded = controller.isGrounded;
    }
    void UpdateJumpPhase()
    {
        
        bool isMoving = currentInput.magnitude > 0.1f;
        if (isMoving)
        {
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
        if (controller.isGrounded)
        {
            anim.SetBool("IsFalling", false);
            anim.SetBool("IsJumping", false);
        }
        else if (playerVelocity.y > 0)
        {
            anim.SetBool("IsJumping", true); 
        }
        if (isFalling)
        {
            anim.SetBool("IsFalling", true); 
        }
        else
        {
            anim.SetBool("IsFalling", false);
        }

        if (controller.isGrounded)
        {
            anim.SetBool("IsGrounded", true);
        }
        else
        {
            anim.SetBool("IsGrounded", false);
        }
    }
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        currentInput = input;
        input = input.normalized;

        anim.SetFloat("Vertical", input.y, 0.1f, Time.deltaTime);
        anim.SetFloat("Horizontal", input.x, 0.1f, Time.deltaTime);

        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        if(controller.isGrounded && playerVelocity.y < 0) {
            playerVelocity.y = -2f;
        }
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        
    }
    public void Jump()
    {
        if (controller.isGrounded && !isFalling && timeSinceLastJump >= jumpTimeoutDuration || controller.isGrounded && !isFalling && bhop)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -0.2f * gravity);
            timeSinceLastJump = 0;
            if (bhop && !isBhopping) 
            {
                isBhopping = true;
                normalSpeed = speed;
                speed *= bhopSpeedMultiplier; 
            }
        }
        else if (!bhop && isBhopping) 
        {
            isBhopping = false;
            speed = normalSpeed; 
        }
    }
    public void Crouch()
    {
        crouching = !crouching;
        anim.SetBool("IsCrouching", crouching);
        crouchTimer = 0;
        lerpCrouch = true;
        if (crouching)
            speed = 2;
        else
        {
            speed = 5;
        }
    }
    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
            speed = 8;
        else
        {
            speed = 5;
        }
    }
}
