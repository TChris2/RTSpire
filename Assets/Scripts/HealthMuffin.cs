using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Heals player & controls how muffins work
public class HealthMuffin : MonoBehaviour
{
    // Player Heal Text
    private TMPro.TMP_Text healthDisplay;
    // Heal Amount
    [SerializeField]
    private float heal = 20f;
    // When the muffin can heal the player
    bool Healing = true;
    // Animator for muffin
    private Animator muffinAni;
    // Heal audio
    [SerializeField]
    private AudioClip healsfx; 
    private AudioSource audioSource;
    [SerializeField]
    private float cooldown = 10f;

    void Start()
    {
        // Gets components
        audioSource = GetComponent<AudioSource>();
        healthDisplay = GameObject.Find("HealthDisplay").GetComponent<TMPro.TMP_Text>();
        muffinAni = gameObject.GetComponent<Animator>();
    }

    // When the player enters the muffin's collider
    private void OnTriggerEnter(Collider other)
    {
        // Checks if player has entered the collider
        if (other.CompareTag("Player"))
        {
            // Gets player's components
            PlayerState pState = other.GetComponentInChildren<PlayerState>();
            Animator playerAni = other.GetComponent<Animator>();

            // Only allows player to heal if they are not at max health
            if (Healing == true && pState.health != pState.hMax)
            {
                // Prevents player from repeatly healing during cooldown
                Healing = false;

                // Plays heal animation
                playerAni.Play("Player Heal");
                audioSource.PlayOneShot(healsfx);

                // Heals player and caps healing at max health
                pState.health = pState.health > pState.hMax ? pState.hMax : pState.health + heal;
                
                // Updates health display
                healthDisplay.text = $"{pState.health}";

                // starts healing cooldown and disables healing
                StartCoroutine(DisRenable());
            }
        }
    }

    // Disables and then renables muffin after cooldown
    IEnumerator DisRenable()
    {
        // Disables muffin
        muffinAni.Play("FadeOut");
        muffinAni.Play("Heal Used");
        // Cooldown time
        yield return new WaitForSeconds(cooldown);
        Healing = true;
        // Renables the muffin
        muffinAni.Play("FadeIn");
        muffinAni.Play("Heal Hint");
    }
}
