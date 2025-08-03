using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Controls enemy movement
public class EnemyFollow : MonoBehaviour
{
    // NavMeshAgent
    private NavMeshAgent enemyAI;
    // Player location
    private Transform player;
    // Checks to see if the player is no longer on hit cooldown and can get back up
    private EnemyHurt eHurt;
    // Checks whether the enemy is on a navmesh
    private bool IsOnNavMesh;
    // Radius to check where the enemy is on a navmesh
    [SerializeField]
    private float checkRadius = 1.0f;
    // Used to check if enemy is on ground layer
    [SerializeField]
    private LayerMask groundLayer;
    // Enemy animator
    Animator eAni;
    // If the enemy can move
    [HideInInspector]
    public bool isWalk = true;
    Rigidbody enemyRb;

    void Start()
    {
        // Gets player transform
        player = GameObject.Find("Player").GetComponent<Transform>();
        enemyAI = GetComponent<NavMeshAgent>();

        // Gets comps
        eHurt = GetComponent<EnemyHurt>();
        eAni = gameObject.GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // While the enemy is still spinning after getting hit
        if (!eHurt.airWait && !enemyAI.enabled && !isWalk)
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
                StartCoroutine(EnemyUp());
            }
        }
        // Sets player's current position as a destination
        if (enemyAI.enabled)
            enemyAI.SetDestination(player.position);
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

    // Gets enemy up and renables movement
    IEnumerator EnemyUp() 
    {
        // Intial delay
        yield return new WaitForSeconds(1f);
        // Renables movement
        enemyRb.velocity = Vector3.zero;
        enemyRb.angularVelocity = Vector3.zero;
        enemyRb.isKinematic = false;

        // Plays getting up animation
        eAni.Play("Enemy Up");
        // Renables navmeshagent
        enemyAI.enabled = true;
        enemyAI.ResetPath();
        eHurt.isHit = false;
    }
}
