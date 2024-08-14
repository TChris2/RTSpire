using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls player movement
public class PlayerMotor : MonoBehaviour
{
    private Transform pCam;
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float speed = 5;
    public static bool isGrounded;
    public float gravity = -9.8f;
    public float jumpHeight = 3;
    // for player animation
    public static float inputX;
    public static float inputZ;
    public static Vector3 lastGroundPos;

    void Start()
    {
        pCam = GameObject.Find("Main Camera").GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
    }

    // Player Movement
    public void ProcessMove(Vector2 input)
    {
        // Gets the player's last ground pos
        if (isGrounded)
            lastGroundPos = transform.position;
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        // Gets it for playeranimation
        inputX = input.x;
        inputZ = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    // Jumping
    public void Jump()
    {
        if (isGrounded && !PlayerState.isDead && !PlayerState.isWin) 
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3 * gravity);
        }
    }

    // Camera zoom in
    public void ZoomIn()
    {
        if (!PlayerState.isDead && !PlayerState.isWin)
            PlayerLook.distance -= 1;
    }

    // Camera zoom out
    public void ZoomOut()
    {
        if (!PlayerState.isDead && !PlayerState.isWin)
            PlayerLook.distance += 1;
    }

    // Orientates the player to the camera
    void LateUpdate ()
    {
        //orientates player to camera
        Vector3 eulerRotation = new Vector3(transform.eulerAngles.x, pCam.transform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Euler(eulerRotation);
    }
}
