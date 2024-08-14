using UnityEngine;
using System.Collections;

// Controls the player's movement animations
public class PlayerAniMovement : MonoBehaviour
{
    public static bool isRunning;
    public static bool isJumping;
    bool isDying;
    public static bool isWinning;
    public static float prevInputX;
    public static float prevInputZ;
    private Animator PlayerAni;
    private void Start()
    {
        PlayerAni = gameObject.GetComponent<Animator>();
        // Sets default direction
        prevInputX = 0;
        prevInputZ = 1;

        isDying = false;
        isWinning = false;
    }

    private void Update()
    {
        // If the player has died or won
        if (PlayerState.isDead || PlayerState.isWin)
        {
            if (PlayerState.isDead && !isDying)
            {
                isDying = true;
                PlayerAni.Play("Death");
            }

            if (PlayerState.isWin && !isWinning)
            {
                isWinning = true;
                PlayerAni.Play("Win");
            }
        }
        // Plays a movement animation if the player isn't attacking
        else if (!PlayerAniMelee.isMelee && !PlayerAniThrow.isThrowing)
        {
            // Plays jump animation if the player is in the air
            if (!PlayerMotor.isGrounded)
                Jump();
            // Plays running animation if grounded and moving
            else if (PlayerMotor.isGrounded && PlayerMotor.inputX != 0 || PlayerMotor.inputZ != 0)
                Run();
            // Plays idle animation if grounded an not moving
            else if (PlayerMotor.isGrounded && PlayerMotor.inputX == 0 && PlayerMotor.inputZ == 0)
                Idle();
        }
    }

    private IEnumerator IdleCheck()
    {
        float checkX = PlayerMotor.inputX;
        float checkZ = PlayerMotor.inputZ;

        yield return new WaitForSeconds(.3f);

        if (PlayerMotor.inputX == checkX && PlayerMotor.inputZ == checkZ)
            Idle();
    }

    // Jump animation
    public void Jump()
    {
        // Updates player direction if moving
        if (PlayerMotor.inputX != 0 || PlayerMotor.inputZ != 0)
        {
            prevInputX = PlayerMotor.inputX;
            prevInputZ = PlayerMotor.inputZ;
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
        if (PlayerMotor.inputX != 0 || PlayerMotor.inputZ != 0)
        {
            prevInputX = PlayerMotor.inputX;
            prevInputZ = PlayerMotor.inputZ;
        }

        if (PlayerMotor.inputX < 0)
            PlayerAni.Play("RunLeft");
        else if (PlayerMotor.inputX > 0)
            PlayerAni.Play("RunRight");
        else if (PlayerMotor.inputZ == 1)
            PlayerAni.Play("RunForward");
        else if (PlayerMotor.inputZ == -1)
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