using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// How enemies are hit and effects of different on them by different player attacks
public class EnemyHurt : MonoBehaviour
{
    // Gets attack info from the player attack
    AttackInfo atkInfo;
    // Gets health for entity
    Entity eEntity;
    // Applies forces to rigidbody
    Rigidbody enemyRb;
    // Navmeshagent
    UnityEngine.AI.NavMeshAgent enemy;
    // Enemy movment
    EnemyFollow eFollow;
    Animator eAni;
    // Checks to see whether the enemy has been hit
    public bool isHit;
    // Controls when the EnemyFollow script can start the checking to renable movement
    public bool airWait;
    private BoxCollider attackCollider;

    void Start()
    {
        // Gets the enemy's components
        eEntity = GetComponent<Entity>();
        enemyRb = GetComponent<Rigidbody>();
        enemy = GetComponent<UnityEngine.AI.NavMeshAgent>();
        eFollow = GetComponent<EnemyFollow>();
        eAni = GetComponent<Animator>();
        isHit = false;
        enemyRb.isKinematic = true;
        airWait = false;
    }

    // When the enemy enters the collider of one of the player's attacks
    private void OnTriggerEnter(Collider other)
    {
        // Checks to see if the enemy has already been hit
        if (!isHit)
        {
            // Checks to see which attack the enemy is being attacked by\
            // If kick
            if (other.CompareTag("RT Kick") && other.gameObject.name == "Kick Hitbox")
            {
                // Sets to true to prevent the enemy being hit multiple times while hit
                isHit = true;
                airWait = true;
                // Gets collider of attack
                attackCollider = other.GetComponent<BoxCollider>();
                // Gets attack info
                atkInfo = other.GetComponent<AttackInfo>();
                // Deals damage
                eEntity.Health -= atkInfo.dmg;

                // Applies knockback
                StartCoroutine(MeleeKnockback(other));
            }
            // If punch
            else if (other.CompareTag("RT Punch") && other.gameObject.name == "Punch Hitbox")
            {
                // Sets to true to prevent the enemy being hit multiple times while hit
                isHit = true;
                airWait = true;
                // Gets collider of attack
                attackCollider = other.GetComponent<BoxCollider>();
                // Gets attack info
                atkInfo = other.GetComponent<AttackInfo>();
                // Deals damage
                eEntity.Health -= atkInfo.dmg;

                // Applies knockback
                StartCoroutine(MeleeKnockback(other));
            }
            // If Cupcake
            else if (other.CompareTag("Cupcake") && other.gameObject.name == "Cupcake Orientate")
            {
                // Sets to true to prevent the enemy being hit multiple times while hit
                isHit = true;
                airWait = true;
                attackCollider = other.GetComponent<BoxCollider>();
                // Gets attack info
                atkInfo = other.GetComponent<AttackInfo>();
                // Deals damage
                eEntity.Health -= atkInfo.dmg;

                // Applies knockback
                StartCoroutine(CupcakeKnockback(other));
            }
        }
    }
    
    // Applies knockback of melee attacks (Punches & Kicks)
    IEnumerator MeleeKnockback(Collider other) {
        // Gets transform of collider
        Transform attackTransform = attackCollider.transform;

        // Disables navmeshagent
        enemy.enabled = false;
        enemyRb.isKinematic = false;

        // Calculate the direction from the collider to the attack transform
        Vector3 attackPosition = attackTransform.position;
        Vector3 ePos = transform.position;
        Vector3 directionToAttack = (ePos - attackPosition).normalized;
        // Sets y comp to zero
        directionToAttack = new Vector3(directionToAttack.x, 0, directionToAttack.z).normalized;
        
        // Gets forward of attack collider
        Vector3 Forward = attackTransform.forward;
        // Sets y comp to zero
        Vector3 knockbackDirection = new Vector3(Forward.x, 0, Forward.z).normalized;

        // Adds all forces together
        Vector3 force = (2*knockbackDirection + directionToAttack/2).normalized * atkInfo.forForce + Vector3.up * atkInfo.upForce;

        yield return new WaitForSeconds(.1f);
        
        eFollow.isWalk = false;

        // Applies force
        enemyRb.AddForce(force, ForceMode.Impulse);
        // Plays enemy attack animation
        eAni.Play("Enemy Spin");

        yield return new WaitForSeconds(.5f);
        // Begins the checking process to renable movement
        airWait = false;
    }

    // Applies knockback of Cupcake
    IEnumerator CupcakeKnockback(Collider other) {
        // Gets transform of collider
        Transform attackTransform = attackCollider.transform;

        enemy.enabled = false;
        enemyRb.isKinematic = false;

        // Calculate the direction from the collider to the attack transform
        Vector3 attackPosition = attackTransform.position;
        Vector3 ePos = transform.position;
        Vector3 directionToAttack = (ePos - attackPosition).normalized;
        // Sets y comp to zero
        directionToAttack = new Vector3(directionToAttack.x, 0, directionToAttack.z).normalized;

        // Gets forward of attack collider
        Vector3 Forward = attackTransform.forward;
        // Sets y comp to zero
        Vector3 knockbackDirection = new Vector3(Forward.x, 0, Forward.z).normalized;

        // Adds all forces together
        Vector3 force = (knockbackDirection/2 + 2*directionToAttack).normalized * atkInfo.forForce + Vector3.up * atkInfo.upForce;
        
        yield return new WaitForSeconds(.1f);

        eFollow.isWalk = false;
        // Applies force
        enemyRb.AddForce(force, ForceMode.Impulse);
        // Plays enemy attack animation
        eAni.Play("Enemy Spin");

        yield return new WaitForSeconds(.5f);
        // Begins the checking process to renable movement
        airWait = false;
    }
}
