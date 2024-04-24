using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMachineAnimation : MonoBehaviour
{
    public float maxSize = 9f;  // Maximum y size
    public float minSize = 1f;  // Minimum y size
    public float changeSpeed = 1f;  // Speed of size change

    private bool increasing = true;  // Flag to track if size is increasing or decreasing

    void Update()
    {
        if (TMachineState.eSpawnTime == false)
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
