using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the player's throwing animations
public class PlayerAniThrow : MonoBehaviour
{
    public static bool isThrowing;
    private CupcakeCounter cCount;
    private Animator PlayerAni;
    float atkCooldown;
    private Coroutine attackOffCoroutine;

    [SerializeField]
    private GameObject cupcake;
    private Vector3 cSpawnPos;
    public static Quaternion cRotate;

    private void Start()
    {
        PlayerAni = gameObject.GetComponent<Animator>();
        cCount = GameObject.Find("CupcakeCounter").GetComponent<CupcakeCounter>();
    }
    
    public void Throw()
    {
        if (!PlayerAniMelee.isMelee && !isThrowing && cCount.cupcakeCount-1 != -1 && !PlayerState.isDead && !PlayerState.isWin)
        {
            cCount.CounterDown();
            isThrowing = true;

            if (PlayerAniMovement.prevInputX < 0)
                PlayerAni.Play("ThrowLeft");
            else if (PlayerAniMovement.prevInputX > 0)
                PlayerAni.Play("ThrowRight");
            else if (PlayerAniMovement.prevInputZ == 1)
                PlayerAni.Play("ThrowForward");
            else if (PlayerAniMovement.prevInputZ == -1)
                PlayerAni.Play("ThrowBack");

            StartCoroutine(CupcakeSpawn());

            atkCooldown = .817f;
            if (attackOffCoroutine != null)
            {
                StopCoroutine(attackOffCoroutine);
            }
            attackOffCoroutine = StartCoroutine(AttackOff());
        }
    }

    private IEnumerator AttackOff()
    {
        yield return new WaitForSeconds(atkCooldown);

        PlayerAniThrow.isThrowing = false;

        attackOffCoroutine = null;
    }

    /* Spawns A Cupcake
    ---------------------------------------------- */
    private IEnumerator CupcakeSpawn()
    {
        yield return new WaitForSeconds(0.595729166667f);

        CupcakeSpawnPos();

        cRotate = Quaternion.Euler(0, PlayerLook.rotation.eulerAngles.y, 0);

        Instantiate(cupcake, transform.position + cRotate * cSpawnPos, Quaternion.identity);
    }

    /* Where Cupcake spawns
    ---------------------------------------------- */
    void CupcakeSpawnPos()
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
                    cSpawnPos.x = -6;
                    cSpawnPos.z = 1;
                }

                // Throw Left Back Diagonal
                else if (PlayerAniMovement.prevInputZ < 0)
                {
                    cSpawnPos.x = -6;
                    cSpawnPos.z = -1;
                }
            }
            // Throw Left Only
            else
            {
                cSpawnPos.x = -6;
                cSpawnPos.z = 0;
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
                    cSpawnPos.x = 6;
                    cSpawnPos.z = 1;
                }

                // Throw Right Back Diagonal
                else if (PlayerAniMovement.prevInputZ < 0)
                {
                    cSpawnPos.x = 6;
                    cSpawnPos.z = -1;
                }
            }
            // Throw Right Only
            else
            {
                cSpawnPos.x = 6;
                cSpawnPos.z = 0;
            }
        }

        // Throw Forward
        else if (PlayerAniMovement.prevInputZ == 1)
        {
            cSpawnPos.x = 0;
            cSpawnPos.z = 1;
        }

        // Throw Back
        else if (PlayerAniMovement.prevInputZ == -1)
        {
            cSpawnPos.x = 0;
            cSpawnPos.z = -1;
        }

        cSpawnPos.y = -.5f;
    }
}
