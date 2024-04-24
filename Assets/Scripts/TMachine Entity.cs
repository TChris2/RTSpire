using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMachineEntity : MonoBehaviour
{
    [SerializeField]
    private GameObject mOpen;
    [SerializeField]
    private GameObject mClosed;
    public AudioClip[] hurtClips; 
    private AudioSource audioSource;
    [SerializeField]
    private float StartHealth = 5;
    private float health;
    public bool isTMachineDestroyed = false;

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            if (health != value && audioSource != null && health > 0f) {
                audioSource.PlayOneShot(hurtClips[0]);
            }
            health = value;
            //destroys objects when their health reaches 0
            if (health <= 0f)
            {
                isTMachineDestroyed = true;
                audioSource.PlayOneShot(hurtClips[1]);
                float delay = hurtClips[1].length; 
                mOpen.SetActive(false);
                mClosed.SetActive(false);
                Invoke("DestroyMachine", delay);
            }
        }
    }

    void DestroyMachine()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        //sets health
        Health = StartHealth;
        audioSource = gameObject.GetComponent<AudioSource>();
    }
}
