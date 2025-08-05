using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles health of enemies
public class Entity : MonoBehaviour
{
    // Enemy hurt sounds
    public AudioClip[] hurtClips; 
    private AudioSource audioSource;
    // Controls starting health for enemies
    [SerializeField]
    private float StartHealth = 5;
    // Keeps track of enemy health
    private float health;
    private Animator eAni;

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
                // If the enemy is not dead when hit
                if (health > 0f)
                {
                    audioSource.PlayOneShot(hurtClips[Random.Range(0, 3)]);   
                    eAni.Play("Enemy Hurt");
                }
                // When enemy health reaches zero and dies
                else if (health <= 0f)
                {
                    // Plays one of three hurt clips
                    int randomClipNum = Random.Range(1, 201);
                    if (randomClipNum > 15)
                    {
                        randomClipNum = Random.Range(0, 3);
                    }
                    else if (randomClipNum <= 15 && randomClipNum > 5)
                    {
                        randomClipNum = 3;
                    }
                    else if (randomClipNum <= 5)
                    {
                        randomClipNum = 4;
                    }

                    // Plays sfx
                    audioSource.PlayOneShot(hurtClips[randomClipNum]);
                    float delay = hurtClips[randomClipNum].length;

                    // Plays hurt animation and death fade
                    eAni.Play("Enemy Hurt");
                    eAni.Play("Death Fade");
                    // Destroys enemy after the sound is played
                    Invoke("DestroyEnemy", delay);
                }
            }
        }
    }

    // Destroys the enemy
    void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        // Sets start health for enemies
        Health = StartHealth;
        // Gets enemy components
        audioSource = gameObject.GetComponent<AudioSource>();
        eAni = GetComponent<Animator>();
    }
}
