using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Button functions for the start menu
public class StartMenuButtons : MonoBehaviour
{
    // Animator for lv load object
    private Animator lvLoadAni;
    // Animator for death ui
    private Animator deathUIAni;
    float healthRefill = 100;

    // Opening screen when the player presses start
    public void StartGame()
    {
        StartCoroutine(Transition(1));
    }

    // When the player beats the game and goes back to the main menu
    public void MainMenu()
    {
        StartCoroutine(Transition(0));
    }

    // Transitions to main menu or start of game
    private IEnumerator Transition(int state)
    {
        lvLoadAni = GameObject.Find("Lv Transition").GetComponent<Animator>();

        // Lv outro transition
        lvLoadAni.Play("LvOutro");
        // ---------------------------
        // ---------------------------
        // ---------------------------
        // Maybe make longer
        yield return new WaitForSecondsRealtime(2f);

        // Goes to Main Menu
        if (state == 0)
        {
            SceneManager.LoadScene(0);
        }
        // Starts Game
        else if (state == 1)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex+1);
        }
    }

    private void OnDisable()
    {
        // Saves player health for next lv
        PlayerPrefs.SetFloat("PlayerHealth", healthRefill);
        PlayerPrefs.Save();
    }
}
