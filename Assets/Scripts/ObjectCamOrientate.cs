using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Orientates objects so they face the camera
public class ObjectCamOrientate : MonoBehaviour
{
    // Main/Player cam
    private Transform pCam;
    
    void Start()
    {
        // Gets Cam
        pCam = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    void LateUpdate()
    {
        // Rotates the object to face the camera
        Vector3 eulerRotation = new Vector3(transform.eulerAngles.x, pCam.transform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Euler(eulerRotation);
    }
}
