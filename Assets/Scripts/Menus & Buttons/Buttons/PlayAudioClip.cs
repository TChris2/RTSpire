using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Plays an audio clip
public class PlayAudioClip : MonoBehaviour
{
    [SerializeField]
    private AudioClip clip;
    [SerializeField]
    private AudioSource audioSource;

    public void PlayClip()
    {
        audioSource.PlayOneShot(clip);
    }
}
