using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles how damage is dealt to enemies
public class EnemyHurt : MonoBehaviour
{
    // Applies forces to rigidbody
    Rigidbody enemyRb;
    // Navmeshagent
    UnityEngine.AI.NavMeshAgent enemyAI;
    // Scripts
    // Enemy movment
    EnemyFollow eFollow;
    // Gets health for entity
    Entity eEntity;
    Animator eAni;
    // Checks to see whether the enemy has been hit
    public bool isHit = false;
    [HideInInspector]
    // Controls when the EnemyFollow script can start the checking to renable movement
    public bool airWait = false;

    void Start()
    {
        // Gets the enemy's components
        eEntity = GetComponent<Entity>();
        enemyRb = GetComponent<Rigidbody>();
        enemyAI = GetComponent<UnityEngine.AI.NavMeshAgent>();
        eFollow = GetComponent<EnemyFollow>();
        eAni = GetComponent<Animator>();
        enemyRb.isKinematic = true;
    }

    // When the player's attack enters the enemy's collider
    private void OnTriggerEnter(Collider other)
    {
        // Checks to see if the enemy has already been hit
        if (!isHit)
        {
            // Debug.Log("Something has entered the collider");
            // Checks to see which attack the enemy is being attacked by\
            // If RT attack
            if (other.CompareTag("RT Attack") && other.GetComponent<AttackInfo>())
            {
                // Debug.Log("RT Attacked");
                // Sets to true to prevent the enemy being hit multiple times while hit
                isHit = true;
                airWait = true;
                // Gets collider of attack
                BoxCollider attackCollider = other.GetComponent<BoxCollider>();
                // Gets attack info
                AttackInfo atkInfo = other.GetComponent<AttackInfo>();
                // Deals damage
                eEntity.Health -= atkInfo.dmg;

                // Applies knockback
                StartCoroutine(MeleeKnockback(other, attackCollider, atkInfo));
            }
            // If Cupcake
            else if (other.CompareTag("Cupcake") && other.GetComponent<AttackInfo>())
            {
                // Debug.Log("Cupcake Attacked");
                // Sets to true to prevent the enemy being hit multiple times while hit
                isHit = true;
                airWait = true;
                BoxCollider attackCollider = other.GetComponent<BoxCollider>();
                // Gets attack info
                AttackInfo atkInfo = other.GetComponent<AttackInfo>();
                // Deals damage
                eEntity.Health -= atkInfo.dmg;

                // Applies knockback
                StartCoroutine(CupcakeKnockback(other, attackCollider, atkInfo));
            }
        }
    }
    
    // Applies knockback of melee attacks (Punches & Kicks)
    IEnumerator MeleeKnockback(Collider other, BoxCollider attackCollider, AttackInfo atkInfo) {
        // Gets transform of collider
        Transform attackTransform = attackCollider.transform;

        // Disables navmeshagent
        enemyAI.enabled = false;
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
        Forward = new Vector3(Forward.x, 0, Forward.z).normalized;

        // Adds all forces together
        Vector3 force = (2*Forward + directionToAttack/2).normalized * atkInfo.forForce + Vector3.up * atkInfo.upForce;

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
    IEnumerator CupcakeKnockback(Collider other, Collider attackCollider, AttackInfo atkInfo) {
        // Gets transform of collider
        Transform attackTransform = attackCollider.transform;

        enemyAI.enabled = false;
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
        Forward = new Vector3(Forward.x, 0, Forward.z).normalized;

        // Adds all forces together
        Vector3 force = (Forward/2 + 2*directionToAttack).normalized * atkInfo.forForce + Vector3.up * atkInfo.upForce;
        
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
