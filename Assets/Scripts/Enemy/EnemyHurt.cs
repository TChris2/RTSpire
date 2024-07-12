using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour
{
    AttackInfo atInfo;
    Entity eEntity;
    Rigidbody enemyRb;
    UnityEngine.AI.NavMeshAgent enemy;
    EnemyFollow eFollow;
    Animator eAni;
    public bool isHit;
    public bool airWait;
    private BoxCollider attackCollider;
    void Start()
    {
        eEntity = GetComponent<Entity>();
        enemyRb = GetComponent<Rigidbody>();
        enemy = GetComponent<UnityEngine.AI.NavMeshAgent>();
        eFollow = GetComponent<EnemyFollow>();
        eAni = GetComponent<Animator>();
        isHit = false;
        enemyRb.isKinematic = true;
        airWait = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isHit)
        {
            if (other.CompareTag("RT Kick") && other.gameObject.name == "Kick Hitbox")
            {
                isHit = true;
                airWait = true;
                attackCollider = other.GetComponent<BoxCollider>();
                atInfo = other.GetComponent<AttackInfo>();
                // Deals damage
                eEntity.Health -= atInfo.dmg;

                StartCoroutine(KickKnockback(other));
            }
            else if (other.CompareTag("RT Punch") && other.gameObject.name == "Punch Hitbox")
            {
                isHit = true;
                airWait = true;
                attackCollider = other.GetComponent<BoxCollider>();
                atInfo = other.GetComponent<AttackInfo>();
                // Deals damage
                eEntity.Health -= atInfo.dmg;

                StartCoroutine(KickKnockback(other));
            }
            else if (other.CompareTag("Cupcake") && other.gameObject.name == "Cupcake Orientate")
            {
                isHit = true;
                airWait = true;
                attackCollider = other.GetComponent<BoxCollider>();
                atInfo = other.GetComponent<AttackInfo>();
                // Deals damage
                eEntity.Health -= atInfo.dmg;

                StartCoroutine(CupcakeKnockback(other));
            }
        }
    }
    
    IEnumerator KickKnockback(Collider other) {
        Transform attackTransform = attackCollider.transform;

        enemy.enabled = false;
        enemyRb.isKinematic = false;

        // Calculate the direction from the collider to the attack transform
        Vector3 attackPosition = attackTransform.position;
        Vector3 ePos = transform.position;

        // Calculate the direction from the enemy to the attack transform
        Vector3 directionToAttack = (ePos - attackPosition).normalized;
        directionToAttack = new Vector3(directionToAttack.x, 0, directionToAttack.z).normalized;
        
        Vector3 Forward = attackTransform.forward;
        Vector3 knockbackDirection = new Vector3(Forward.x, 0, Forward.z).normalized;

        // Apply the knockback force
        Vector3 force = (2*knockbackDirection + directionToAttack/2).normalized * atInfo.forForce + Vector3.up * atInfo.upForce;

        yield return new WaitForSeconds(.1f);

        eFollow.isWalk = false;
        enemyRb.AddForce(force, ForceMode.Impulse);
        eAni.Play("Enemy Spin");

        yield return new WaitForSeconds(.5f);
        airWait = false;
    }

    IEnumerator CupcakeKnockback(Collider other) {
        Transform attackTransform = attackCollider.transform;

        enemy.enabled = false;
        enemyRb.isKinematic = false;

        // Calculate the direction from the collider to the attack transform
        Vector3 attackPosition = attackTransform.position;
        Vector3 ePos = transform.position;

        // Calculate the direction from the enemy to the attack transform
        Vector3 directionToAttack = (ePos - attackPosition).normalized;

        Vector3 Forward = attackTransform.forward;
        Vector3 knockbackDirection = new Vector3(Forward.x, 0, Forward.z).normalized;

        // Apply the knockback force
        Vector3 force = (knockbackDirection/2 + 2*directionToAttack).normalized * atInfo.forForce + Vector3.up * atInfo.upForce;
        
        yield return new WaitForSeconds(.1f);

        eFollow.isWalk = false;
        enemyRb.AddForce(force, ForceMode.Impulse);
        eAni.Play("Enemy Spin");

        yield return new WaitForSeconds(.5f);
        airWait = false;
    }
}
