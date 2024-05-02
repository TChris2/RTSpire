using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TMachineAnimation
public class TMachineAnimation : MonoBehaviour
{
    // Max y size
    public float maxSize = 9f;  
    // Min y size
    public float minSize = 1f;  
    // Speed of size change
    public float changeSpeed = 1f;  

    // Flag to track if size is increasing or decreasing
    private bool increasing = true;  
    public TMachineState TMState;

    void Update()
    {
        if (TMState.eSpawnTime == false)
        {
            // Check if size is increasing
            if (increasing)
            {
                // Increase y size
                transform.localScale += new Vector3(0f, changeSpeed * Time.deltaTime, 0f);
                
                // If size reaches or exceeds maxSize, start decreasing size
                if (transform.localScale.y >= maxSize)
                {
                    transform.localScale = new Vector3(transform.localScale.x, maxSize, transform.localScale.z);
                    increasing = false;
                }
            }
            else
            {
                // Decrease y size
                transform.localScale -= new Vector3(0f, changeSpeed * Time.deltaTime, 0f);
                
                // If size reaches or falls below minSize, start increasing size
                if (transform.localScale.y <= minSize)
                {
                    transform.localScale = new Vector3(transform.localScale.x, minSize, transform.localScale.z);
                    increasing = true;
                }
            }
        }
    }
}
