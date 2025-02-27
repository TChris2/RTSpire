using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls health of TMachines
public class TMachineEntity : MonoBehaviour
{
    // Stores hurt clips
    public AudioClip[] hurtClips; 
    private AudioSource audioSource;
    // Start health of enemy
    [SerializeField]
    private float StartHealth = 5;
    // Tracks health of enemy
    private float health;
    // Checks to see if the machine has reached 0 health
    public bool isTMachineDestroyed;
    private GameObject DestroyCounter;
    private ObjectiveDestroyMachine objDestroy;
    // Machine counter text
    private TMPro.TMP_Text DestroyCounterTxt;
    private Animator TMAni; 
    private PlayerState pState;

    void Start()
    {
        // Sets health
        Health = StartHealth;
        isTMachineDestroyed = false;
        audioSource = gameObject.GetComponentInChildren<AudioSource>();
        // Get destroy counter
        DestroyCounter = GameObject.Find("DestroyCounter");
        objDestroy = DestroyCounter.GetComponent<ObjectiveDestroyMachine>();
        DestroyCounterTxt = DestroyCounter.GetComponent<TMPro.TMP_Text>();
        DestroyCounterTxt.text = $"{objDestroy.TMachineCounter}";
        TMAni = GetComponent<Animator>();

        pState = GameObject.Find("Player Hitbox").GetComponent<PlayerState>();
    }
    
    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if (audioSource != null && health > 0f) 
            {
                // Plays hurt clip
                audioSource.PlayOneShot(hurtClips[0]);
                TMAni.Play("TMachineHurt");
            }
            // Destroys machine when their health reaches 0
            else if (audioSource != null && health <= 0f && !isTMachineDestroyed)
            {
                // Sets to true to prevent it from playing multiple times
                isTMachineDestroyed = true;
                audioSource.PlayOneShot(hurtClips[1]);
                float delay = hurtClips[1].length; 

                // Plays destroyed animation
                TMAni.Play("TMachineDestroyed");
                
                // Updates counter
                objDestroy.TMachineCounter -= 1;
                DestroyCounterTxt.text = $"{objDestroy.TMachineCounter}";

                // Checks to see if the player has destroyed all the machines
                if (objDestroy.TMachineCounter <= 0)
                    Invoke("Win", .9f);

                // Destroys machine
                Invoke("DestroyMachine", delay-1f);
            }
        }
    }

    // If the player has destroyed all machines
    void Win()
    {
        pState.isWin = true;
    }

    // Destroys machine
    void DestroyMachine()
    {
        Destroy(gameObject);
    }
}
