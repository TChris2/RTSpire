using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

// Button functions for the pause screen menu
public class PauseMenuBaseButtons : MonoBehaviour
{
    // Animator for lv load object
    private Animator lvLoadAni;
    private MenuPause pause;

    [SerializeField]
    private CanvasGroup pScreenBase;
    [SerializeField]
    private CanvasGroup ynScreen;
    [SerializeField]
    private GameObject ynInitial;
    private GameObject pScreenPrev;
    [SerializeField]
    private TMPro.TMP_Text ynText;
    [SerializeField]
    private CanvasGroup opMenu;
    [SerializeField]
    private CanvasGroup gameMenu;
    [SerializeField]
    private CanvasGroup audioMenu;
    [SerializeField]
    private GameObject opInitial;
    // Decides what scene the game exits to
    // 1: Hub 2: Menu 3: Exits Game
    private float ynNext;

    void Start()
    {
        lvLoadAni = GameObject.Find("Lv Transition").GetComponent<Animator>();
    }

    // Resumes the game
    public void Resume()
    {
        pause = GameObject.Find("Player").GetComponent<MenuPause>();
        pause.Pause();
    }

    // Exits to hub
    public void ExitHub(GameObject prevBtn)
    {
        ynText.text = $"{"\nExit To Hub?"}";
        ynNext = 1;
        ynMenuOpen(prevBtn);
    }

    // Exits to main menu
    public void ExitMainMenu(GameObject prevBtn)
    {
        ynText.text = $"{"Exit To\nMain Menu?"}";
        ynNext = 2;
        ynMenuOpen(prevBtn);
    }

    // Exits the game
    public void ExitGame(GameObject prevBtn)
    {
        ynText.text = $"{"\nExit Game?"}";
        ynNext = 3;
        ynMenuOpen(prevBtn);
    }

    void ynMenuOpen(GameObject prevBtn) 
    {
        pScreenPrev = prevBtn;
        pScreenBase.interactable = false;
        pScreenBase.alpha = 0;
        pScreenBase.blocksRaycasts = false;

        EventSystem.current.SetSelectedGameObject(ynInitial);

        ynScreen.interactable = true;
        ynScreen.alpha = 1;
        ynScreen.blocksRaycasts = true;
    }

    public void OpMenuOpen(GameObject prevBtn) 
    {
        pScreenPrev = prevBtn;
        pScreenBase.interactable = false;
        pScreenBase.alpha = 0;

        // Ensures menu is disabled when the option menu is opened
        audioMenu.interactable = false;
        audioMenu.alpha = 0;
        audioMenu.blocksRaycasts = false;

        EventSystem.current.SetSelectedGameObject(opInitial);

        gameMenu.interactable = true;
        gameMenu.alpha = 1;
        gameMenu.blocksRaycasts = true;

        opMenu.interactable = true;
        opMenu.alpha = 1;
        opMenu.blocksRaycasts = true;
    }

    public void Yes()
    {
        ynScreen.interactable = false;
        StartCoroutine(Next());
    }

    IEnumerator Next()
    {
        // Lv outro transition
        lvLoadAni.speed = 1.5f;
        lvLoadAni.Play("LvOutro");
        // ---------------------------
        // ---------------------------
        // ---------------------------
        // Maybe make longer
        yield return new WaitForSecondsRealtime(1.5f);

        switch (ynNext)
        {
            // 1: Hub 
            case 1:
                Debug.Log("Does Not Have Functionality Currently");
                No();
                break;
            // 2: Menu
            case 2:
                SceneManager.LoadScene(0);
                break;
            // 3: Exits Game
            case 3:
                Application.Quit();
                break;
        }
    }

    public void No()
    {
        ynScreen.interactable = false;
        ynScreen.alpha = 0;
        ynScreen.blocksRaycasts = false;

        EventSystem.current.SetSelectedGameObject(pScreenPrev);

        pScreenBase.interactable = true;
        pScreenBase.alpha = 1;
        pScreenBase.blocksRaycasts = true;
    }

    private void OnDisable()
    {
        lvLoadAni.speed = 1;
        Time.timeScale = 1;
    }
}
