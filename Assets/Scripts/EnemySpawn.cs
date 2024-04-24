using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{   
    // Enemy prefab
    [SerializeField]
    private GameObject Enemy;
    // enemy spawn cooldown
    [SerializeField]
    private float sCooldown = 10f;

    void Start ()
    {
        StartCoroutine(ESpawn());
    }

    IEnumerator ESpawn () 
    {
        // spawns enemies whilst it hasn't reached the eTotal cap and the player still has a positive amount of health
        while (true /*&& LevelReload.health > 0*/)
        {
            // starts the cooldown
            yield return new WaitForSeconds(sCooldown);
            
            TMachineState.eSpawnTime = true;
            Instantiate(Enemy, transform.position + new Vector3(-10, 0, 0), Quaternion.identity);
            Instantiate(Enemy, transform.position + new Vector3(10, 0, 0), Quaternion.identity);
            Instantiate(Enemy, transform.position + new Vector3(0, 0, 10), Quaternion.identity);
            Instantiate(Enemy, transform.position + new Vector3(0, 0, -10), Quaternion.identity);
        }
    }
}
