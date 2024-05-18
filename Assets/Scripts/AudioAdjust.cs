using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAdjust : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls health of enemies
public class Entity : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent enemy;
    private CharacterController controller;
    // Enemy hurt sounds
    public AudioClip[] hurtClips; 
    private AudioSource audioSource;
    // Controls starting health for enemies
    [SerializeField]
    private float StartHealth = 5;
    private float health;
    private float healthCheck;

    public float Health
    {
        get
        {
            return health;
        }
        set
        {   
            healthCheck = health;
            health = value;
            // Plays clip when enemy is hurt
            if (audioSource != null && health != healthCheck && health > 0f) {
                audioSource.PlayOneShot(hurtClips[0]);
                float delay = hurtClips[0].length; 
                enemy.enabled = false;
                controller.enabled = false; 
                Invoke("MovementOn", delay);
            }
            // When enemy health reaches 0
            if (audioSource != null && health <= 0f)
            {
                // Has a 1 in 10 chance to play the scream
                int randomClipNum = Random.Range(0, 11);
                if (randomClipNum > 0)
                    randomClipNum = 0;
                else
                    randomClipNum = 1;
                audioSource.PlayOneShot(hurtClips[randomClipNum]);
                float delay = hurtClips[randomClipNum].length; 
                enemy.isStopped = true;
                // Destroys enemy after the sound is played
                Invoke("DestroyEnemy", delay);
            }
        }
    }

    // Turns back on movemeny
    void MovementOn()
    {
        controller.enabled = true; 
        enemy.enabled = true;
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
        enemy = GetComponent<UnityEngine.AI.NavMeshAgent>();
        controller = GetComponent<CharacterController>();
    }
}

    */
}
