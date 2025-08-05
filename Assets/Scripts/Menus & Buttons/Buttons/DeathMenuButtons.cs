using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Button functions for the death menu
public class DeathMenuButtons : MonoBehaviour
{
    // Animator for lv load object
    private Animator lvLoadAni;
    // Animator for death ui
    private Animator deathUIAni;
    private PlayerState pState;

    void Start()
    {
        pState = GameObject.Find("Player Hitbox").GetComponent<PlayerState>();
    }

    // When the player dies and selects the restart option
    public void LvRestart()
    {
        // Resets player health
        pState.health = pState.hMax;

        StartCoroutine(TransitionDeath(1));
    }

    // When the player dies and selects main menu
    public void DeathMainMenu()
    {
        StartCoroutine(TransitionDeath(0));
    }

    // When the player beats the game and goes back to the main menu
    public void MainMenu()
    {
        StartCoroutine(Transition(0));
    }

    // Opening screen when the player presses start
    public void StartGame()
    {
        pState.health = pState.hMax;
        StartCoroutine(Transition(1));
    }

    // Does fade load to death UI to next scene
    private IEnumerator TransitionDeath(int state)
    {
        deathUIAni = GameObject.Find("Death UI").GetComponent<Animator>();
        // Lv outro transition
        deathUIAni.Play("DeathUIFade");
        yield return new WaitForSecondsRealtime(2f);

        // Goes to Main Menu
        if (state == 0)
        {
            SceneManager.LoadScene(0);
        }
        // Restarts level
        else if (state == 1)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }

    // Transitions to main menu or start of game
    private IEnumerator Transition(int state)
    {
        lvLoadAni = GameObject.Find("Lv Transition").GetComponent<Animator>();

        // Lv outro transition
        lvLoadAni.Play("Lv Load Outro");
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
            // Sets player health to full
            pState.health = pState.hMax;

            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex+1);
        }
    }

    private void OnDisable()
    {
        // Saves prefs
        // Saves player health for next lv
        PlayerPrefs.SetFloat("PlayerHealth", pState.health);
        PlayerPrefs.Save();
    }
}
