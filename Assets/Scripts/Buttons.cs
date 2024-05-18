using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Buttons
public class Buttons : MonoBehaviour
{
    [SerializeField]
    private GameObject LvLoad;
    private Animator lvLoadTransition;
    private Animator DeathUITransition;

    public void DeathRestartLvButton ()
    {
        // Resets player health
        PlayerState.health = 100;

        StartCoroutine(TransitionDeath(1));
    }

    public void DeathMainMenuButton ()
    {
        StartCoroutine(TransitionDeath(0));
    }

    public void MainMenuButton ()
    {
        StartCoroutine(Transition(0));
    }

    public void StartGameButton ()
    {
        PlayerState.health = 100;
        StartCoroutine(Transition(1));
    }

    private IEnumerator TransitionDeath(int state)
    {
        DeathUITransition = GameObject.Find("Death UI").GetComponent<Animator>();

        // Lv outro transition
        DeathUITransition.Play("DeathUIFade");
    
        yield return new WaitForSeconds(2f);

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

    private IEnumerator Transition(int state)
    {
        Instantiate(LvLoad, Vector3.zero, Quaternion.identity);
        lvLoadTransition = GameObject.Find("LvLoad").GetComponent<Animator>();

        // Lv outro transition
        lvLoadTransition.Play("LvLoadOutro");
        // ---------------------------
        // ---------------------------
        // ---------------------------
        // Maybe make longer
        yield return new WaitForSeconds(2f);

        // Goes to Main Menu
        if (state == 0)
        {
            SceneManager.LoadScene(0);
        }
        // Starts Game
        else if (state == 1)
        {
            // Sets player health to full
            PlayerState.health = 100;

            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex+1);
        }
    }

    private void OnDisable()
    {
        // Saves player health for next lv
        PlayerPrefs.SetFloat("PlayerHealth", PlayerState.health);
        PlayerPrefs.Save();
    }
}
