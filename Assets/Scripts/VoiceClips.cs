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
        if (AudioPlay == true && !PlayerState.isDead)
            StartCoroutine(RandomClip());
    }

    IEnumerator RandomClip ()
    {
        AudioPlay = false;
        int randomClipNum = Random.Range(0, 101);
        if (randomClipNum < 52)
            audioSource.PlayOneShot(voiceClips[randomClipNum]);
        yield return new WaitForSeconds(Random.Range(0, 60));
        AudioPlay = true;
    }
}
