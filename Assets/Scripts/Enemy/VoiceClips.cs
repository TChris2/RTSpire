using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Plays a random sfx
public class VoiceClips : MonoBehaviour
{
    // Gets list of voice clips
    private VoiceClipSelectionMenuButtons vcMenu;
    // Gets voice clip variables
    private TumbleAudioSettings tumbleAudio;
    private AudioSource audioSource;
    bool audioPlay;
    // Toggle to play enemy voice clips
    private Toggle clipPlayToggle;

    void Start()
    {
        audioPlay = true;
        audioSource = gameObject.GetComponent<AudioSource>();
        // Gets voice clip variables
        tumbleAudio = GameObject.Find("Audio Options Scroll View").GetComponent<TumbleAudioSettings>();
        // Gets list of voice clips
        vcMenu = GameObject.Find("Enemy Voice Clip Menu Scroll View").GetComponent<VoiceClipSelectionMenuButtons>();
        // Gets toggle determining if voice clips play or not
        clipPlayToggle = GameObject.Find("Enemy Voice Clip Toggle Button").GetComponent<Toggle>();

        if (clipPlayToggle.isOn && tumbleAudio.yapRate != 0 && vcMenu.clipList.Count != 0)
            StartCoroutine(RandomClipStart());
    }
   
    void Update()
    {
        // If a sfx isn't already playing and the player has not won or died
        if (clipPlayToggle.isOn && tumbleAudio.yapRate != 0 && vcMenu.clipList.Count != 0 && audioPlay == true)
            StartCoroutine(RandomClip());
    }

    // Plays a random sfx
    IEnumerator RandomClip()
    {
        // Set to false to prevent multiple sfxs from playing at the same time
        audioPlay = false;

        // Sets a random delay to prevent sfxs from all playing at the same time
        yield return new WaitForSeconds(Random.Range(tumbleAudio.minDelay, tumbleAudio.maxDelay + 1));

        // Checks to make sure any of the conditions have changed during the delay
        if (clipPlayToggle.isOn && tumbleAudio.yapRate != 0 && vcMenu.clipList.Count != 0)
        {
            int randomNum = Random.Range(1, 101);
            if (tumbleAudio.yapRate == 100 || randomNum <= tumbleAudio.yapRate)
            {
                randomNum = Random.Range(0, vcMenu.clipList.Count);
                audioSource.PlayOneShot(vcMenu.clipList[randomNum]);
                
                AudioClip randomVC = vcMenu.clipList[randomNum];
                float clipLength = randomVC.length;
                yield return new WaitForSeconds(clipLength);
            }
            
            audioPlay = true;
        }
    }

    // Plays a random clip when the enemy is spawned in
    IEnumerator RandomClipStart()
    {
        // Set to false to prevent multiple sfxs from playing at the same time
        audioPlay = false;
        
        int randomNum = Random.Range(1, 101);
        if (tumbleAudio.yapRate == 100 || randomNum <= tumbleAudio.yapRate)
        {
            randomNum = Random.Range(0, vcMenu.clipList.Count);
            audioSource.PlayOneShot(vcMenu.clipList[randomNum]);
            
            AudioClip randomVC = vcMenu.clipList[randomNum];
            float clipLength = randomVC.length;
            yield return new WaitForSeconds(clipLength);
        }
        
        // Sets a random delay to prevent sfxs from all playing at the same time
        yield return new WaitForSeconds(Random.Range(tumbleAudio.minDelay, tumbleAudio.maxDelay + 1));
        audioPlay = true;
    }
}
