using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
