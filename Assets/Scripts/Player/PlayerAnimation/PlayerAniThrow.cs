using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the player's throwing animations
public class PlayerAniThrow : MonoBehaviour
{
    public bool isThrowing;
    private CupcakeCounter cCount;
    private Animator PlayerAni;
    float atkCooldown;
    private Coroutine attackOffCoroutine;

    [SerializeField]
    private GameObject cupcake;
    private Vector3 cSpawnPos;
    public Quaternion cRotate;
    private PlayerLook look;
    private PlayerState pState;
    private PlayerAniMovement pAniMovement;
    private PlayerAniMelee pAniMelee;

    private void Start()
    {
        PlayerAni = GetComponent<Animator>();
        cCount = GameObject.Find("CupcakeCounter").GetComponent<CupcakeCounter>();
        look = GetComponent<PlayerLook>();
        pState = GetComponentInChildren<PlayerState>();
        pAniMelee = GetComponent<PlayerAniMelee>();
        pAniMovement = GetComponent<PlayerAniMovement>();
    }
    
    public void Throw()
    {
        if (!pAniMelee.isMelee && !isThrowing && cCount.cupcakeCount-1 != -1 && !pState.isDead && !pState.isWin)
        {
            cCount.CounterDown();
            isThrowing = true;

            if (pAniMovement.prevInputX < 0)
                PlayerAni.Play("ThrowLeft");
            else if (pAniMovement.prevInputX > 0)
                PlayerAni.Play("ThrowRight");
            else if (pAniMovement.prevInputZ == 1)
                PlayerAni.Play("ThrowForward");
            else if (pAniMovement.prevInputZ == -1)
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

        isThrowing = false;

        attackOffCoroutine = null;
    }

    /* Spawns A Cupcake
    ---------------------------------------------- */
    private IEnumerator CupcakeSpawn()
    {
        yield return new WaitForSeconds(0.595729166667f);

        CupcakeSpawnPos();

        cRotate = Quaternion.Euler(0, look.playerRotation.eulerAngles.y, 0);

        Instantiate(cupcake, transform.position + cRotate * cSpawnPos, Quaternion.identity);
    }

    /* Where Cupcake spawns
    ---------------------------------------------- */
    void CupcakeSpawnPos()
    {
        // Throw Left and Throw Left Diagonal
        if (pAniMovement.prevInputX < 0)
        {   
            // Throw Left Diagonal
            if (pAniMovement.prevInputX != -1)
            {   
                // Throw Left Forward Diagonal
                if (pAniMovement.prevInputZ > 0)
                {
                    cSpawnPos.x = -6;
                    cSpawnPos.z = 1;
                }

                // Throw Left Back Diagonal
                else if (pAniMovement.prevInputZ < 0)
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
        else if (pAniMovement.prevInputX > 0)
        {   
            // Throw Right Diagonal
            if (pAniMovement.prevInputX != 1)
            {   
                // Throw Right Forward Diagonal
                if (pAniMovement.prevInputZ > 0)
                {
                    cSpawnPos.x = 6;
                    cSpawnPos.z = 1;
                }

                // Throw Right Back Diagonal
                else if (pAniMovement.prevInputZ < 0)
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
        else if (pAniMovement.prevInputZ == 1)
        {
            cSpawnPos.x = 0;
            cSpawnPos.z = 1;
        }

        // Throw Back
        else if (pAniMovement.prevInputZ == -1)
        {
            cSpawnPos.x = 0;
            cSpawnPos.z = -1;
        }

        cSpawnPos.y = -.5f;
    }
}
