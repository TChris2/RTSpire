using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private Transform player;
    private Camera cam;
    private float xRotation = 0f;
    private float yRotation = 0f;
    public float xSens = 30;
    public float ySens = 30;
    [Tooltip("Distance from player")]
    public static float distance = 15;
    public static Quaternion rotation;
    
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        cam = GameObject.Find("PlayerCam").GetComponent<Camera>();
        // Set initial position
        transform.position = player.position + new Vector3(0, 0, -distance);
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        yRotation += (mouseX * Time.deltaTime) * xSens;
        xRotation += (mouseY * Time.deltaTime) * ySens;
        // Restricts up down cam movement
        xRotation = Mathf.Clamp(xRotation, -30, 60);
        
        // Updates cam rotations
        rotation = Quaternion.Euler(xRotation, yRotation, 0);
        // Updates cam position
        Vector3 newPosition = player.position + rotation * new Vector3(0f, 0f, -distance);
        
        // Apply the new position and look at the target
        cam.transform.position = newPosition;
        cam.transform.LookAt(player);
    }
}