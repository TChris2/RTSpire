using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Gets inputs from the player
public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.PlayerActions player;
    public PlayerInput.MenuActions menu;
    
    private PlayerMotor motor;
    private PlayerLook look;
    private PlayerAniMelee aniMelee;
    private PlayerAniThrow aniThrow;
    private MenuPause pause; 
    private PlayerState pState;
    
    //Maybe change to start
    void Awake()
    {
        playerInput = new PlayerInput();
        player = playerInput.Player;
        menu = playerInput.Menu;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        aniMelee = GetComponent<PlayerAniMelee>();
        aniThrow = GetComponent<PlayerAniThrow>();
        pause = GetComponent<MenuPause>();

        player.Jump.performed += ctx => motor.Jump();
        player.Melee.performed += ctx => aniMelee.Melee();
        player.Throw.performed += ctx => aniThrow.Throw();
        player.CamZoomIn.performed += ctx => look.StartZoomIn();
        player.CamZoomIn.canceled += ctx => look.StopZoomIn();
        player.CamZoomOut.performed += ctx => look.StartZoomOut();
        player.CamZoomOut.canceled += ctx => look.StopZoomOut();
        player.Pause.performed += ctx => pause.Pause();
        menu.Resume.performed += ctx => pause.Resume();

        pState = GetComponentInChildren<PlayerState>();
    }

    void FixedUpdate()
    {
        if (!pState.isDead && !pState.isWin)   
            motor.ProcessMove(player.Movement.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        if (!pState.isDead && !pState.isWin)
        {
            look.ProcessLook(player.Look.ReadValue<Vector2>());
        
            if (look.isZoomIn)
                look.ZoomIn();
            if (look.isZoomOut)
                look.ZoomOut();
        }
    }

    private void OnEnable()
    {
        playerInput.Enable();
        menu.Disable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }
}
