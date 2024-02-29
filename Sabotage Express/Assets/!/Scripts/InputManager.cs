using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    
    private PlayerMotor motor;
    private InventoryManager invManager;
    private PlayerLook look;
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        motor=GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        invManager = GetComponent<InventoryManager>();
        
        onFoot.Jump.performed += ctx => motor.Jump();
        
        onFoot.Crouch.performed +=ctx => motor.Crouch();
        onFoot.Sprint.performed +=ctx => motor.Sprint();

    }
    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }
    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
}
