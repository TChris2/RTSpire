using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarp : MonoBehaviour
{
    public CharacterController playerController;
    public float targetX;
    public float targetY;
    public float targetZ;

    public AudioClip audioClip; 
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //checks if player has entered
        if (other.CompareTag("Player"))
        {   
            //plays lose noise and sets player back to checkpoint
            audioSource.PlayOneShot(audioClip);
            Vector3 targetPosition = new Vector3(targetX, targetY, targetZ);
            playerController.enabled = false; // Disable the CharacterController temporarily
            playerController.transform.position = targetPosition; // Teleport the player
            playerController.enabled = true; // Re-enable the CharacterController
        }
    }

}
