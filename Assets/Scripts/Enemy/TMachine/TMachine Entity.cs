using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles health of TMachines
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
    [HideInInspector]
    public bool isTMachineDestroyed = false;
    // Destroy counter components
    private GameObject DestroyCounter;
    private ObjectiveDestroyMachine objDestroy;
    // Machine counter text
    private TMPro.TMP_Text DestroyCounterTxt;
    // TMachine animator
    private Animator TMAni; 

    void Start()
    {
        // Sets intial health
        Health = StartHealth;
        // Gets comps from t machine
        TMAni = GetComponent<Animator>();
        audioSource = gameObject.GetComponentInChildren<AudioSource>();
        // Get comps from destroy counter
        DestroyCounter = GameObject.Find("DestroyCounter");
        objDestroy = DestroyCounter.GetComponent<ObjectiveDestroyMachine>();
        DestroyCounterTxt = DestroyCounter.GetComponent<TMPro.TMP_Text>();
    }
    
    // Updates health
    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;

            if (audioSource != null)
            {
                // If the TMachine is not dead when hit
                if (audioSource != null && health > 0f)
                {
                    // Plays hurt clip
                    audioSource.PlayOneShot(hurtClips[0]);

                    // Plays hit effect
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
                    objDestroy.WinCheck();

                    // Destroys machine
                    Invoke("DestroyMachine", delay - 1f);
                }
            }
        }
    }

    // Destroys machine
    void DestroyMachine()
    {
        Destroy(gameObject);
    }
}
