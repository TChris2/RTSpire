using UnityEngine;
using System.Collections;
// Controls the player's animations alongside attacks
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject cupcake;

    public static bool isRunning;
    public static bool isJumping;
    public static bool isMelee;
    public static bool isAtking;
    public static bool isThrowing;
    bool isDying;
    public static bool isWinning;
    public static float prevInputX;
    public static float prevInputZ;
    private Vector3 cSpawnPos;
    public static Quaternion cRotate;
    private Animator PlayerAni;

    float comboCount;
    float prevAtkInputX;
    float prevAtkInputZ;
    float atkCooldown;

    public float t1 = 1f;
    public float t2 = .25f;

    private Coroutine attackOffCoroutine;

    private CupcakeCounter cCount;
    private void Start()
    {
        PlayerAni = gameObject.GetComponent<Animator>();
        prevInputX = 0;
        prevInputZ = 1;

        isDying = false;
        isWinning = false;
        comboCount = 0;

        cCount = GameObject.Find("CupcakeCounter").GetComponent<CupcakeCounter>();
    }

    private void Update()
    {
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
        else if (!isMelee && !isThrowing)
        {
            if (!PlayerMotor.isGrounded)
                Jump();
            else if (PlayerMotor.isGrounded && PlayerMotor.inputX != 0 || PlayerMotor.inputZ != 0)
                Run();
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

    public void Jump()
    {
        if (PlayerMotor.inputX != 0 || PlayerMotor.inputZ != 0)
        {
            prevInputX = PlayerMotor.inputX;
            prevInputZ = PlayerMotor.inputZ;

            if (PlayerMotor.inputX < 0)
                PlayerAni.Play("JumpLeft");
            else if (PlayerMotor.inputX > 0)
                PlayerAni.Play("JumpRight");
            else if (PlayerMotor.inputZ == 1)
                PlayerAni.Play("JumpForward");
            else if (PlayerMotor.inputZ == -1)
                PlayerAni.Play("JumpBack");
        }
        else if (PlayerMotor.inputX == 0 && PlayerMotor.inputZ == 0)
        {
            if (prevInputX < 0)
                PlayerAni.Play("JumpLeft");
            else if (prevInputX > 0)
                PlayerAni.Play("JumpRight");
            else if (prevInputZ == 1)
                PlayerAni.Play("JumpForward");
            else if (prevInputZ == -1)
                PlayerAni.Play("JumpBack");
        }
    }

    public void Run()
    {
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

    public void Melee()
    {
        if (!isAtking && !isThrowing && !PlayerState.isDead && !PlayerState.isWin)
        {
            if (PlayerMotor.inputX != 0 || PlayerMotor.inputZ != 0)
            {
                prevInputX = PlayerMotor.inputX;
                prevInputZ = PlayerMotor.inputZ;

                if (prevAtkInputX != prevInputX || prevAtkInputZ != prevInputZ)
                {
                    comboCount = 0;
                }
            }
            isMelee = true;
            isAtking = true;

            prevAtkInputX = prevInputX;
            prevAtkInputZ = prevInputZ;

            if (PlayerMotor.isGrounded && comboCount < 2)
            {
                if (comboCount == 0)
                {
                    if (prevInputX < 0)
                        PlayerAni.Play("PunchLeftP1");
                    else if (prevInputX > 0)
                        PlayerAni.Play("PunchRightP1");
                    else if (prevInputZ == 1)
                        PlayerAni.Play("PunchForwardP1");
                    else if (prevInputZ == -1)
                        PlayerAni.Play("PunchBackP1");
                }
                else if (comboCount == 1)
                {
                    if (prevInputX < 0)
                        PlayerAni.Play("PunchLeftP2");
                    else if (prevInputX > 0)
                        PlayerAni.Play("PunchRightP2");
                    else if (prevInputZ == 1)
                        PlayerAni.Play("PunchForwardP2");
                    else if (prevInputZ == -1)
                        PlayerAni.Play("PunchBackP2");
                }

                comboCount += 1;
                atkCooldown = .48f;
            }
            else if (!PlayerMotor.isGrounded || comboCount == 2 && prevAtkInputX == prevInputX && prevAtkInputZ == prevInputZ)
            {
                if (prevInputX < 0)
                    PlayerAni.Play("KickLeft");
                else if (prevInputX > 0)
                    PlayerAni.Play("KickRight");
                else if (prevInputZ == 1)
                    PlayerAni.Play("KickForward");
                else if (prevInputZ == -1)
                    PlayerAni.Play("KickBack");

                atkCooldown = .817f;
                comboCount = 3;
            }

            if (attackOffCoroutine != null)
            {
                StopCoroutine(attackOffCoroutine);
            }
            attackOffCoroutine = StartCoroutine(AttackOff());
        }
    }

    private IEnumerator AttackOff()
    {
        yield return new WaitForSeconds(atkCooldown);

        if (isAtking)
        {
            if (comboCount <= 2 && comboCount != 0)
            {
                isAtking = false;
                float temp = comboCount;
                yield return new WaitForSeconds(t1);
                if (!isAtking && temp == comboCount)
                {
                    comboCount = 0;
                    isMelee = false;
                }
            }
            else if (comboCount == 3)
            {
                comboCount = 0;
                yield return new WaitForSeconds(t2);
                isMelee = false;
                isAtking = false;
            }
        }
        else if (isThrowing)
        {
            isThrowing = false;
        }

        attackOffCoroutine = null;
    }

    public void Throw()
    {
        if (!isMelee && !isThrowing && cCount.cupcakeCount-1 != -1 && !PlayerState.isDead && !PlayerState.isWin)
        {
            cCount.CounterDown();
            isThrowing = true;

            if (prevInputX < 0)
                PlayerAni.Play("ThrowLeft");
            else if (prevInputX > 0)
                PlayerAni.Play("ThrowRight");
            else if (prevInputZ == 1)
                PlayerAni.Play("ThrowForward");
            else if (prevInputZ == -1)
                PlayerAni.Play("ThrowBack");

            StartCoroutine(CupcakeSpawn());

            atkCooldown = .817f;
            if (attackOffCoroutine != null)
            {
                StopCoroutine(attackOffCoroutine);
            }
            attackOffCoroutine = StartCoroutine(AttackOff());
        }
    }

    /* Spawns A Cupcake
    ---------------------------------------------- */
    private IEnumerator CupcakeSpawn()
    {
        yield return new WaitForSeconds(0.595729166667f);

        CupcakeSpawnPos();

        cRotate = Quaternion.Euler(0, PlayerLook.rotation.eulerAngles.y, 0);

        Instantiate(cupcake, transform.position + cRotate * cSpawnPos, Quaternion.identity);
    }

    /* Where Cupcake spawns
    ---------------------------------------------- */
    void CupcakeSpawnPos()
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
                    cSpawnPos.x = -6;
                    cSpawnPos.z = 1;
                }

                // Throw Left Back Diagonal
                else if (prevInputZ < 0)
                {
                    cSpawnPos.x = -6;
                    cSpawnPos.z = -1;
                }
            }
            // Throw Left Only
            else
            {
                cSpawnPos.x = -6;
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
                    cSpawnPos.x = 6;
                    cSpawnPos.z = 1;
                }

                // Throw Right Back Diagonal
                else if (prevInputZ < 0)
                {
                    cSpawnPos.x = 6;
                    cSpawnPos.z = -1;
                }
            }
            // Throw Right Only
            else
            {
                cSpawnPos.x = 6;
                cSpawnPos.z = 0;
            }
        }

        // Throw Forward
        else if (prevInputZ == 1)
        {
            cSpawnPos.x = 0;
            cSpawnPos.z = 1;
        }

        // Throw Back
        else if (prevInputZ == -1)
        {
            cSpawnPos.x = 0;
            cSpawnPos.z = -1;
        }

        cSpawnPos.y = -.5f;
    }
}