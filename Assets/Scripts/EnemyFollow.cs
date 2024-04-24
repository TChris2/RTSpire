using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        float ranSize = Random.Range(-.1f, .2f);
        eFrame.localScale -= new Vector3(ranSize, ranSize, 0);
                    
        //eFrame.position = currentSize;

        planeRenderer.material = EnemyList[Random.Range(1, 400)];

        // gets player object
        player = GameObject.Find("Player").GetComponent<Transform>();
        enemy = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        // sets player's current position as a destination
        enemy.SetDestination(player.position);

        // enemy stops moving once the player runs out of health
        /*if (LevelReload.health <= 0) {
            enemy.isStopped = true;
        }*/
    }
}
