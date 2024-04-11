using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    [Tooltip("Rotation Speed")]
    [SerializeField]
    public float rSpeed = 30;

    void Update()
    {
        //rotates the object around the y axis
        transform.Rotate(Vector3.up, rSpeed * Time.deltaTime);
    }
}
