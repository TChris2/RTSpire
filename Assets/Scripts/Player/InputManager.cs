using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Gets inputs from player
public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    
    private PlayerMotor motor;
    private PlayerLook look;
    private PlayerAnimation animate;
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        animate = GetComponent<PlayerAnimation>();

        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Melee.performed += ctx => animate.Melee();
        onFoot.Throw.performed += ctx => animate.Throw();
        onFoot.CamZoomIn.performed += ctx => motor.ZoomIn();
        onFoot.CamZoomOut.performed += ctx => motor.ZoomOut();
    }

    void FixedUpdate()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
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
