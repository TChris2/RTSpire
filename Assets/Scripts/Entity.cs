using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls health of enemies
public class Entity : MonoBehaviour
{
    // Enemy hurt sounds
    public AudioClip[] hurtClips; 
    private AudioSource audioSource;
    // Controls starting health for enemies
    [SerializeField]
    private float StartHealth = 5;
    private float health;

    public float Health
    {
        get
        {
            return health;
        }
        set
        {   
            // Plays clip when enemy is hurt
            if (health != value && health > 0f) {
                audioSource.PlayOneShot(hurtClips[0]);
            }
            health = value;
            // When enemy health reaches 0
            if (health <= 0f)
            {
                // Has a 1 in 10 chance to play the scream
                int randomClipNum = Random.Range(0, 11);
                if (randomClipNum > 0)
                    randomClipNum = 0;
                else
                    randomClipNum = 1;
                audioSource.PlayOneShot(hurtClips[randomClipNum]);
                float delay = hurtClips[randomClipNum].length; 
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
        // Sets health
        Health = StartHealth;
        audioSource = gameObject.GetComponent<AudioSource>();
    }
}
