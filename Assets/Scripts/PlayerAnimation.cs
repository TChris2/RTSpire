using UnityEngine;
using System.Collections;
// update cupcake throw for and back with Cupcake Text in davinci
// add hit boxes
// add cupcake throw
// Add SFX to jump, kick, and throw
// health
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject cupcake;
    [SerializeField]
    private Transform player;

    // Still Mats
    public Material stillForwardMat;
    public Material stillBackwardMat;
    public Material stillLeftMat;
    public Material stillRightMat;
    // Kick Mats
    public Material[] KickLeft = new Material[8];
    public Material[] KickRight = new Material[8];
    public Material[] KickForward = new Material[8];
    public Material[] KickBack = new Material[8];
    // Throw Mats
    public Material[] ThrowLeft = new Material[8];
    public Material[] ThrowRight = new Material[8];
    public Material[] ThrowForward = new Material[8];
    public Material[] ThrowBack = new Material[8];
    // Jump Mats
    public Material[] JumpStill = new Material[4];
    // Run Mats
    public Material[] runLeftMats = new Material[7];
    public Material[] runRightMats = new Material[7];
    public Material[] runForwardMats = new Material[7];
    public Material[] runBackMats = new Material[7];
    // Win Mat
    public Material[] winMats;
    // Win Mat
    public Material[] deathMats;
    
    [SerializeField]
    private Transform pback;
    [SerializeField]
    private Renderer planeRenderer;
    private int frameCounter;
    [SerializeField]
    private int frameDelay = 80;
    private bool isRunning;
    public static bool isJumping;
    public static bool isKicking;
    public static bool isThrowing;
    bool isDying;
    public static float prevInputX = 0;
    public static float prevInputZ = 1;
    // Cupcake Throw Direction
    public static Vector3 cSpawnPos = Vector3.zero;
    public static Quaternion cRotate;
    // Hitbox
    [SerializeField]
    private GameObject KickForAttack;
    [SerializeField]
    private GameObject KickBackAttack;
    [SerializeField]
    private GameObject KickLeftAttack;
    [SerializeField]
    private GameObject KickRightAttack;


    private void Start()
    {
        isDying = false;
        planeRenderer.material = stillForwardMat;

        KickLeftAttack.SetActive(false);
        KickRightAttack.SetActive(false);
        KickForAttack.SetActive(false);
        KickBackAttack.SetActive(false);
        
    }

    private void Update()
    {
        /* Death Animation
        ---------------------------------------------- */
        if (PlayerState.isDead && !isDying) {
            isDying = true;
            StartCoroutine(DeathAnimation());
        }

        /* Kicking Animation
        ---------------------------------------------- */
        else if (PlayerMotor.kickOn && !isKicking && !isThrowing && !isDying)
        {   
            // Prevents other animations from playing
            isKicking = true;
            // Kick Left and Kick Left Diagonal
            if (prevInputX < 0)
                StartCoroutine(KickAnimation(KickLeft));

            // Kick Right and Kick Right Diagonal
            else if (prevInputX > 0)
                StartCoroutine(KickAnimation(KickRight));

            // Kick Forward
            else if (prevInputZ == 1)
                StartCoroutine(KickAnimation(KickForward));

            // Kick Back
            else if (prevInputZ == -1)
                StartCoroutine(KickAnimation(KickBack));

        }   

        /* Throwing Animation
        ---------------------------------------------- */
        else if (PlayerMotor.throwOn && !isKicking && !isThrowing && !isDying)
        {   
            // Prevents other animations from playing
            isThrowing = true;
            // Throw Left and Throw Left Diagonal
            if (prevInputX < 0)
                StartCoroutine(ThrowAnimation(ThrowLeft));

            // Throw Right and Throw Right Diagonal
            else if (prevInputX > 0)
                StartCoroutine(ThrowAnimation(ThrowRight));

            // Throw Forward
            else if (prevInputZ == 1)
                StartCoroutine(ThrowAnimation(ThrowForward));

            // Throw Back
            else if (prevInputZ == -1)
                StartCoroutine(ThrowAnimation(ThrowBack));

        } 

        /* Jump Animation
        ---------------------------------------------- */
        else if (!PlayerMotor.isGrounded && !isJumping && !isKicking && !isThrowing && !isDying)
        {   
            StartCoroutine(Jump());
        }

        /* Idle Animation
        ---------------------------------------------- */
        else if (PlayerMotor.inputX == 0 && PlayerMotor.inputZ == 0 && !isJumping && !isKicking && !isThrowing && !isDying)
        {   
            // Left Idle and Left Diagonal Idle
            if (prevInputX < 0)
                planeRenderer.material = stillLeftMat;

            // Right Idle and Right Diagonal Idle    
            else if (prevInputX > 0)
                planeRenderer.material = stillRightMat;

            // Forward Idle
            else if (prevInputZ == 1)
                planeRenderer.material = stillForwardMat;

            // Back Idle
            else if (prevInputZ == -1)
                planeRenderer.material = stillBackwardMat;
        }

        /* Running Animation
        ---------------------------------------------- */
        else if (!isRunning && !isJumping && !isKicking && !isThrowing && !isDying)
        {   
            // Left and Left Diagonal
            if (PlayerMotor.inputX < 0)
                StartCoroutine(RunAnimation(runLeftMats));

            // Right and Right Diagonal
            else if (PlayerMotor.inputX > 0)
                StartCoroutine(RunAnimation(runRightMats));

            // Forward
            else if (PlayerMotor.inputZ == 1)
                StartCoroutine(RunAnimation(runForwardMats));

            // Backwards
            else if (PlayerMotor.inputZ == -1)
                StartCoroutine(RunAnimation(runBackMats));
        }
    }

    /* Run Animation
    ---------------------------------------------- */
    private IEnumerator RunAnimation(Material[] runMats)
    {
        isRunning = true;
        int currentRunIndex = 0;
        planeRenderer.material = runMats[currentRunIndex];
        float frameTimer = 0f;

        if (PlayerMotor.inputX != 0 || PlayerMotor.inputZ != 0)
        {
            prevInputX = PlayerMotor.inputX;
            prevInputZ = PlayerMotor.inputZ;
        }

        while (true)
        {
            frameTimer += Time.deltaTime;
            // If the player jumps or attacks
            if (!PlayerMotor.isGrounded || isKicking || isThrowing || isDying)
            {
                break;
            }
            else if (frameTimer >= frameDelay / 1000f)
            {
                frameTimer = 0f;
                currentRunIndex = (currentRunIndex + 1) % runMats.Length;
                planeRenderer.material = runMats[currentRunIndex];
            }
            // If player stops running
            if (PlayerMotor.inputX != prevInputX || PlayerMotor.inputZ != prevInputZ)
            {
                break;
            }

            yield return null;
        }

        isRunning = false;
    }

    /* Jump Animation
    ---------------------------------------------- */
    private IEnumerator Jump()
    {   
        isJumping = true;

        while (PlayerMotor.isGrounded == false)
        {
            if (isKicking || isThrowing || isDying)
            {
                break;
            }
            else if (PlayerMotor.inputX != 0 || PlayerMotor.inputZ != 0)
            {
                prevInputX = PlayerMotor.inputX;
                prevInputZ = PlayerMotor.inputZ;

                // Left Jump and Left Diagonal Jump
                if (PlayerMotor.inputX < 0)
                    planeRenderer.material = JumpStill[0];

                // Right Jump and Right Diagonal Jump
                else if (PlayerMotor.inputX > 0)
                    planeRenderer.material = JumpStill[1];
                
                // Forward Jump
                else if (PlayerMotor.inputZ == 1)
                    planeRenderer.material = JumpStill[2];

                // Back Jump
                else if (PlayerMotor.inputZ == -1)
                    planeRenderer.material = JumpStill[3];
            }
            else if (PlayerMotor.inputX == 0 && PlayerMotor.inputZ == 0)
            {
                // Left Jump and Left Diagonal Jump
                if (prevInputX < 0)
                    planeRenderer.material = JumpStill[0];
                
                // Right Jump and Right Diagonal Jump
                else if (prevInputX > 0)
                    planeRenderer.material = JumpStill[1];

                // Forward Jump
                else if (prevInputZ == 1)
                    planeRenderer.material = JumpStill[2];

                // Back Jump
                else if (prevInputZ == -1)
                    planeRenderer.material = JumpStill[3];
            }

            yield return null;
        }
        
        isJumping = false;
    }
    
    /* Kick Animation
    ---------------------------------------------- */
    private IEnumerator KickAnimation(Material[] kickMats)
    {
        int currentKickIndex = 0;
        planeRenderer.material = kickMats[currentKickIndex];
        float frameTimer = 0f;

        if (PlayerMotor.inputX != 0 || PlayerMotor.inputZ != 0)
        {
            prevInputX = PlayerMotor.inputX;
            prevInputZ = PlayerMotor.inputZ;
        }

        while (currentKickIndex != 7)
        {
            if (isDying)
            {
                break;
            }
            frameTimer += Time.deltaTime;

            if (frameTimer >= frameDelay / 1000f) 
            {
                frameTimer = 0f;
                currentKickIndex = (currentKickIndex + 1) % kickMats.Length;
                planeRenderer.material = kickMats[currentKickIndex];
            }

            if (currentKickIndex == 2) {
                KickOn();
            }

            yield return null;
        }
        KickOff();
        isKicking = false;
        PlayerMotor.kickOn = false;
    }

    /* Enables kicking hitbox
    ---------------------------------------------- */
    void KickOn()
    {
        // Kick Left
        if (prevInputX < 0)
        {   
            KickLeftAttack.SetActive(true);
        }

        // Throw Right 
        else if (prevInputX > 0)
        {   
            KickRightAttack.SetActive(true);
        }

        // Throw Forward
        else if (prevInputZ == 1)
        {
            KickForAttack.SetActive(true);
        }

        // Throw Back
        else if (prevInputZ == -1)
        {
            KickBackAttack.SetActive(true);
        }
    }

    /* Disables kicking hitbox
    ---------------------------------------------- */
    void KickOff()
    {
        // Kick Left
        if (prevInputX < 0)
        {   
            KickLeftAttack.SetActive(false);
        }

        // Throw Right 
        else if (prevInputX > 0)
        {   
            KickRightAttack.SetActive(false);
        }

        // Throw Forward
        else if (prevInputZ == 1)
        {
            KickForAttack.SetActive(false);
        }

        // Throw Back
        else if (prevInputZ == -1)
        {
            KickBackAttack.SetActive(false);
        }
    }

    /* Throw Animation
    ---------------------------------------------- */
    private IEnumerator ThrowAnimation(Material[] throwMats)
    {
        int currentThrowIndex = 0;
        planeRenderer.material = throwMats[currentThrowIndex];
        float frameTimer = 0f;

        if (PlayerMotor.inputX != 0 || PlayerMotor.inputZ != 0)
        {
            prevInputX = PlayerMotor.inputX;
            prevInputZ = PlayerMotor.inputZ;
        }

        while (currentThrowIndex != 7)
        {
            if (isDying)
            {
                break;
            }
            frameTimer += Time.deltaTime;

            if (frameTimer >= frameDelay / 1000f) 
            {
                frameTimer = 0f;
                currentThrowIndex = (currentThrowIndex + 1) % throwMats.Length;
                planeRenderer.material = throwMats[currentThrowIndex];
            }

            yield return null;
        }
        
        
        CupcakeSpawn();

        cRotate = Quaternion.Euler(0, PlayerLook.rotation.eulerAngles.y, 0);

        Instantiate(cupcake, player.position + cRotate * cSpawnPos, Quaternion.identity);

        isThrowing = false;
        PlayerMotor.throwOn = false;
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

    /* Death Animation
    ---------------------------------------------- */
    private IEnumerator DeathAnimation()
    {
        int currentIndex = 0;
        planeRenderer.material = deathMats[currentIndex];
        float frameTimer = 0f;

        while (currentIndex != 28)
        {
            frameTimer += Time.deltaTime;

            if (frameTimer >= frameDelay / 1000f) 
            {
                frameTimer = 0f;
                currentIndex = (currentIndex + 1) % deathMats.Length;
                planeRenderer.material = deathMats[currentIndex];
            }

            yield return null;
        }
    }
}

