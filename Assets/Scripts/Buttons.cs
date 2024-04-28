using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Buttons
public class Buttons : MonoBehaviour
{
    [SerializeField]
    private GameObject LvLoadOutro;
    [SerializeField]
    private Animator lvLoadOutroTransition;

    void Start()
    {
        LvLoadOutro.SetActive(false);
    }
    public void RestartLvButton ()
    {
        // Resets player health
        PlayerState.health = 100;
        // Restarts level
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void MainMenuButton ()
    {
        // Resets player health
        PlayerState.health = 100;
        StartCoroutine(MainMenu());
    }

    public void StartGameButton ()
    {
        PlayerState.health = 100;
        StartCoroutine(StartLv());
    }

    private IEnumerator MainMenu()
    {
        LvLoadOutro.SetActive(true);

        // Lv outro transition
        LvLoadOutro.SetActive(true);
        lvLoadOutroTransition.speed = .8f;
        lvLoadOutroTransition.SetTrigger("Win");
    
        yield return new WaitForSeconds(2f);

        // Goes to Main Menu
        SceneManager.LoadScene(0);
    }

    private IEnumerator StartLv()
    {
        // Resets player health
        PlayerState.health = 100;

        LvLoadOutro.SetActive(true);

        // Lv outro transition
        LvLoadOutro.SetActive(true);
        lvLoadOutroTransition.speed = .8f;
        lvLoadOutroTransition.SetTrigger("Win");
    
        yield return new WaitForSeconds(2f);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex+1);
    }

    private void OnDisable()
    {
        // Saves player health for next lv
        PlayerPrefs.SetFloat("PlayerHealth", PlayerState.health);
        PlayerPrefs.Save();
    }
}
