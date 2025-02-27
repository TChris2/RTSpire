using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ensures audiosource with the start screen scene music plays
public class AudioStartScreen : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        AudioListener.pause = false;
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.ignoreListenerPause = true;
    }
}
