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
    public float gravity = -9.8f;
    public float jumpHeight = 3;
    // for player animation
    public float inputX;
    public float inputZ;
    public Vector3 lastGroundPos;
    public bool isGrounded;
    private PlayerState pState;

    void Start()
    {
        pCam = GameObject.Find("Main Camera").GetComponent<Transform>();
        controller = GetComponent<CharacterController>();

        pState = GetComponentInChildren<PlayerState>();
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
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    // Jumping
    public void Jump()
    {
        if (isGrounded && !pState.isDead && !pState.isWin) 
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3 * gravity);
        }
    }

    // Orientates the player to the camera
    void LateUpdate ()
    {
        //orientates player to camera
        Vector3 eulerRotation = new Vector3(transform.eulerAngles.x, pCam.transform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Euler(eulerRotation);
    }
}
