using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private Transform camera;
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float speed = 5;
    public static bool isGrounded;
    public static bool kickOn = false;
    public static bool throwOn = false;
    public float gravity = -9.8f;
    public float jumpHeight = 3;
    // for player animation
    public static float inputX;
    public static float inputZ;

    void Start()
    {
        camera = GameObject.Find("PlayerCam").GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
    }

    public void ProcessMove(Vector2 input)
    {
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

    public void Jump()
    {
        if (isGrounded) 
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3 * gravity);
        }
    }

    public void Kick()
    {
        if (!kickOn && !PlayerAnimation.isThrowing)
        {
            kickOn = true;
        }
    }

    public void Throw()
    {
        if (!throwOn && !PlayerAnimation.isKicking)
        {
            throwOn = true;
        }
    }

    public void ZoomIn()
    {
        PlayerLook.distance -= 1;
    }

    public void ZoomOut()
    {
        PlayerLook.distance += 1;
    }

    void LateUpdate ()
    {
        //orientates player to camera
        Vector3 eulerRotation = new Vector3(transform.eulerAngles.x, camera.transform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Euler(eulerRotation);
    }
}
