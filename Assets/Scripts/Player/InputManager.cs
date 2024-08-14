using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Gets inputs from player
public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.PlayerActions player;
    
    private PlayerMotor motor;
    private PlayerLook look;
    private PlayerAniMelee aniMelee;
    private PlayerAniThrow aniThrow;
    private MenuPause pause; 
    void Awake()
    {
        playerInput = new PlayerInput();
        player = playerInput.Player;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        aniMelee = GetComponent<PlayerAniMelee>();
        aniThrow = GetComponent<PlayerAniThrow>();
        pause = GetComponent<MenuPause>();

        player.Jump.performed += ctx => motor.Jump();
        player.Melee.performed += ctx => aniMelee.Melee();
        player.Throw.performed += ctx => aniThrow.Throw();
        player.CamZoomIn.performed += ctx => motor.ZoomIn();
        player.CamZoomOut.performed += ctx => motor.ZoomOut();
    }

    void FixedUpdate()
    {
        if (!PlayerState.isDead && !PlayerState.isWin)   
            motor.ProcessMove(player.Movement.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        if (!PlayerState.isDead && !PlayerState.isWin)
            look.ProcessLook(player.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        player.Enable();
    }
    private void OnDisable()
    {
        player.Disable();
    }
}
