using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Controls enemy movement
public class EnemyFollow : MonoBehaviour
{
    private NavMeshAgent enemy;
    private Transform player;
    [SerializeField]
    private Transform eFrame;
    public Material[] EnemyList = new Material[400];
    [SerializeField]
    private Renderer planeRenderer;
    
    void Start()
    {
        // Sets a random size for an enemy
        float ranSize = Random.Range(-.1f, .2f);
        eFrame.localScale -= new Vector3(ranSize, ranSize, 0);

        planeRenderer.material = EnemyList[Random.Range(1, 400)];

        // Gets player object
        player = GameObject.Find("Player").GetComponent<Transform>();
        enemy = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        // Sets player's current position as a destination
        enemy.SetDestination(player.position);

        // Enemy stops moving once the player is dead or wins
        if (PlayerState.isDead || PlayerState.isWin) {
            enemy.isStopped = true;
        }
    }
}
