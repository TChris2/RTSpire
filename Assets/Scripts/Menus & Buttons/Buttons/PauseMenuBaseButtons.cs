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
    // Scripts
    private MenuPause pause;
    private OptionsMenuButtons opMenuBtns;
    // Menus
    [Header("Menus")]
    public CanvasGroup pScreenBase;
    public CanvasGroup ynScreen;
    [SerializeField]
    private CanvasGroup opMenu;
    [SerializeField]
    private CanvasGroup gameMenu;
    [SerializeField]
    private CanvasGroup audioMenu;
    [Header("Objects")]
    [SerializeField]
    private GameObject ynInitial;
    private GameObject pScreenPrev;
    [SerializeField]
    private TMPro.TMP_Text ynText;
    [SerializeField]
    private GameObject opInitial;
    // Decides what scene the game exits to
    // 1: Hub 2: Menu 3: Exits Game
    private float ynNext;

    void Start()
    {
        // Gets components
        lvLoadAni = GameObject.Find("Lv Transition").GetComponent<Animator>();
        pause = GameObject.Find("Player").GetComponent<MenuPause>();
        opMenuBtns = GameObject.Find("Options Menu").GetComponent<OptionsMenuButtons>();
    }

    // Resumes the game
    public void Resume()
    {
        pause.Resume();
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
        MenuOpenClose(pScreenBase, false);

        EventSystem.current.SetSelectedGameObject(ynInitial);

        MenuOpenClose(ynScreen, true);
    }

    public void OpMenuOpen(GameObject prevBtn)
    {
        pScreenPrev = prevBtn;
        MenuOpenClose(pScreenBase, false);

        opMenuBtns.GameOpMenu();

        EventSystem.current.SetSelectedGameObject(opInitial);

        MenuOpenClose(opMenu, true);
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
        lvLoadAni.Play("Lv Load Outro");
        // ---------------------------
        // ---------------------------
        // ---------------------------
        // Maybe make longer
        yield return new WaitForSecondsRealtime(1.5f);

        switch (ynNext)
        {
            // 1: Hub 
            case 1:
                Debug.Log("Don't forgot to change to hub scene when that exists");
                SceneManager.LoadScene(0);
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
        MenuOpenClose(ynScreen, false);

        EventSystem.current.SetSelectedGameObject(pScreenPrev);

        MenuOpenClose(pScreenBase, true);
    }

    // Opens or closes selected menus
    private void MenuOpenClose(CanvasGroup menu, bool isOpen)
    {
        menu.interactable = isOpen;
        menu.alpha = isOpen ? 1 : 0;
        menu.blocksRaycasts = isOpen;
    }

    private void OnDisable()
    {
        lvLoadAni.speed = 1;
        Time.timeScale = 1;
    }
}
