using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls health of TMachines
public class TMachineEntity : MonoBehaviour
{
    public AudioClip[] hurtClips; 
    private AudioSource audioSource;
    [SerializeField]
    private float StartHealth = 5;
    private float health;
    public bool isTMachineDestroyed;
    // Text
    private TMPro.TMP_Text DestroyCounter;

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if (audioSource != null && health > 0f) {
                audioSource.PlayOneShot(hurtClips[0]);
            }
            //destroys objects when their health reaches 0
            else if (audioSource != null && health <= 0f && !isTMachineDestroyed)
            {
                isTMachineDestroyed = true;
                audioSource.PlayOneShot(hurtClips[1]);
                float delay = hurtClips[1].length; 
                
                Invoke("UpdateCounter", 1f);
                Invoke("DestroyMachine", delay-1f);
            }
        }
    }

    void UpdateCounter()
    {
        // Updates counter
        ObjectiveDestroyMachine.TMachineCounter -= 1;
        DestroyCounter.text = $"{ObjectiveDestroyMachine.TMachineCounter}";
    }

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
        DestroyCounter = GameObject.Find("DestroyCounter").GetComponent<TMPro.TMP_Text>();
        DestroyCounter.text = $"{ObjectiveDestroyMachine.TMachineCounter}";
    }
}
