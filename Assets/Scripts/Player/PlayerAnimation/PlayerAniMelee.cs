using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the player's melee animations
public class PlayerAniMelee : MonoBehaviour
{
    public bool isMelee;
    public bool isAtking;
    private Animator PlayerAni;
    float comboCount;
    float atkCooldown;
    private Coroutine attackOffCoroutine;
    float prevAtkInputX;
    float prevAtkInputZ;
    [SerializeField]
    private float t1 = .6f;
    [SerializeField]
    private float t2 = .2f;
    private PlayerState pState;
    private PlayerAniThrow pAniThrow;
    private PlayerAniMovement pAniMovement;
    private PlayerMotor motor;

    private void Start()
    {
        PlayerAni = GetComponent<Animator>();
        comboCount = 0;
        pState = GetComponentInChildren<PlayerState>();
        pAniThrow = GetComponent<PlayerAniThrow>();
        pAniMovement = GetComponent<PlayerAniMovement>();
        motor = GetComponent<PlayerMotor>();
    }

    public void Melee()
    {
        if (!isAtking && !pAniThrow.isThrowing && !pState.isDead && !pState.isWin)
        {
            if (motor.inputX != 0 || motor.inputZ != 0)
            {
                pAniMovement.prevInputX = motor.inputX;
                pAniMovement.prevInputZ = motor.inputZ;

                if (prevAtkInputX != pAniMovement.prevInputX || prevAtkInputZ != pAniMovement.prevInputZ)
                {
                    comboCount = 0;
                }
            }
            isMelee = true;
            isAtking = true;

            prevAtkInputX = pAniMovement.prevInputX;
            prevAtkInputZ = pAniMovement.prevInputZ;

            if (motor.isGrounded && comboCount < 2)
            {
                if (comboCount == 0)
                {
                    if (pAniMovement.prevInputX < 0)
                        PlayerAni.Play("PunchLeftP1");
                    else if (pAniMovement.prevInputX > 0)
                        PlayerAni.Play("PunchRightP1");
                    else if (pAniMovement.prevInputZ == 1)
                        PlayerAni.Play("PunchForwardP1");
                    else if (pAniMovement.prevInputZ == -1)
                        PlayerAni.Play("PunchBackP1");
                }
                else if (comboCount == 1)
                {
                    if (pAniMovement.prevInputX < 0)
                        PlayerAni.Play("PunchLeftP2");
                    else if (pAniMovement.prevInputX > 0)
                        PlayerAni.Play("PunchRightP2");
                    else if (pAniMovement.prevInputZ == 1)
                        PlayerAni.Play("PunchForwardP2");
                    else if (pAniMovement.prevInputZ == -1)
                        PlayerAni.Play("PunchBackP2");
                }

                comboCount += 1;
                atkCooldown = .48f;
            }
            else if (!motor.isGrounded || comboCount == 2 && prevAtkInputX == pAniMovement.prevInputX 
                    && prevAtkInputZ == pAniMovement.prevInputZ)
            {
                if (pAniMovement.prevInputX < 0)
                    PlayerAni.Play("KickLeft");
                else if (pAniMovement.prevInputX > 0)
                    PlayerAni.Play("KickRight");
                else if (pAniMovement.prevInputZ == 1)
                    PlayerAni.Play("KickForward");
                else if (pAniMovement.prevInputZ == -1)
                    PlayerAni.Play("KickBack");

                atkCooldown = .817f;
                comboCount = 3;
            }

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

        if (isAtking)
        {
            if (comboCount <= 2 && comboCount != 0)
            {
                isAtking = false;
                float temp = comboCount;
                yield return new WaitForSeconds(t1);
                if (!isAtking && temp == comboCount)
                {
                    comboCount = 0;
                    isMelee = false;
                }
            }
            else if (comboCount == 3)
            {
                comboCount = 0;
                yield return new WaitForSeconds(t2);
                isMelee = false;
                isAtking = false;
            }
        }
        else if (pAniThrow.isThrowing)
        {
            pAniThrow.isThrowing = false;
        }

        attackOffCoroutine = null;
    }
}
