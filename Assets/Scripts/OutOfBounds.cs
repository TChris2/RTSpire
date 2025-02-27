using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// If the player or enemy falls out of bounds
public class OutOfBounds : MonoBehaviour
{
    // Hurt clip for player
    [SerializeField]
    private AudioClip hurt; 
    // Fall damage
    [SerializeField]
    private float fDamage = 5f;
    // Cooldown
    [SerializeField]
    private float attackCool = 2f;
    // Text
    private TMPro.TMP_Text healthDisplay;
    // Health UI Animator
    private Animator playerUIAni;

    void Start()
    {
        // Gets health text
        healthDisplay = GameObject.Find("HealthDisplay").GetComponent<TMPro.TMP_Text>();

        // Gets Health UI animator
        playerUIAni = GameObject.Find("Player UI").GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Gets necessary player components
        PlayerState pState = other.GetComponentInChildren<PlayerState>();
        AudioSource audioSource = other.GetComponentInChildren<AudioSource>();
        CharacterController playerController = other.GetComponent<CharacterController>();
        Animator playerAni = other.GetComponent<Animator>();
        PlayerMotor motor = other.GetComponent<PlayerMotor>();

        // If the player has entered they will be warped back to their last ground position
        if (other.CompareTag("Player"))
        {   
            audioSource.PlayOneShot(hurt);

            // Will not dmg the player if it would kill them
            if (pState.health - fDamage > 0) 
            {
                pState.health -= fDamage;
                healthDisplay.text = $"{pState.health}";
                StartCoroutine(pState.PlayerHurt(attackCool));
            }

            // Disables player movement
            playerController.enabled = false; 
            // Teleports the player
            playerController.transform.position = motor.lastGroundPos; 
            // Renables player movement
            playerController.enabled = true; 
        }
        // Will intstantly kill an enemey if they are out of bounds
        if (other.CompareTag("Enemy"))
        {
            // Kills enemy
            if (other.TryGetComponent(out Entity enemy))
            {
                enemy.Health = 0;
            }
        }
    }
}
