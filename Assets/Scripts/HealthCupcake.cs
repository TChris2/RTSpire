using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCupcake : MonoBehaviour
{
    // Text
    private TMPro.TMP_Text healthDisplay;
    public GameObject hMuffin;
    // Heal Amount
    [SerializeField]
    private float heal = 20f;
    bool Healing;
    [SerializeField]
    private Animator HealBob;

    [SerializeField]
    private AudioClip healsfx; 
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GameObject.Find("P Hitbox").GetComponent<AudioSource>();
        healthDisplay = GameObject.Find("HealthDisplay").GetComponent<TMPro.TMP_Text>();
        HealBob.speed = .8f;

        Healing = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //checks if player has entered
        if (other.CompareTag("Player"))
        {   
            // prevents player from repeatly healing during cooldown
            if (Healing == true) {
                audioSource.PlayOneShot(healsfx);

                PlayerState.health += heal;
                // caps health at 100
                if (PlayerState.health > 100)
                {
                    PlayerState.health = 100;
                }
                healthDisplay.text = $"{PlayerState.health}";

                // starts healing cooldown and disables healing
                StartCoroutine(DeleteRespawn());
            }
        }
    }

    IEnumerator DeleteRespawn()
    {
        Healing = false;
        // disables object
        hMuffin.SetActive(false);
        // wait 10 seconds
        yield return new WaitForSeconds(10f);
        Healing = true;
        // renables the object
        hMuffin.SetActive(true);
    }
}
