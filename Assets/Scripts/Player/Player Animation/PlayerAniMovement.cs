using UnityEngine;
using System.Collections;

// Controls the player's movement animations
public class PlayerAniMovement : MonoBehaviour
{
    public bool isRunning;
    public bool isJumping;
    // Used in CupcakeMove
    public float prevInputX;
    // Used in CupcakeMove
    public float prevInputZ;
    private Animator PlayerAni;
    // Scripts
    private PlayerState pState;
    private PlayerAniThrow pAniThrow;
    private PlayerAniMelee pAniMelee;
    private PlayerMotor motor;
    private float idleTimer = 0f;
    public float idleDelay = 0.5f;
    public bool isIdle = false;

    void Start()
    {
        PlayerAni = GetComponent<Animator>();
        // Sets default direction
        prevInputX = 0;
        prevInputZ = 1;

        pState = GetComponentInChildren<PlayerState>();
        pAniThrow = GetComponent<PlayerAniThrow>();
        pAniMelee = GetComponent<PlayerAniMelee>();
        motor = GetComponent<PlayerMotor>();
    }

    void Update()
    {
        // Plays a movement animation if the player isn't attacking
        if (!pAniMelee.isMelee && !pAniThrow.isThrowing && !pState.isDead && !pState.isWin)
        {
            // Plays jump animation if the player is in the air
            if (!motor.isGrounded)
            {
                IdleReset();
                Jump();
            }
            // Grounded animations
            else
            {
                // If the player is idle
                if (motor.inputX == 0 && motor.inputZ == 0)
                {
                    idleTimer += Time.deltaTime;
                    if (idleTimer >= idleDelay && !isIdle)
                    {
                        Idle();
                        isIdle = true;
                    }
                }
                else
                {
                    IdleReset();
                    Run();
                }
            }
        }
    }

    // Resets idle timer
    void IdleReset()
    {
        idleTimer = 0f;
        isIdle = false;
    }
    
    // Jump animation
    public void Jump()
    {
        // Updates player direction if moving
        if (motor.inputX != 0 || motor.inputZ != 0)
        {
            prevInputX = motor.inputX;
            prevInputZ = motor.inputZ;
        }

        if (prevInputX < 0)
            PlayerAni.Play("JumpLeft");
        else if (prevInputX > 0)
            PlayerAni.Play("JumpRight");
        else if (prevInputZ == 1)
            PlayerAni.Play("JumpForward");
        else if (prevInputZ == -1)
            PlayerAni.Play("JumpBack");

    }

    // Run animation
    public void Run()
    {
        // Updates player direction if moving
        if (motor.inputX != 0 || motor.inputZ != 0)
        {
            prevInputX = motor.inputX;
            prevInputZ = motor.inputZ;
        }

        if (motor.inputX < 0)
            PlayerAni.Play("RunLeft");
        else if (motor.inputX > 0)
            PlayerAni.Play("RunRight");
        else if (motor.inputZ == 1)
            PlayerAni.Play("RunForward");
        else if (motor.inputZ == -1)
            PlayerAni.Play("RunBack");
    }

    // Idle animation
    public void Idle()
    {
        if (prevInputX < 0)
            PlayerAni.Play("StillLeft");
        else if (prevInputX > 0)
            PlayerAni.Play("StillRight");
        else if (prevInputZ == 1)
            PlayerAni.Play("StillForward");
        else if (prevInputZ == -1)
            PlayerAni.Play("StillBack");
    }
}