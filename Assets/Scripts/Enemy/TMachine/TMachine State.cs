using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls animation state of TMachines
public class TMachineState : MonoBehaviour
{
    public AudioClip ding; 
    private AudioSource audioSource;
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

        StartCoroutine(ESpawn());
    }

    IEnumerator ESpawn() 
    {
        while (true) 
        {
            // Starts the cooldown
            yield return new WaitForSeconds(sCooldown);

            // If the loop has already started when the machine was already destroyed
            if (isBreakable && TMEntity != null && TMEntity.isTMachineDestroyed) 
                break;

            TMAni.Play("TMachineOpen");
            audioSource.PlayOneShot(ding);
            float delay = ding.length; 

            // Spawns enemies on each side of the machine
            Instantiate(Enemy, centerPos.position + new Vector3(-10, 0, 0), Quaternion.identity);
            Instantiate(Enemy, centerPos.position + new Vector3(10, 0, 0), Quaternion.identity);
            Instantiate(Enemy, centerPos.position + new Vector3(0, 0, 10), Quaternion.identity);
            Instantiate(Enemy, centerPos.position + new Vector3(0, 0, -10), Quaternion.identity);
            
            yield return new WaitForSeconds(delay-1f);

            // Prevents closed animation from spawning when the TMachine is destroyed
            if (isBreakable && TMEntity != null && TMEntity.isTMachineDestroyed)
                break;

            TMAni.Play("TMachineClosed");
        }
    }
}
