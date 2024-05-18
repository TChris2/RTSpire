using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Incase the player falls out of bounds
public class PlayerOutOfBounds : MonoBehaviour
{
    [SerializeField]
    private CharacterController playerController;
    [SerializeField]
    private AudioClip hurt; 
    private AudioSource audioSource;
    // Fall damage
    [SerializeField]
    private float fDamage = 5f;
    [SerializeField]
    private float AttackCool = 2f;
    // Text
    private TMPro.TMP_Text healthDisplay;
    // Image States
    [SerializeField]
    private GameObject RTNormal;
    [SerializeField]
    private GameObject RTPain;

    void Start()
    {
        audioSource = GameObject.Find("P Hitbox").GetComponent<AudioSource>();
        healthDisplay = GameObject.Find("HealthDisplay").GetComponent<TMPro.TMP_Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the player has entered they will be warped back to their last ground position
        if (other.CompareTag("Player"))
        {   
            audioSource.PlayOneShot(hurt);
            // Will not kill the player if they are on low health
            if (PlayerState.health - fDamage > 0) 
            {
                PlayerState.health -= fDamage;
                healthDisplay.text = $"{PlayerState.health}";
                StartCoroutine(PlayerHurt());
            }

            // Disables player movement temporarily
            playerController.enabled = false; 
            // Teleport the player
            playerController.transform.position = PlayerMotor.lastGroundPos; 
            // Renables player movement
            playerController.enabled = true; 
        }
        // Checks if enemy
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
        // Changes ui
        RTNormal.SetActive(false);
        RTPain.SetActive(true);

        // Gives a short time of invincibility to the player until they can get hit again
        yield return new WaitForSeconds(AttackCool);

        // Changes ui back
        RTNormal.SetActive(true);
        RTPain.SetActive(false);
        PlayerState.isDamaged = false;
    }
}
