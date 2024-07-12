using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Controls enemy movement
public class EnemyFollow : MonoBehaviour
{
    private NavMeshAgent enemy;
    private Transform player;
    private EnemyHurt eHurt;
    public bool IsOnNavMesh;
    [SerializeField]
    private float checkRadius = 1.0f;
    [SerializeField]
    private LayerMask groundLayer;
    Animator eAni;
    public bool isWalk;
    Rigidbody enemyRb;

    void Start()
    {

        // Gets player object
        player = GameObject.Find("Player").GetComponent<Transform>();
        enemy = GetComponent<NavMeshAgent>();

        eHurt = GetComponent<EnemyHurt>();
        eAni = gameObject.GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody>();
        isWalk = true;
    }

    void Update()
    {
        if (!PlayerState.isDead && !PlayerState.isWin)  
        {
            if (!eHurt.airWait && !enemy.enabled && !isWalk)
            {
                IsOnNavMesh = CheckIfOnNavMesh();
                if (IsOnNavMesh) 
                {
                    isWalk = true;
                    eAni.SetTrigger("Down");
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

    bool CheckIfOnNavMesh()
    {
        // Define the position to check
        Vector3 position = transform.position;
        UnityEngine.AI.NavMeshHit hit;
        
        // Check if the position is on a NavMesh surface within the specified radius
        if (UnityEngine.AI.NavMesh.SamplePosition(position, out hit, checkRadius, UnityEngine.AI.NavMesh.AllAreas) &&
        Physics.Raycast(position, Vector3.down, out RaycastHit rayHit, checkRadius, groundLayer))
            return true;
        else
            return false;
    }

    IEnumerator NavMeshOn() 
    {
        yield return new WaitForSeconds(1f);
        enemyRb.velocity = Vector3.zero;
        enemyRb.angularVelocity = Vector3.zero;
        enemyRb.isKinematic = false;

        eAni.Play("Enemy Up");
        enemy.enabled = true;
        enemy.ResetPath();
        eHurt.isHit = false;
    }
}
