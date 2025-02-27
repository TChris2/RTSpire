using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets random material for an enemy
public class TumbleFrameSetter : MonoBehaviour
{
    [SerializeField]
    private Material [] tumbleMats = new Material[400];
    // Decides which mats are enabled or disabled
    private bool [] tumbleMatsCheck;
    private Renderer planeRenderer;
    int ranNum;
    void Start()
    {
        // Gets plane components
        planeRenderer = GetComponent<Renderer>();

        ranNum = Random.Range(0, tumbleMats.Length);

        // Sets material to plane
        planeRenderer.material = tumbleMats[ranNum];
    }
}
