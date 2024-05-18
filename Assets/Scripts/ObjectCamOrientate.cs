using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Orientates objects so they face the camera
public class ObjectCamOrientate : MonoBehaviour
{
    private Transform camera;
    
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    void LateUpdate()
    {
        Vector3 eulerRotation = new Vector3(transform.eulerAngles.x, camera.transform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Euler(eulerRotation);
    }
}
