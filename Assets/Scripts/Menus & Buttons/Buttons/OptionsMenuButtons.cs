using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Button functions for the top buttons of the option menu
public class OptionsMenuButtons : MonoBehaviour
{
    // Main op menu
    private CanvasGroup opMenu;
    [SerializeField]
    // Main pause screen
    private CanvasGroup pScreenBase;
    // Content of audio menu
    [SerializeField]
    private CanvasGroup audioMenu;
    // Content of game menu
    [SerializeField]
    private CanvasGroup gameMenu;
    [SerializeField]
    private GameObject prevScreenOpBtn;

    // For updating navigation of buttons
    [SerializeField]
    private Button gameMenuBtn;
    [SerializeField]
    private Button audioMenuBtn;
    [SerializeField]
    private Button exitMenuBtn;
    // Intitial button nav for that menu - 1st is Game 2nd is Audio 
    [SerializeField]
    private Selectable[] menuBtnNav;
    // For reseting ScrollRects when a menu is opened
    [SerializeField]
    private ScrollRect audioScrollRect;
    [SerializeField]
    private ScrollRect gameScrollRect;
    // Auto Scrolls
    [SerializeField]
    private ScrollRectAutoScroll audioOpAutoScroll;
    [SerializeField]
    private ScrollRectAutoScroll gameOpAutoScroll;

    void Start()
    {
        opMenu = GetComponent<CanvasGroup>();
    }

    // Opens game menu content
    public void GameOpMenu()
    {
        audioMenu.interactable = false;
        audioMenu.alpha = 0;
        audioMenu.blocksRaycasts = false;
        audioOpAutoScroll.isMenuOpen = false;

        // Enables auto scroll
        gameOpAutoScroll.isMenuOpen = true;
        StartCoroutine(gameOpAutoScroll.AutoScroll());
        // Resets scroll view
        gameScrollRect.verticalNormalizedPosition = 1;


        gameMenu.interactable = true;
        gameMenu.alpha = 1;
        gameMenu.blocksRaycasts = true;

        UpdateBtnNav(0);
    }

    // Opens audio menu content
    public void AudioOpMenu()
    {
        gameMenu.interactable = false;
        gameMenu.alpha = 0;
        gameMenu.blocksRaycasts = false;
        gameOpAutoScroll.isMenuOpen = false;

        // Enables auto scroll
        audioOpAutoScroll.isMenuOpen = true;
        StartCoroutine(audioOpAutoScroll.AutoScroll());
        // Resets scroll view
        audioScrollRect.verticalNormalizedPosition = 1;

        audioMenu.interactable = true;
        audioMenu.alpha = 1;
        audioMenu.blocksRaycasts = true;

        UpdateBtnNav(1);
    }

    void UpdateBtnNav(int navMenu)
    {
        Navigation gameMenuBtnNav = gameMenuBtn.navigation;
        gameMenuBtnNav.selectOnDown = menuBtnNav[navMenu];

        Navigation audioMenuBtnNav = audioMenuBtn.navigation;
        audioMenuBtnNav.selectOnDown = menuBtnNav[navMenu];

        Navigation exitMenuBtnNav = exitMenuBtn.navigation;
        exitMenuBtnNav.selectOnDown = menuBtnNav[navMenu];

        gameMenuBtn.navigation = gameMenuBtnNav;
        audioMenuBtn.navigation = audioMenuBtnNav;
        exitMenuBtn.navigation = exitMenuBtnNav;
    }

    // Closes op menu and reopens main pause screen menu
    public void ExitOpMenu()
    {
        // Turns off auto scroll
        audioOpAutoScroll.isMenuOpen = false;
        gameOpAutoScroll.isMenuOpen = false;

        opMenu.interactable = false;
        opMenu.alpha = 0;
        opMenu.blocksRaycasts = false;

        EventSystem.current.SetSelectedGameObject(prevScreenOpBtn);

        pScreenBase.interactable = true;
        pScreenBase.alpha = 1;
    }
}
