using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Controls shadows size & opacity based on distance from the ground
public class ShadowFade : MonoBehaviour
{
    [SerializeField]
    private LayerMask groundLayer;
    public float distanceToGround;
    private DecalProjector decalProjector;
    private float shadowMaxX;
    private float shadowMaxY;
    [SerializeField]
    private float sizeBuff;
    [SerializeField]
    private float sizeDivide;
    private bool isVisable = true;
    private bool isLooping = false;


    void Start()
    {
        decalProjector = GetComponentInChildren<DecalProjector>();
        shadowMaxX = decalProjector.size.x;
        shadowMaxY = decalProjector.size.y;
    }

    void FixedUpdate()
    {

        if (isVisable && !isLooping)
        {
            // Start raycast from current object position downward
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, decalProjector.size.z, groundLayer))
            {
                distanceToGround = hit.distance;
                Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.green);

                // Normalize distance cal
                float normDistance = Mathf.Clamp01(distanceToGround / (decalProjector.size.z / 2));

                // Lerp betweens min and max based on normDistance
                float sizeX = Mathf.Lerp(shadowMaxX + sizeBuff, shadowMaxX * sizeDivide, normDistance);
                float sizeY = Mathf.Lerp(shadowMaxY + sizeBuff, shadowMaxY * sizeDivide, normDistance);
                float opacity = Mathf.Lerp(1, .5f, normDistance);

                // Apply new size and opacity
                decalProjector.size = new Vector3(sizeX, sizeY, decalProjector.size.z);
                decalProjector.fadeFactor = opacity;
            }
            else
            {
                // No ground detected within maxDistance
                distanceToGround = -1f;
                Debug.DrawRay(transform.position, Vector3.down * decalProjector.size.z, Color.red);
            }
        }
    }

    // Starts shadow fade in or out from ShadowFadeInOut which sends animation duration info
    public void FadeStart(float duration)
    {
        StartCoroutine(FadeInOut(duration));
    }

    public IEnumerator FadeInOut(float duration)
    {
        isLooping = true;
        isVisable = !isVisable;

        float finalOpacity = isVisable ? 1 : 0;
        float startOpacity = decalProjector.fadeFactor;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            decalProjector.fadeFactor = Mathf.Lerp(startOpacity, finalOpacity, t);
            yield return null;
        }

        decalProjector.fadeFactor = finalOpacity;
        isLooping = false;
    }

}
