using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCamOrientate : MonoBehaviour
{
    private Transform camera;
    
    void Start()
    {
        camera = GameObject.Find("PlayerCam").GetComponent<Transform>();
    }

    void LateUpdate ()
    {
        //orientates player to camera
        Vector3 eulerRotation = new Vector3(transform.eulerAngles.x, camera.transform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Euler(eulerRotation);
    }
}
