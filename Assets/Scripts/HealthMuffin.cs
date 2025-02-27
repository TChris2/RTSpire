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
    bool Healing;
    // Animator for muffin
    private Animator muffinAni;
    // Heal audio
    [SerializeField]
    private AudioClip healsfx; 
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        healthDisplay = GameObject.Find("HealthDisplay").GetComponent<TMPro.TMP_Text>();
        muffinAni = gameObject.GetComponent<Animator>();
        muffinAni.Play("HealBob");
        muffinAni.speed = .8f;
        Healing = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //checks if player has entered
        if (other.CompareTag("Player"))
        {   
            // Gets player's components
            PlayerState pState = other.GetComponentInChildren<PlayerState>();
            Animator playerAni = other.GetComponent<Animator>();

            // prevents player from repeatly healing during cooldown
            if (Healing == true && pState.health != pState.hMax) {
                playerAni.Play("Player Heal");
                audioSource.PlayOneShot(healsfx);

                pState.health += heal;
                // caps health at 100
                if (pState.health > pState.hMax)
                {
                    pState.health = pState.hMax;
                }
                healthDisplay.text = $"{pState.health}";

                // starts healing cooldown and disables healing
                StartCoroutine(DisRenable());
            }
        }
    }

    IEnumerator DisRenable()
    {
        Healing = false;
        // disables object
        muffinAni.Play("FadeOut");
        muffinAni.Play("Heal Used");
        // wait 10 seconds
        yield return new WaitForSeconds(10f);
        Healing = true;
        // renables the object
        muffinAni.Play("FadeIn");
        muffinAni.Play("Heal Hint");
    }
}
