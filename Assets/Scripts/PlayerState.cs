using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Controls player state and player ui alongside lv load transitions
public class PlayerState : MonoBehaviour
{
    // Image States
    [SerializeField]
    private GameObject RTNormal;
    [SerializeField]
    private GameObject RTPain;
    [SerializeField]
    private GameObject RTDead;
    // loading
    [SerializeField]
    private GameObject DeathUI;
    [SerializeField]
    private Animator deathUITransition;
    [SerializeField]
    private GameObject DeathLoad;
    [SerializeField]
    private Animator deathLoadTransition;
    [SerializeField]
    private GameObject LvLoadIntro;
    [SerializeField]
    private Animator lvLoadIntroTransition;
    [SerializeField]
    private GameObject LvLoadOutro;
    [SerializeField]
    private Animator lvLoadOutroTransition;
    // Text
    private TMPro.TMP_Text healthDisplay;
    // Audio
    public AudioClip[] hurt; 
    public AudioClip vonLaugh; 
    public AudioClip win; 
    private AudioSource audioSource;
    // Health
    public static float health = 100;
    // Enemy Damage
    [SerializeField]
    private float eDamage = 5f;
    [SerializeField]
    private float eAttackCool = 2f;
    public static bool isDead;
    public static bool isDamaged;
    public static bool isWin;

    void Awake()
    {   
        audioSource = gameObject.GetComponent<AudioSource>();
        
        // Sets player ui states
        RTDead.SetActive(false);
        RTPain.SetActive(false);
        DeathUI.SetActive(false);
        DeathLoad.SetActive(false);
        LvLoadOutro.SetActive(false);
        
        // Lv intro transition
        LvLoadIntro.SetActive(true);
        lvLoadIntroTransition.speed = .9f;
        lvLoadIntroTransition.SetTrigger("Start");

        isDead = false;
        isDamaged = false;
        isWin = false;

        // Sets health display
        healthDisplay = GameObject.Find("HealthDisplay").GetComponent<TMPro.TMP_Text>();
        // Gets player's health from previous level
        health = PlayerPrefs.GetFloat("PlayerHealth", 0);
        healthDisplay.text = $"{health}";
    }

    void Update()
    {
        // Checks if the player has won
        if (isWin && !PlayerAnimation.isWinning) {
            StartCoroutine(PlayerWin());
        } 
    }
    
    // Takes damage from enemy
    private void OnTriggerEnter(Collider other)
    {
        if (!isDamaged && !PlayerState.isDead && !PlayerState.isWin)
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
        // Changes ui
        RTNormal.SetActive(false);
        RTPain.SetActive(true);

        audioSource.PlayOneShot(hurt[0]);
        // Gives a short time of invincibility to the player until they can get hit again
        yield return new WaitForSeconds(eAttackCool);

        // Changes ui back
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
        // Waits until the player dead sfx finishes playing
        yield return new WaitForSeconds(delay-1f);
        
        // Death load screen
        DeathLoad.SetActive(true);
        deathLoadTransition.speed = .5f;
        deathLoadTransition.SetTrigger("Death");
        audioSource.PlayOneShot(vonLaugh);
        delay = vonLaugh.length; 
        // Waits until the laugh sfx plays
        yield return new WaitForSeconds(delay+.5f);

        // Restart screen + Main menu screen
        DeathUI.SetActive(true);
        deathUITransition.SetTrigger("Death");
        yield return null;
    }

    private IEnumerator PlayerWin()
    {
        audioSource.PlayOneShot(win);
        float delay = win.length; 
        // Waits until the player win sfx finishes playing
        yield return new WaitForSeconds(delay);
        
        // Lv outro transition
        LvLoadOutro.SetActive(true);
        lvLoadOutroTransition.speed = .8f;
        lvLoadOutroTransition.SetTrigger("Win");

        yield return new WaitForSeconds(2f);
        // Next level
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex+1);

        yield return null;
    }

    private void OnDisable()
    {
        // Saves player health for next lv
        PlayerPrefs.SetFloat("PlayerHealth", health);
        PlayerPrefs.Save();
    }
}
