using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Does animation for Cupcake
public class CupcakeAnimation : MonoBehaviour
{
    // Stores Mats for animation
    public Material[] cSpinMats = new Material[8];
    [SerializeField]
    private Transform cupcake;
    [SerializeField]
    private Renderer planeRenderer;
    // Keeps tracks of frames
    float frameTimer;
    // The delay to change mats
    [SerializeField]
    private int frameDelay = 80;
    // Keeps track of the current mat index
    int currentIndex;

    private void Start()
    {
        // Resets timer
        frameTimer = 0f;
        // Starts at the first mat
        currentIndex = 0;
        planeRenderer.material = cSpinMats[currentIndex];
    }

    private void Update()
    {
        frameTimer += Time.deltaTime;
        // When the frame timer reaches the frame delay
        if (frameTimer >= frameDelay / 1000f)
        {
            // Resets timer
            frameTimer = 0f;
            // Sets the next mat of the array
            currentIndex = (currentIndex + 1) % cSpinMats.Length;
            planeRenderer.material = cSpinMats[currentIndex];
        }
    }
}
