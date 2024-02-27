using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    //private bool isGrounded;
    private Animator anim;
    bool crouching = false;
    float crouchTimer = 1;
    bool lerpCrouch = false;
    bool sprinting = false;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    private Vector2 currentInput;
    private bool isFalling;
    private float jumpTimeoutDuration = 0.35f; // Timeout duration in seconds
    private float timeSinceLastJump = 0;
    public bool bhop;
    void Start()
    {
        anim = gameObject.GetComponent<Animator> ();
        controller = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }

        isFalling = !controller.isGrounded && playerVelocity.y < 0;
        UpdateJumpPhase();
        if (controller.isGrounded)
        {
            timeSinceLastJump += Time.deltaTime; // Update the timer
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
            anim.SetBool("IsJumping", true); // Jumping up
        }
        if (isFalling)
        {
            anim.SetBool("IsFalling", true); // Falling
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
        // Normalize input to avoid faster diagonal movement
        input = input.normalized;

        // Set the animation parameters based on input
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
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -0.3f * gravity);
            timeSinceLastJump = 0;
        }
    }
    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
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
