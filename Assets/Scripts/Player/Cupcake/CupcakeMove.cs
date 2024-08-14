using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls movement for Cupcake
public class CupcakeMove : MonoBehaviour
{
    // Player
    private CharacterController controller;
    private Vector3 playerVelocity;
    // Direction Cupcake moves
    private Vector3 throwDirection;
    public float gravity = -9.8f;
    // How fast it moves
    [SerializeField]
    private float speed = 5;
    // How long till it is deleted
    [SerializeField]
    private float cSnap = 6;
    public static bool isGrounded;
    private Animator cupcakeAni;
    private CupcakeCounter cCount;
    
    void Start()
    {
        cupcakeAni = GetComponent<Animator>();
        cCount = GameObject.Find("CupcakeCounter").GetComponent<CupcakeCounter>();
        // Gets controller from the object
        controller = GetComponent<CharacterController>();
        // Determines the direction from the player Cupcake moves
        CupcakeDirection();
        throwDirection = PlayerAniThrow.cRotate * throwDirection;
        // Starts the snap countdown
        StartCoroutine(CupcakeSnap());
    }

    void Update()
    {
        // Sees if player is grounded
        isGrounded = controller.isGrounded;

        // Applies movement
        controller.Move(transform.TransformDirection(throwDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        // If in the air
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    // Snaps Cupcake
    private IEnumerator CupcakeSnap()
    {
        yield return new WaitForSeconds(cSnap-1);
        cupcakeAni.Play("CupcakeFade");
        yield return new WaitForSeconds(1f);
        cCount.CounterUp();
        Destroy(gameObject);
    }

    // Determines the direction from the player Cupcake moves
    void CupcakeDirection()
    {
        // Throw Left and Throw Left Diagonal
        if (PlayerAniMovement.prevInputX < 0)
        {   
            // Throw Left Diagonal
            if (PlayerAniMovement.prevInputX != -1)
            {   
                // Throw Left Forward Diagonal
                if (PlayerAniMovement.prevInputZ > 0)
                {
                    throwDirection.x = -1;
                    throwDirection.z = 1;
                }

                // Throw Left Back Diagonal
                else if (PlayerAniMovement.prevInputZ < 0)
                {
                    throwDirection.x = -1;
                    throwDirection.z = -1;
                }
            }
            // Throw Left Only
            else
            {
                throwDirection.x = -1;
                throwDirection.z = 0;
            }
        }

        // Throw Right and Throw Right Diagonal
        else if (PlayerAniMovement.prevInputX > 0)
        {   
            // Throw Right Diagonal
            if (PlayerAniMovement.prevInputX != 1)
            {   
                // Throw Right Forward Diagonal
                if (PlayerAniMovement.prevInputZ > 0)
                {
                    throwDirection.x = 1;
                    throwDirection.z = 1;
                }

                // Throw Right Back Diagonal
                else if (PlayerAniMovement.prevInputZ < 0)
                {
                    throwDirection.x = 1;
                    throwDirection.z = -1;
                }
            }
            // Throw Right Only
            else
            {
                throwDirection.x = 1;
                throwDirection.z = 0;
            }
        }

        // Throw Forward
        else if (PlayerAniMovement.prevInputZ == 1)
        {
            throwDirection.x = 0;
            throwDirection.z = 1;
        }

        // Throw Back
        else if (PlayerAniMovement.prevInputZ == -1)
        {
            throwDirection.x = 0;
            throwDirection.z = -1;
        }

        throwDirection.y = 0;
    }
}
