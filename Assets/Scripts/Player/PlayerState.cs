using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

// Controls player state and player ui alongside lv load transitions
public class PlayerState : MonoBehaviour
{
    // UI Anis
    private Animator deathUIAni;
    private Animator deathLoadAni;
    private Animator lvLoadAni;
    // Text
    private TMPro.TMP_Text healthDisplay;
    // Audio
    public AudioClip[] hurt; 
    public AudioClip vonLaugh; 
    public AudioClip win; 
    private AudioSource audioSource;
    // Health
    public float health = 100;
    public float hMax = 100;
    [SerializeField]
    private float eAttackCool = 2f;
    // Used in EnemySpawn, EnemyFollow
    public bool isDead = false;
    public bool isDamaged = false;
    // Used in EnemySpawn, EnemyFollow, VoiceClips
    public bool isWin = false;
    [SerializeField]
    private bool isInvincible;

    // Health UI Animator
    private Animator playerUIAni;
    private Animator playerAni;

    [SerializeField]
    private GameObject dMenuInitial;
    
    void Start()
    {   
        audioSource = GetComponent<AudioSource>();

        deathUIAni = GameObject.Find("Death UI").GetComponent<Animator>();
        deathLoadAni = GameObject.Find("Death Transition").GetComponent<Animator>();
        lvLoadAni = GameObject.Find("Lv Transition").GetComponent<Animator>();

        // Sets health display
        healthDisplay = GameObject.Find("HealthDisplay").GetComponent<TMPro.TMP_Text>();
        // Gets player's health from previous level
        health = PlayerPrefs.GetFloat("Player Health", hMax);
        healthDisplay.text = $"{health}";

        playerUIAni = GameObject.Find("Player UI").GetComponent<Animator>();
        playerAni = GetComponentInParent<Animator>();
    }
    
    // Takes damage from enemy
    private void OnTriggerEnter(Collider other)
    {
        // Checks to see if the player has already taken damage
        if (!isDamaged && !isDead && !isWin)
        {
            // Check if the object entering collider is an enemy
            if (other.CompareTag("Enemy") && !isInvincible)
            {
                EnemyHurt eHurt = other.GetComponent<EnemyHurt>();
                if (!eHurt.isHit)
                {
                    isDamaged = true;
                    AttackInfo eAtkInfo = other.GetComponent<AttackInfo>();

                    if (health - eAtkInfo.dmg < 0)
                        health = 0;
                    else
                        health -= eAtkInfo.dmg;

                    healthDisplay.text = $"{health}";

                    if (health > 0)
                        StartCoroutine(PlayerHurt(eAttackCool));
                    // Death
                    else if (health <= 0) {
                        isDead = true;
                        StartCoroutine(PlayerDead());
                    }
                }
            }
        }
    }

    public IEnumerator PlayerHurt(float cooldown)
    {
        // Changes UI
        playerUIAni.Play("Hurt");
        playerAni.Play("Player Hurt");

        audioSource.PlayOneShot(hurt[0]);
        // Gives a short time of invincibility to the player until they can get hit again
        yield return new WaitForSeconds(cooldown);

        // Changes UI back
        playerUIAni.Play("Normal");
        playerAni.SetTrigger("No Longer Invincible");

        isDamaged = false;
    }

    private IEnumerator PlayerDead()
    {
        AudioListener.pause = true;
        audioSource.ignoreListenerPause = true;
        playerAni.updateMode = AnimatorUpdateMode.UnscaledTime;
        Time.timeScale = 0;
        audioSource.PlayOneShot(hurt[1]);
        float delay = hurt[1].length; 
        
        playerUIAni.Play("Dead");
        playerAni.Play("Death");
        // Waits until the player dead sfx finishes playing
        yield return new WaitForSecondsRealtime(delay-1.2f);
        
        // Death load screen
        deathLoadAni.Play("DeathLoad");

        audioSource.PlayOneShot(vonLaugh);
        delay = vonLaugh.length; 
        // Waits until the laugh sfx plays
        yield return new WaitForSecondsRealtime(delay+.5f);

        // Restart screen + Main menu screen
        deathUIAni.Play("DeathUILoad");
        
        EventSystem.current.SetSelectedGameObject(dMenuInitial);
    }

    public IEnumerator PlayerWin()
    {
        isWin = true;
        AudioListener.pause = true;
        audioSource.ignoreListenerPause = true;
        playerAni.updateMode = AnimatorUpdateMode.UnscaledTime;
        Time.timeScale = 0;
        audioSource.PlayOneShot(win);
        float delay = win.length; 
        // Plays player win animation
        playerAni.Play("Win");
        // Waits until the player win sfx finishes playing
        yield return new WaitForSecondsRealtime(delay);
        
        // Lv outro transition
        lvLoadAni.Play("Lv Load Outro");

        yield return new WaitForSecondsRealtime(2f);
        // Next level
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex+1);

        yield return null;
    }

    private void OnDisable()
    {
        // Saves player health for next lv
        PlayerPrefs.SetFloat("Player Health", health);
        PlayerPrefs.Save();

        // Resets timescale & audiolistener before moving to the next scene
        Time.timeScale = 1;
        AudioListener.pause = false;
    }
}
