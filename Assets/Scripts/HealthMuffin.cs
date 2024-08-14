using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        audioSource = GameObject.Find("P Hitbox").GetComponent<AudioSource>();
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
            // prevents player from repeatly healing during cooldown
            if (Healing == true && PlayerState.health != PlayerState.hMax) {
                audioSource.PlayOneShot(healsfx);

                PlayerState.health += heal;
                // caps health at 100
                if (PlayerState.health > PlayerState.hMax)
                {
                    PlayerState.health = PlayerState.hMax;
                }
                healthDisplay.text = $"{PlayerState.health}";

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
        // wait 10 seconds
        yield return new WaitForSeconds(10f);
        Healing = true;
        // renables the object
        muffinAni.Play("FadeIn");
    }
}
