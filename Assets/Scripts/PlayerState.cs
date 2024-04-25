using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    // Image States
    [SerializeField]
    private GameObject RTNormal;
    [SerializeField]
    private GameObject RTPain;
    [SerializeField]
    private GameObject RTDead;
    [SerializeField]
    private GameObject DeathUI;
    [SerializeField]
    private Animator deathUITransition;
    [SerializeField]
    private GameObject DeathLoad;
    [SerializeField]
    private Animator deathLoadTransition;
    // Text
    private TMPro.TMP_Text healthDisplay;
    public AudioClip[] hurt; 
    public AudioClip vonLaugh; 
    private AudioSource audioSource;
    // Health
    public static float health = 100;
    // Enemy Damage
    [SerializeField]
    private float eDamage = 5f;
    [SerializeField]
    private float eAttackCool = 2f;
    public static bool isDead = false;
    public static bool isDamaged = false;

    void Awake()
    {   
        audioSource = gameObject.GetComponent<AudioSource>();
        
        RTDead.SetActive(false);
        RTPain.SetActive(false);
        DeathUI.SetActive(false);
        DeathLoad.SetActive(false);

        isDead = false;
        isDamaged = false;

        healthDisplay = GameObject.Find("HealthDisplay").GetComponent<TMPro.TMP_Text>();
        
        health = PlayerPrefs.GetFloat("PlayerHealth", 0);
        health = 100;
        healthDisplay.text = $"{health}";
    }
    
    // Takes damage from enemy
    private void OnTriggerEnter(Collider other)
    {
        if (!isDamaged)
        {
            // Check if the entering collider has the tag "Enemy"
            if (other.CompareTag("Enemy"))
            {
                isDamaged = true;
                health -= eDamage;
                healthDisplay.text = $"{health}";
                if (health > 0)
                    StartCoroutine(PlayerHurt());
                // Death
                else if (health <= 0) {
                    isDead = true;
                    StartCoroutine(PlayerDead());
                }
            }
        }
    }

    private IEnumerator PlayerHurt()
    {
        RTNormal.SetActive(false);
        RTPain.SetActive(true);

        audioSource.PlayOneShot(hurt[0]);
        yield return new WaitForSeconds(eAttackCool);
        // make win animation
        // add transition for retry button
        // Add Retry button when dead
        // Add Muffin pickup
        // try to figure out issue with TMachine

        RTNormal.SetActive(true);
        RTPain.SetActive(false);
        isDamaged = false;
    }

    private IEnumerator PlayerDead()
    {
        audioSource.PlayOneShot(hurt[1]);
        float delay = hurt[1].length; 
        RTNormal.SetActive(false);
        RTDead.SetActive(true);
        yield return new WaitForSeconds(delay-1.5f);
        
        DeathLoad.SetActive(true);
        deathLoadTransition.speed = .4f;
        deathLoadTransition.SetTrigger("Death");
        audioSource.PlayOneShot(vonLaugh);
        delay = vonLaugh.length; 
        Debug.Log(delay);
        yield return new WaitForSeconds(delay);

        DeathUI.SetActive(true);
        deathUITransition.SetTrigger("Death");
        yield return null;
    }

    private void OnDisable()
    {
        //saves high score
        PlayerPrefs.SetFloat("PlayerHealth", health);
        PlayerPrefs.Save();
    }
}
