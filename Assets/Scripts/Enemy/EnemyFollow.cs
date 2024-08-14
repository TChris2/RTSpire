using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Controls enemy movement
public class EnemyFollow : MonoBehaviour
{
    // NavMeshAgent
    private NavMeshAgent enemy;
    // Player location
    private Transform player;
    // Checks to see if the player is no longer on hit cooldown and can get back up
    private EnemyHurt eHurt;
    // Checks whether the enemy is on a navmesh
    public bool IsOnNavMesh;
    // Radius to check where the enemy is on a navmesh
    [SerializeField]
    private float checkRadius = 1.0f;
    // Used to check if enemy is on ground layer
    [SerializeField]
    private LayerMask groundLayer;
    // Enemy animator
    Animator eAni;
    // If the enemy can move
    public bool isWalk;
    Rigidbody enemyRb;

    void Start()
    {
        // Gets player transform
        player = GameObject.Find("Player").GetComponent<Transform>();
        enemy = GetComponent<NavMeshAgent>();

        // Gets enemy comps
        eHurt = GetComponent<EnemyHurt>();
        eAni = gameObject.GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody>();
        isWalk = true;
    }

    void Update()
    {
        // If the player has not yet won or died
        if (!PlayerState.isDead && !PlayerState.isWin)  
        {
            // If airWait, the navmesh agent, and isWalk is disabled
            if (!eHurt.airWait && !enemy.enabled && !isWalk)
            {
                // Checks to see if the enemy is on a navmesh
                IsOnNavMesh = CheckIfOnNavMesh();
                // If the enemy is on a navmesh
                if (IsOnNavMesh) 
                {
                    // Sets isWalk to true
                    isWalk = true;
                    // Plays down animation
                    eAni.SetTrigger("Down");
                    // Renables movement
                    StartCoroutine(NavMeshOn());
                }
            }
            // Sets player's current position as a destination
            if (enemy.enabled)
                enemy.SetDestination(player.position);
        }
        // Enemy stops moving once the player is dead or wins
        if (enemy.enabled && PlayerState.isDead || enemy.enabled && PlayerState.isWin) {
            enemy.isStopped = true;
        }
    }

    // Checks to see if the player is on a navmesh
    bool CheckIfOnNavMesh()
    {
        // Gets enemy position
        Vector3 position = transform.position;
        UnityEngine.AI.NavMeshHit hit;
        
        // Check if the position is on a NavMesh surface && ground layer within the specified radius
        if (UnityEngine.AI.NavMesh.SamplePosition(position, out hit, checkRadius, UnityEngine.AI.NavMesh.AllAreas) &&
        Physics.Raycast(position, Vector3.down, out RaycastHit rayHit, checkRadius, groundLayer))
            return true;
        else
            return false;
    }

    // Renables movement
    IEnumerator NavMeshOn() 
    {
        yield return new WaitForSeconds(1f);
        enemyRb.velocity = Vector3.zero;
        enemyRb.angularVelocity = Vector3.zero;
        enemyRb.isKinematic = false;

        // Plays getting up animation
        eAni.Play("Enemy Up");
        // Renables navmeshagent
        enemy.enabled = true;
        enemy.ResetPath();
        eHurt.isHit = false;
    }
}
