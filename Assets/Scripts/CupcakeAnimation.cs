using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupcakeAnimation : MonoBehaviour
{
    public Material[] cSpinMats = new Material[8];
    
    [SerializeField]
    private Transform cupcake;
    [SerializeField]
    private Renderer planeRenderer;
    private int frameCounter;
    [SerializeField]
    private int frameDelay = 80;
    private bool isRunning;
    public static float prevInputX = 0;
    public static float prevInputZ = 1;
    int currentRunIndex;
    float frameTimer;

    private void Start()
    {
        currentRunIndex = 0;
        planeRenderer.material = cSpinMats[currentRunIndex];
        frameTimer = 0f;
    }

    private void Update()
    {
        frameTimer += Time.deltaTime;
        if (frameTimer >= frameDelay / 1000f)
        {
            frameTimer = 0f;
            currentRunIndex = (currentRunIndex + 1) % cSpinMats.Length;
            planeRenderer.material = cSpinMats[currentRunIndex];
        }
    }
}
