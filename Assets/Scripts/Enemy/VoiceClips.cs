using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Plays a random sfx
public class VoiceClips : MonoBehaviour
{
    public AudioClip[] voiceClips; 
    private AudioSource audioSource;
    bool AudioPlay;

    void Start()
    {
        AudioPlay = true;
        audioSource = gameObject.GetComponent<AudioSource>();
    }
   
    void Update()
    {
        // If a sfx isn't already playing and the player has not won or died
        if (AudioPlay == true && !PlayerState.isDead  && !PlayerState.isWin)
            StartCoroutine(RandomClip());
    }

    // Plays a random sfx
    IEnumerator RandomClip ()
    {
        // Set to false to prevent multiple sfxs from playing at the same time
        AudioPlay = false;
        // Has a 50% chance to play a sfx
        int randomClipNum = Random.Range(0, 189);
        if (randomClipNum < 95)
            audioSource.PlayOneShot(voiceClips[randomClipNum]);
            
        // Sets a random delay to prevent sfxs from all playing at the same time
        yield return new WaitForSeconds(Random.Range(0, 60));
        AudioPlay = true;
    }
}
