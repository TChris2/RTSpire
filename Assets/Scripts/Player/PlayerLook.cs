using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls player camera
public class PlayerLook : MonoBehaviour
{
    private Transform player;
    private Camera pCam;
    [SerializeField]
    private float xRotation = 0f;
    private float yRotation = 0f;
    // Sensitivity of cam movement
    public float rotateSens = 30;
    // Distance from camera
    [SerializeField]
    private float distance = 24;
    public Quaternion playerRotation;
    [HideInInspector]
    public bool isZoomIn = false;
    [HideInInspector]
    public bool isZoomOut = false;
    public float zoomSens = 20;
    
    void Start()
    {
        player = GetComponent<Transform>();
        pCam = GameObject.Find("Main Camera").GetComponent<Camera>();

        // Get player cam distance between lvs/instances
        distance = PlayerPrefs.GetFloat("Cam Distance", 24);

        // Set initial position
        transform.position = player.position + new Vector3(0, 0, -distance);
    }

    // Controls movement of camera
    public void ProcessLook(Vector2 input)
    {
        // Camera can be movement whilst the player still hasn't won or died
        yRotation += (input.x * Time.deltaTime) * -rotateSens;
        xRotation += (input.y * Time.deltaTime) * rotateSens;

        // Restricts up down cam movement
        xRotation = Mathf.Clamp(xRotation, -30, 35);
            
        // Updates cam rotations
        playerRotation = Quaternion.Euler(xRotation, yRotation, 0);
        // Updates cam position
        Vector3 newPosition = player.position + playerRotation * new Vector3(0f, 0f, -distance);
            
        // Apply the new position and look at the target
        pCam.transform.position = newPosition;
        pCam.transform.LookAt(player);
    }

    // Camera zoom in
    public void ZoomIn()
    {   
        if (distance >= 1)
            distance = distance - 1 * zoomSens * Time.deltaTime;
    }

    // Camera zoom out
    public void ZoomOut()
    {   
        if (distance <= 40)
            distance = distance + 1 * zoomSens * Time.deltaTime;
    }

    public void StartZoomIn()
    {
        isZoomIn = true;
    }

    public void StopZoomIn()
    {
        isZoomIn = false;
    }

    public void StartZoomOut()
    {
        isZoomOut = true;
    }

    public void StopZoomOut()
    {
        isZoomOut = false;
    }

    private void OnDisable()
    {
        // Saves adjustments to cam distance
        PlayerPrefs.SetFloat("Cam Distance", distance);
        PlayerPrefs.Save();
    }
}
