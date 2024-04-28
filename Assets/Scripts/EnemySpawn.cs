using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls enemy spawning
public class EnemySpawn : MonoBehaviour
{   
    // Enemy prefab
    [SerializeField]
    private GameObject Enemy;
    // Enemy spawn cooldown
    [SerializeField]
    private float sCooldown = 10f;

    void Start ()
    {
        StartCoroutine(ESpawn());
    }

    IEnumerator ESpawn () 
    {
        // Spawns enemies until the player dies or wins
        while (!PlayerState.isDead && !PlayerState.isWin)
        {
            // starts the cooldown
            yield return new WaitForSeconds(sCooldown);
            // If the loop has already started
            if (PlayerState.isDead)
                break;
            // Sends signal to TMachineState to change animations
            TMachineState.eSpawnTime = true;
            // Spawns enemies on each side of the machine
            Instantiate(Enemy, transform.position + new Vector3(-10, 0, 0), Quaternion.identity);
            Instantiate(Enemy, transform.position + new Vector3(10, 0, 0), Quaternion.identity);
            Instantiate(Enemy, transform.position + new Vector3(0, 0, 10), Quaternion.identity);
            Instantiate(Enemy, transform.position + new Vector3(0, 0, -10), Quaternion.identity);
        }
    }
}
