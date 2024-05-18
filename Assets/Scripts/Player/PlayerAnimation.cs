using UnityEngine;
using System.Collections;
// Controls the player's animations alongside attacks
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject cupcake;
    [SerializeField]
    private Transform player;

    // Throw Mats
    public Material[] ThrowLeft = new Material[8];
    public Material[] ThrowRight = new Material[8];
    public Material[] ThrowForward = new Material[8];
    public Material[] ThrowBack = new Material[8];
    public static bool isRunning;
    public static bool isJumping;
    public static bool isKicking;
    public static bool isThrowing;
    bool isDying;
    public static bool isWinning;
    // Stores direction of player
    public static float prevInputX;
    public static float prevInputZ;
    // Cupcake throw direction
    public static Vector3 cSpawnPos;
    public static Quaternion cRotate;
    private Animator PlayerAni;


    private void Start()
    {
        PlayerAni = gameObject.GetComponent<Animator>();
        prevInputX = 0;
        prevInputZ = 1;

        isDying = false;
        isWinning = false;
    }

    private void Update()
    {
        if (PlayerState.isDead || PlayerState.isWin)
        {
            /* Death Animation
            ---------------------------------------------- */
            if (PlayerState.isDead && !isDying) {
                isDying = true;
                PlayerAni.Play("Death");
            }

            /* Win Animation
            ---------------------------------------------- */
            if (PlayerState.isWin && !isWinning) {
                isWinning = true;
                PlayerAni.Play("Win");
            }
        }

        else if (!isKicking && !isThrowing)
        {   
            /* Jump Animation
            ---------------------------------------------- */
            if (!PlayerMotor.isGrounded) 
                Jump();

            /* Run Animation
            ---------------------------------------------- */
            else if (PlayerMotor.isGrounded && PlayerMotor.inputX != 0 || PlayerMotor.inputZ != 0)
                Run();

            /* Idle Animation
            ---------------------------------------------- */
            else if (PlayerMotor.isGrounded && PlayerMotor.inputX == 0 && PlayerMotor.inputZ == 0)
                Idle();//StartCoroutine(IdleCheck());
        }
    }

    /* Checks if player is idle
    ---------------------------------------------- */
    private IEnumerator IdleCheck()
    {
        float checkX = PlayerMotor.inputX;
        float checkZ = PlayerMotor.inputZ;

        yield return new WaitForSeconds(.3f);

        if (PlayerMotor.inputX == checkX && PlayerMotor.inputZ == checkZ)
            Idle();
    }

    /* Jump Animation
    ---------------------------------------------- */
    public void Jump()
    {   
        if (PlayerMotor.inputX != 0 || PlayerMotor.inputZ != 0)
        {
            prevInputX = PlayerMotor.inputX;
            prevInputZ = PlayerMotor.inputZ;

            // Left Jump and Left Diagonal Jump
            if (PlayerMotor.inputX < 0)
                PlayerAni.Play("JumpLeft");

            // Right Jump and Right Diagonal Jump
            else if (PlayerMotor.inputX > 0)
                PlayerAni.Play("JumpRight");
                
            // Forward Jump
            else if (PlayerMotor.inputZ == 1)
                PlayerAni.Play("JumpForward");

            // Back Jump
            else if (PlayerMotor.inputZ == -1)
                PlayerAni.Play("JumpBack");
        }
        else if (PlayerMotor.inputX == 0 && PlayerMotor.inputZ == 0)
        {
            // Left Jump and Left Diagonal Jump
            if (prevInputX < 0)
                PlayerAni.Play("JumpLeft");
            
            // Right Jump and Right Diagonal Jump
            else if (prevInputX > 0)
                PlayerAni.Play("JumpRight");

            // Forward Jump
            else if (prevInputZ == 1)
                PlayerAni.Play("JumpForward");

            // Back Jump
            else if (prevInputZ == -1)
                PlayerAni.Play("JumpBack");
        }
    }

    /* Run Animation
    ---------------------------------------------- */
    public void Run()
    {
        if (PlayerMotor.inputX != 0 || PlayerMotor.inputZ != 0)
        {
            prevInputX = PlayerMotor.inputX;
            prevInputZ = PlayerMotor.inputZ;
        }

        // Left and Left Diagonal
        if (PlayerMotor.inputX < 0)
            PlayerAni.Play("RunLeft");

        // Right and Right Diagonal
        else if (PlayerMotor.inputX > 0)
            PlayerAni.Play("RunRight");

        // Forward
        else if (PlayerMotor.inputZ == 1)
            PlayerAni.Play("RunForward");

        // Backwards
        else if (PlayerMotor.inputZ == -1)
            PlayerAni.Play("RunBack");
    }

    /* Idle Animation
    ---------------------------------------------- */
    public void Idle()
    {
        // Left Idle and Left Diagonal Idle
        if (prevInputX < 0)
            PlayerAni.Play("StillLeft");

        // Right Idle and Right Diagonal Idle    
        else if (prevInputX > 0)
            PlayerAni.Play("StillRight");

        // Forward Idle
        else if (prevInputZ == 1)
            PlayerAni.Play("StillForward");

        // Back Idle
        else if (prevInputZ == -1)
            PlayerAni.Play("StillBack");
    }
    
    /* Kick Animation
    ---------------------------------------------- */
    public void Kick()
    {
        if (!isKicking && !isThrowing && !PlayerState.isDead && !PlayerState.isWin)
        {
            isKicking = true;

            // Left and Left Diagonal
            if (prevInputX < 0)
                PlayerAni.Play("KickLeft");

            // Right and Right Diagonal
            else if (prevInputX > 0)
            {
                PlayerAni.Play("KickRight");
            }

            // Forward
            else if (prevInputZ == 1)
                PlayerAni.Play("KickForward");

            // Back
            else if (prevInputZ == -1)
                PlayerAni.Play("KickBack");

            StartCoroutine(AttackOff());
        }
    }

    /* Throw Animation
    ---------------------------------------------- */
    public void Throw()
    {   
        if (!isKicking && !isThrowing && !PlayerState.isDead && !PlayerState.isWin)
        {
            isThrowing = true;

            // Left and Left Diagonal
            if (prevInputX < 0)
                PlayerAni.Play("ThrowLeft");

            // Right and Right Diagonal
            else if (prevInputX > 0)
            {
                PlayerAni.Play("ThrowRight");
            }

            // Forward
            else if (prevInputZ == 1)
                PlayerAni.Play("ThrowForward");

            // Back
            else if (prevInputZ == -1)
                PlayerAni.Play("ThrowBack");

            StartCoroutine(AttackOff());
        }
    }

    /* Turns off attack trigger
    ---------------------------------------------- */
    private IEnumerator AttackOff()
    {
        yield return new WaitForSeconds(.817f);
        
        if (isKicking)
        {
            isKicking = false;
        }
        else if (isThrowing)
        {
            CupcakeSpawn();

            cRotate = Quaternion.Euler(0, PlayerLook.rotation.eulerAngles.y, 0);

            Instantiate(cupcake, player.position + cRotate * cSpawnPos, Quaternion.identity);

            isThrowing = false;
        }
    }

    /* Where Cupcake spawns
    ---------------------------------------------- */
    void CupcakeSpawn()
    {
        // Throw Left and Throw Left Diagonal
        if (prevInputX < 0)
        {   
            // Throw Left Diagonal
            if (prevInputX != -1)
            {   
                // Throw Left Forward Diagonal
                if (prevInputZ > 0)
                {
                    cSpawnPos.x = -10;
                    cSpawnPos.z = 6;
                }

                // Throw Left Back Diagonal
                else if (prevInputZ < 0)
                {
                    cSpawnPos.x = -10;
                    cSpawnPos.z = -6;
                }
            }
            // Throw Left Only
            else
            {
                cSpawnPos.x = -10;
                cSpawnPos.z = 0;
            }
        }

        // Throw Right and Throw Right Diagonal
        else if (prevInputX > 0)
        {   
            // Throw Right Diagonal
            if (prevInputX != 1)
            {   
                // Throw Right Forward Diagonal
                if (prevInputZ > 0)
                {
                    cSpawnPos.x = 10;
                    cSpawnPos.z = 6;
                }

                // Throw Right Back Diagonal
                else if (prevInputZ < 0)
                {
                    cSpawnPos.x = 10;
                    cSpawnPos.z = -6;
                }
            }
            // Throw Right Only
            else
            {
                cSpawnPos.x = 10;
                cSpawnPos.z = 0;
            }
        }

        // Throw Forward
        else if (prevInputZ == 1)
        {
            cSpawnPos.x = 0;
            cSpawnPos.z = 6;
        }

        // Throw Back
        else if (prevInputZ == -1)
        {
            cSpawnPos.x = 0;
            cSpawnPos.z = -6;
        }

        cSpawnPos.y = -.5f;
    }
}