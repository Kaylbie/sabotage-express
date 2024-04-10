using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : NetworkBehaviour
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
        onFoot.hotbar1.performed +=ctx => invManager.selectSlot(0);
        onFoot.hotbar2.performed +=ctx => invManager.selectSlot(1);
        onFoot.hotbar3.performed +=ctx => invManager.selectSlot(2);
        onFoot.hotbar4.performed +=ctx => invManager.selectSlot(3);
        onFoot.hotbar5.performed +=ctx => invManager.selectSlot(4);
        onFoot.hotbar6.performed +=ctx => invManager.selectSlot(5);

    }

    private void LateUpdate()
    {
        if (!IsOwner) return;
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsOwner) return;
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
