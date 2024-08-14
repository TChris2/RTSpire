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
    public bool isTMachineDestroyed;
    // Text
    private TMPro.TMP_Text DestroyCounter;
    private Animator TMAni; 

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
                isTMachineDestroyed = true;
                audioSource.PlayOneShot(hurtClips[1]);
                float delay = hurtClips[1].length; 

                TMAni.Play("TMachineDestroyed");
                
                // Updates counter
                ObjectiveDestroyMachine.TMachineCounter -= 1;
                DestroyCounter.text = $"{ObjectiveDestroyMachine.TMachineCounter}";

                // Checks to see if the player has destroyed all the machines
                if (ObjectiveDestroyMachine.TMachineCounter <= 0)
                    Invoke("Win", .9f);

                // Destroys machine
                Invoke("DestroyMachine", delay-1f);
            }
        }
    }

    // If the player has destroyed all machines
    void Win()
    {
        PlayerState.isWin = true;
    }

    // Destroys machine
    void DestroyMachine()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        //sets health
        Health = StartHealth;
        isTMachineDestroyed = false;
        audioSource = gameObject.GetComponentInChildren<AudioSource>();
        // Get destroy counter
        DestroyCounter = GameObject.Find("DestroyCounter").GetComponent<TMPro.TMP_Text>();
        DestroyCounter.text = $"{ObjectiveDestroyMachine.TMachineCounter}";
        TMAni = GetComponent<Animator>();
    }
}
