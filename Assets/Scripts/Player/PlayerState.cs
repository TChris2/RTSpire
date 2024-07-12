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
    // Loading Transitions
    [SerializeField]
    private GameObject DeathUI;
    private Animator deathUITransition;
    [SerializeField]
    private GameObject DeathLoad;
    private Animator deathLoadTransition;
    [SerializeField]
    private GameObject LvLoad;
    private Animator lvLoadTransition;
    // Text
    private TMPro.TMP_Text healthDisplay;
    // Audio
    public AudioClip[] hurt; 
    public AudioClip vonLaugh; 
    public AudioClip win; 
    private AudioSource audioSource;
    // Health
    public static float health = 100;
    [SerializeField]
    private float eAttackCool = 2f;
    public static bool isDead;
    public static bool isDamaged;
    public static bool isWin;
    public static bool hasWon;
    [SerializeField]
    private bool isInvincible;

    AttackInfo eAtInfo;
    EnemyHurt eHurt;

    private Coroutine CoroutCheck;

    void Awake()
    { 
        isDead = false;
        isDamaged = false;
        isWin = false;
        hasWon = false;
    }
    
    void Start()
    {   
        audioSource = gameObject.GetComponent<AudioSource>();
        
        // Sets player ui states
        RTDead.SetActive(false);
        RTPain.SetActive(false);
        
        // Lv intro transition
        Instantiate(LvLoad, Vector3.zero, Quaternion.identity);
        lvLoadTransition = GameObject.Find("LvLoad").GetComponent<Animator>();
        lvLoadTransition.Play("LvLoadIntro");

        // Sets health display
        healthDisplay = GameObject.Find("HealthDisplay").GetComponent<TMPro.TMP_Text>();
        // Gets player's health from previous level
        health = PlayerPrefs.GetFloat("PlayerHealth", 0);
        healthDisplay.text = $"{health}";
    }

    void Update()
    {
        // Checks if the player has won
        if (isWin) {
            PlayerAnimation.isWinning = false;
            if (!hasWon)
            {
                StartCoroutine(PlayerWin());
            }
        } 
    }
    
    // Takes damage from enemy
    private void OnTriggerEnter(Collider other)
    {
        if (!isDamaged && !isDead && !isWin)
        {
            // Check if the entering collider has the tag "Enemy"
            if (other.CompareTag("Enemy") && !isInvincible)
            {
                eHurt = other.GetComponent<EnemyHurt>();
                if (!eHurt.isHit)
                {
                    isDamaged = true;
                    eAtInfo = other.GetComponent<AttackInfo>();

                    if (health - eAtInfo.dmg < 0)
                        health = 0;
                    else
                        health -= eAtInfo.dmg;

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
        Instantiate(DeathLoad, Vector3.zero, Quaternion.identity);

        audioSource.PlayOneShot(vonLaugh);
        delay = vonLaugh.length; 
        // Waits until the laugh sfx plays
        yield return new WaitForSeconds(delay+.5f);

        // Restart screen + Main menu screen
        Instantiate(DeathUI, Vector3.zero, Quaternion.identity);
        
        yield return null;
    }
    private IEnumerator PlayerWin()
    {
        hasWon = true;
        audioSource.PlayOneShot(win);
        float delay = win.length; 
        // Waits until the player win sfx finishes playing
        yield return new WaitForSeconds(delay);
        
        // Lv outro transition
        lvLoadTransition.Play("LvLoadOutro");

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
