using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the player's kicking animations
public class PlayerAniMelee : MonoBehaviour
{
    public static bool isMelee;
    public static bool isAtking;
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

    private void Start()
    {
        PlayerAni = gameObject.GetComponent<Animator>();
        comboCount = 0;
    }

    public void Melee()
    {
        if (!isAtking && !PlayerAniThrow.isThrowing && !PlayerState.isDead && !PlayerState.isWin)
        {
            if (PlayerMotor.inputX != 0 || PlayerMotor.inputZ != 0)
            {
                PlayerAniMovement.prevInputX = PlayerMotor.inputX;
                PlayerAniMovement.prevInputZ = PlayerMotor.inputZ;

                if (prevAtkInputX != PlayerAniMovement.prevInputX || prevAtkInputZ != PlayerAniMovement.prevInputZ)
                {
                    comboCount = 0;
                }
            }
            isMelee = true;
            isAtking = true;

            prevAtkInputX = PlayerAniMovement.prevInputX;
            prevAtkInputZ = PlayerAniMovement.prevInputZ;

            if (PlayerMotor.isGrounded && comboCount < 2)
            {
                if (comboCount == 0)
                {
                    if (PlayerAniMovement.prevInputX < 0)
                        PlayerAni.Play("PunchLeftP1");
                    else if (PlayerAniMovement.prevInputX > 0)
                        PlayerAni.Play("PunchRightP1");
                    else if (PlayerAniMovement.prevInputZ == 1)
                        PlayerAni.Play("PunchForwardP1");
                    else if (PlayerAniMovement.prevInputZ == -1)
                        PlayerAni.Play("PunchBackP1");
                }
                else if (comboCount == 1)
                {
                    if (PlayerAniMovement.prevInputX < 0)
                        PlayerAni.Play("PunchLeftP2");
                    else if (PlayerAniMovement.prevInputX > 0)
                        PlayerAni.Play("PunchRightP2");
                    else if (PlayerAniMovement.prevInputZ == 1)
                        PlayerAni.Play("PunchForwardP2");
                    else if (PlayerAniMovement.prevInputZ == -1)
                        PlayerAni.Play("PunchBackP2");
                }

                comboCount += 1;
                atkCooldown = .48f;
            }
            else if (!PlayerMotor.isGrounded || comboCount == 2 && prevAtkInputX == PlayerAniMovement.prevInputX 
                    && prevAtkInputZ == PlayerAniMovement.prevInputZ)
            {
                if (PlayerAniMovement.prevInputX < 0)
                    PlayerAni.Play("KickLeft");
                else if (PlayerAniMovement.prevInputX > 0)
                    PlayerAni.Play("KickRight");
                else if (PlayerAniMovement.prevInputZ == 1)
                    PlayerAni.Play("KickForward");
                else if (PlayerAniMovement.prevInputZ == -1)
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
                PlayerAniMelee.isMelee = false;
                PlayerAniMelee.isAtking = false;
            }
        }
        else if (PlayerAniThrow.isThrowing)
        {
            PlayerAniThrow.isThrowing = false;
        }

        attackOffCoroutine = null;
    }
}
