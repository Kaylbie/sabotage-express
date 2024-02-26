using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private Animator anim;
    bool crouching = false;
    float crouchTimer = 1;
    bool lerpCrouch = false;
    bool sprinting = false;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    
    
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
        if (!isGrounded && playerVelocity.y > 0)
        {
            anim.SetFloat("JumpPhase", 0.5f); // Assuming 0.5 represents the jumping phase
        }
        // Check if the player is falling
        else if (!isGrounded && playerVelocity.y <= 0)
        {
            anim.SetFloat("JumpPhase", 0.75f); // Assuming 0.75 represents the falling phase
        }
        // Check if the player has landed
        else if (isGrounded && playerVelocity.y <= 0)
        {
            anim.SetFloat("JumpPhase", 1f); // Assuming 1 represents the landing phase
        }
        if (isGrounded && anim.GetBool("IsJumping"))
        {
            
            anim.SetBool("IsJumping", false);
        }

        if (!isGrounded)
        {
            anim.SetFloat("JumpPhase", 0.75f);
        }
        isGrounded = controller.isGrounded;
    }
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        // Normalize input to avoid faster diagonal movement
        input = input.normalized;

        // Set the animation parameters based on input
        anim.SetFloat("Vertical", input.y, 0.1f, Time.deltaTime);
        anim.SetFloat("Horizontal", input.x, 0.1f, Time.deltaTime);

        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        if(isGrounded && playerVelocity.y < 0) {
            playerVelocity.y = -2f;
        }
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        
    }
    public void Jump()
    {
        if (isGrounded)
        {
            anim.SetBool("IsJumping", true);
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -0.3f * gravity);
            isGrounded = false;
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
