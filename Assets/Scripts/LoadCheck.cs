using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Waits until all necessary loaded elements are loaded before playing the Lv transition at the start
public class LoadCheck : MonoBehaviour
{
    // Delay after all elements are loaded in
    public float loadDelay;
    Animator LoadAni;
    // Scripts
    VoiceClipSelectionButtons vClipBtns;
    TumbleDesignSelectionButtons tDesignBtns;
    InputManager inputManager;

    void Start()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;

        LoadAni = GetComponent<Animator>();
        vClipBtns = GameObject.Find("Voice Clip Menu").GetComponent<VoiceClipSelectionButtons>();
        tDesignBtns = GameObject.Find("Tumble Design Menu").GetComponent<TumbleDesignSelectionButtons>();
        inputManager = GameObject.Find("Player").GetComponent<InputManager>();

        StartCoroutine(LoadLoopCheck(vClipBtns, tDesignBtns, LoadAni));
    }

    // Loops until all necessary loaded elements are loaded
    IEnumerator LoadLoopCheck(VoiceClipSelectionButtons vClipBtns, TumbleDesignSelectionButtons tDesignBtns, Animator LoadAni)
    {
        while (true)
        {
            yield return null;

            if (tDesignBtns.isLoaded && vClipBtns.isLoaded)
                break;
        }

        yield return new WaitForSecondsRealtime(loadDelay);

        Time.timeScale = 1;
        AudioListener.pause = false;

        inputManager.player.Enable();

        // Plays Lv Intro transition
        LoadAni.Play("Lv Load Intro");
    }
}
