using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (AudioPlay == true && !PlayerState.isDead  && !PlayerState.isWin)
            StartCoroutine(RandomClip());
    }

    IEnumerator RandomClip ()
    {
        AudioPlay = false;
        int randomClipNum = Random.Range(0, 190);
        if (randomClipNum < 95)
            audioSource.PlayOneShot(voiceClips[randomClipNum]);
        yield return new WaitForSeconds(Random.Range(0, 60));
        AudioPlay = true;
    }
}
