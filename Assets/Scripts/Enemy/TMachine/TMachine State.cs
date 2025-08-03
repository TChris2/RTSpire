using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls animation state of TMachines
public class TMachineState : MonoBehaviour
{
    [SerializeField]
    private AudioClip ding; 
    private AudioSource audioSource;
    // Gets health from TMachineEntity
    private TMachineEntity TMEntity;
    // Determines whether the machine is breakable or unbreakable
    [SerializeField]
    private bool isBreakable;
    private Animator TMAni;
    // For spawning enemies
    [SerializeField]
    private Transform centerPos;
    // Enemy prefab
    [SerializeField]
    private GameObject Enemy;
    // Enemy spawn cooldown
    [SerializeField]
    private float sCooldown = 10f;


    void Start()
    {
        // Gets TMachine components
        audioSource = GetComponentInChildren<AudioSource>();
        TMEntity = GetComponentInChildren<TMachineEntity>();
        TMAni = GetComponent<Animator>();

        // Starts spawn function
        StartCoroutine(ESpawn());
    }

    // Spawns enemies
    IEnumerator ESpawn() 
    {
        while (true) 
        {
            // Starts the cooldown
            yield return new WaitForSeconds(sCooldown);

            // If the loop has already started when the machine was already destroyed
            if (isBreakable && TMEntity != null && TMEntity.isTMachineDestroyed) 
                break;

            // plays opening animation and sfx
            TMAni.Play("TMachineOpen");
            audioSource.PlayOneShot(ding);
            float delay = ding.length; 

            // Spawns enemies on each side of the machine
            Instantiate(Enemy, centerPos.position + new Vector3(-10, 0, 0), Quaternion.identity);
            Instantiate(Enemy, centerPos.position + new Vector3(10, 0, 0), Quaternion.identity);
            Instantiate(Enemy, centerPos.position + new Vector3(0, 0, 10), Quaternion.identity);
            Instantiate(Enemy, centerPos.position + new Vector3(0, 0, -10), Quaternion.identity);
            
            // Plays slight delay for sfx before closing machine again
            yield return new WaitForSeconds(delay - 1f);

            // Prevents closed animation from occuring if the TMachine is destroyed
            if (isBreakable && TMEntity != null && TMEntity.isTMachineDestroyed)
                break;

            TMAni.Play("TMachineClosed");
        }
    }
}
