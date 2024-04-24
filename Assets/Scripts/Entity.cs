using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//controls health of objects and determines when they get destroyed
public class Entity : MonoBehaviour
{
    public AudioClip[] hurtClips; 
    private AudioSource audioSource;
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
            if (health != value && audioSource != null && health > 0f) {
                audioSource.PlayOneShot(hurtClips[0]);
            }
            health = value;
            //destroys objects when their health reaches 0
            if (health <= 0f)
            {
                int randomClipNum = Random.Range(0, 11);
                if (randomClipNum > 0)
                    randomClipNum = 0;
                else
                    randomClipNum = 1;
                audioSource.PlayOneShot(hurtClips[randomClipNum]);
                float delay = hurtClips[randomClipNum].length; 
                Invoke("DestroyEnemy", delay);
            }
        }
    }

    void DestroyEnemy()
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
