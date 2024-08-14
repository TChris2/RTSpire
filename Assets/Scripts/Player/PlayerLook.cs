using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls player camera
public class PlayerLook : MonoBehaviour
{
    private Transform player;
    private Camera pCam;
    private float xRotation = 0f;
    private float yRotation = 0f;
    // Sensitivity of cam movement
    public float xSens = 30;
    public float ySens = 30;
    // Distance from camera
    public static float distance = 24;
    public static Quaternion rotation;
    
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        pCam = GameObject.Find("Main Camera").GetComponent<Camera>();

        // Get player cam distance between lvs/instances
        distance = PlayerPrefs.GetFloat("CamDistance", 24);

        // Set initial position
        transform.position = player.position + new Vector3(0, 0, -distance);
    }

    // Controls movement of camera
    public void ProcessLook(Vector2 input)
    {
        // Camera can be movement whilst the player still hasn't won or died
        yRotation += (input.x * Time.deltaTime) * xSens;
        xRotation += (input.y * Time.deltaTime) * ySens;

        // Restricts up down cam movement
        xRotation = Mathf.Clamp(xRotation, -30, 60);
            
        // Updates cam rotations
        rotation = Quaternion.Euler(xRotation, yRotation, 0);
        // Updates cam position
        Vector3 newPosition = player.position + rotation * new Vector3(0f, 0f, -distance);
            
        // Apply the new position and look at the target
        pCam.transform.position = newPosition;
        pCam.transform.LookAt(player);
    }

    private void OnDisable()
    {
        // Saves adjustments to cam distance
        PlayerPrefs.SetFloat("CamDistance", distance);
        PlayerPrefs.Save();
    }
}
