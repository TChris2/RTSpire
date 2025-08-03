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
            // If the enemy is not dead when hit
            if (audioSource != null && health > 0f)
            {
                audioSource.PlayOneShot(hurtClips[0]);
                float delay = hurtClips[0].length;
                eAni.Play("Enemy Hurt");
            }
            // When enemy health reaches zero and dies
            else if (audioSource != null && health <= 0f)
            {
                // Plays one of three hurt clips
                int randomClipNum = Random.Range(1, 201);
                if (randomClipNum > 20)
                {
                    randomClipNum = 0;
                }
                else if (randomClipNum <= 20 && randomClipNum > 5)
                {
                    randomClipNum = 1;
                }
                else if (randomClipNum <= 5)
                {
                    randomClipNum = 2;
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
