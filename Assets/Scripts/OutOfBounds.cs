using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Incase the player falls out of bounds
public class PlayerOutOfBounds : MonoBehaviour
{
    // Player
    private CharacterController playerController;
    // Hurt clip for player
    [SerializeField]
    private AudioClip hurt; 
    private AudioSource audioSource;
    // Fall damage
    [SerializeField]
    private float fDamage = 5f;
    // Cooldown
    [SerializeField]
    private float AttackCool = 2f;
    // Text
    private TMPro.TMP_Text healthDisplay;

    // Health UI Animator
    private Animator playerUIAni;
    private Animator playerAni;

    void Start()
    {
        // Gets audiosource
        audioSource = GameObject.Find("P Hitbox").GetComponent<AudioSource>();
        // Gets health text
        healthDisplay = GameObject.Find("HealthDisplay").GetComponent<TMPro.TMP_Text>();
        playerController = GameObject.Find("Player").GetComponent<CharacterController>();

        // Gets Health UI animator
        playerUIAni = GameObject.Find("Player UI").GetComponent<Animator>();
        playerAni = GameObject.Find("Player").GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the player has entered they will be warped back to their last ground position
        if (other.CompareTag("Player"))
        {   
            audioSource.PlayOneShot(hurt);

            // Will not dmg the player if it would kill them
            if (PlayerState.health - fDamage > 0) 
            {
                PlayerState.health -= fDamage;
                healthDisplay.text = $"{PlayerState.health}";
                StartCoroutine(PlayerHurt());
            }

            // Disables player movement
            playerController.enabled = false; 
            // Teleports the player
            playerController.transform.position = PlayerMotor.lastGroundPos; 
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

    private IEnumerator PlayerHurt()
    {
        PlayerState.isDamaged = true;

        // Changes UI
        playerUIAni.Play("Hurt");
        playerAni.Play("PlayerHurt");

        // Gives a short time of invincibility to the player until they can get hit again
        yield return new WaitForSeconds(AttackCool);

        // Changes UI back
        playerUIAni.Play("Normal");

        PlayerState.isDamaged = false;
    }
}
