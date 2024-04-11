using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelReload : MonoBehaviour
{   
    public TMPro.TMP_Text bestTimeText;
    private float bestTime = 999999999;

    public AudioClip audioClip; 
    private AudioSource audioSource;

    void Awake()
    {   
        //gets audio
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        
        bestTime = PlayerPrefs.GetFloat("BestTime", 999999999);
        //sets best time after each run 
        bestTimeText.text = $"Best Time: {bestTime}s";
    }

    private void OnTriggerEnter(Collider other)
    {   
        //checks if player has entered
        if (other.CompareTag("Player"))
        {   
            if (Timer.timer < bestTime)
            {
                bestTime = Timer.timer;
            }

            //plays win noise and waits until it has played to reload
            audioSource.PlayOneShot(audioClip);
            float delay = audioClip.length; 
            Debug.Log(delay);
            Invoke("LoadNextScene", delay-4.5f);
        }
    }

    void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    private void OnDisable()
    {
        //saves best time
        PlayerPrefs.SetFloat("BestTime", bestTime);
        PlayerPrefs.Save();
    }
}
