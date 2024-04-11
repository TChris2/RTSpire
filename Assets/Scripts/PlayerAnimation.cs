using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
    public Material stillForwardMat;
    public Material stillBackwardMat;
    public Material stillLeftMat;
    public Material stillRightMat;
    public Material[] KickLeft = new Material[8];
    public Material[] KickRight = new Material[8];
    public Material[] KickForward = new Material[8];
    public Material[] KickBack = new Material[8];
    public Material[] JumpStill = new Material[4];
    public Material[] runLeftMats = new Material[7];
    public Material[] runRightMats = new Material[7];
    public Material[] runForwardMats = new Material[7];
    public Material[] runBackMats = new Material[7];
    
    [SerializeField]
    private Transform pback;
    [SerializeField]
    private Renderer planeRenderer;
    private int frameCounter;
    [SerializeField]
    private int frameDelay = 40;
    private bool isRunning;
    private float prevInputX = 0;
    private float prevInputZ = 1;

    private void Start()
    {
        planeRenderer.material = stillForwardMat;
    }

    private void Update()
    {
        // Kicking Animations
        if (PlayerMotor.kickOn == true)
        {
            Debug.Log("AjioknhkikA" + PlayerMotor.kickOn);

            if (prevInputX == -1)
                StartCoroutine(KickAnimation(KickLeft, stillLeftMat));
            if (prevInputX == 1)
                StartCoroutine(KickAnimation(KickRight, stillRightMat));
            if (prevInputZ == 1)
                StartCoroutine(KickAnimation(KickForward, stillForwardMat));
            if (prevInputZ == -1)
                StartCoroutine(KickAnimation(KickBack, stillBackwardMat));
            
        }   
        // Jump Animations
        else if (PlayerMotor.isGrounded == false)
        {   
            StartCoroutine(Jump());
        }
        // Running Animations
        else if (!isRunning)
        {   
            // Left
            if (PlayerMotor.inputX == -1)
                StartCoroutine(RunAnimation(runLeftMats, stillLeftMat));

            // Right
            else if (PlayerMotor.inputX == 1)
                StartCoroutine(RunAnimation(runRightMats, stillRightMat));

            // Forward
            else if (PlayerMotor.inputZ == 1)
                StartCoroutine(RunAnimation(runForwardMats, stillForwardMat));

            // Backwards
            else if (PlayerMotor.inputZ == -1)
                StartCoroutine(RunAnimation(runBackMats, stillBackwardMat));
        }
    }

    private IEnumerator RunAnimation(Material[] runMats, Material stillMat)
    {
        isRunning = true;
        int currentRunIndex = 0;
        planeRenderer.material = runMats[currentRunIndex];
        float frameTimer = 0f;
        prevInputX = PlayerMotor.inputX;
        prevInputZ = PlayerMotor.inputZ;

        while (true)
        {
            frameTimer += Time.deltaTime;

            if (PlayerMotor.isGrounded == false || PlayerMotor.kickOn == true)
            {
                break;
            }
            else if (frameTimer >= frameDelay / 1000f) // frameDelay is in milliseconds, convert to seconds
            {
                frameTimer = 0f;
                currentRunIndex = (currentRunIndex + 1) % runMats.Length;
                planeRenderer.material = runMats[currentRunIndex];
            }

            if (PlayerMotor.inputX != prevInputX || PlayerMotor.inputZ != prevInputZ)
            {
                break;
            }

            yield return null;
        }

        planeRenderer.material = stillMat;
        isRunning = false;
    }

    private IEnumerator Jump()
    {
        while (PlayerMotor.isGrounded == false)
        {
            if (PlayerMotor.inputX == -1)
                planeRenderer.material = JumpStill[0];
            if (PlayerMotor.inputX == 1)
                planeRenderer.material = JumpStill[1];
            if (PlayerMotor.inputZ == 1)
                planeRenderer.material = JumpStill[2];
            if (PlayerMotor.inputZ == -1)
                planeRenderer.material = JumpStill[1];

            yield return null;
        }
// get animations to line up properly, work on diagonal movement
        if (prevInputX == -1)
            planeRenderer.material = stillLeftMat;
        if (prevInputX == 1)
            planeRenderer.material = stillRightMat;
        if (prevInputZ == 1)
            planeRenderer.material = stillForwardMat;
        if (prevInputZ == -1)
            planeRenderer.material = stillBackwardMat;
    }
    
    private IEnumerator KickAnimation(Material[] kickMats, Material stillMat)
    {
        int currentKickIndex = 0;
        planeRenderer.material = kickMats[currentKickIndex];
        float frameTimer = 0f;

        while (currentKickIndex != 7)
        {
            frameTimer += Time.deltaTime;

            if (frameTimer >= frameDelay / 1000f) // frameDelay is in milliseconds, convert to seconds
            {
                frameTimer = 0f;
                currentKickIndex = (currentKickIndex + 1) % kickMats.Length;
                planeRenderer.material = kickMats[currentKickIndex];
            }

            yield return null;
        }

        planeRenderer.material = stillMat;
        PlayerMotor.kickOn = false;
    }
}

