using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Button functions for the top buttons of the option menu
public class OptionsMenuButtons : MonoBehaviour
{
    [Header("Menus")]
    // Main op menu
    public CanvasGroup opMenu;
    [SerializeField]
    // Main pause screen
    private CanvasGroup pScreenBase;
    // Content of audio menu
    [SerializeField]
    private CanvasGroup audioMenu;
    // Content of game menu
    [SerializeField]
    private CanvasGroup gameMenu;
    [Header("Objects")]
    [SerializeField]
    private GameObject prevScreenOpBtn;
    // Auto Scrolls for each menu
    private ScrollRectAutoScroll audioOpAutoScroll;
    private ScrollRectAutoScroll gameOpAutoScroll;

    void Start()
    {
        opMenu = GetComponent<CanvasGroup>();
        gameOpAutoScroll = GameObject.Find("Game Options Menu").GetComponent<ScrollRectAutoScroll>();
        audioOpAutoScroll = GameObject.Find("Audio Options Menu").GetComponent<ScrollRectAutoScroll>();
    }

    // Opens game menu content
    public void GameOpMenu()
    {
        MenuOpenClose(audioMenu, false);
        audioOpAutoScroll.isMenuOpen = false;

        // Enables auto scroll
        gameOpAutoScroll.isMenuOpen = true;
        StartCoroutine(gameOpAutoScroll.AutoScroll());

        // Resets scroll view
        ScrollRect gameScrollRect = gameOpAutoScroll.GetComponent<ScrollRect>();
        gameScrollRect.verticalNormalizedPosition = 1;

        // Updates navigation
        MenuNavigation menuNav = gameOpAutoScroll.GetComponentInChildren<MenuNavigation>();
        menuNav.UpdateTopBarNavigation();

        MenuOpenClose(gameMenu, true);        
    }

    // Opens audio menu content
    public void AudioOpMenu()
    {
        MenuOpenClose(gameMenu, false);
        gameOpAutoScroll.isMenuOpen = false;

        // Enables auto scroll
        audioOpAutoScroll.isMenuOpen = true;
        StartCoroutine(audioOpAutoScroll.AutoScroll());

        // Resets scroll view
        ScrollRect audioScrollRect = audioOpAutoScroll.GetComponent<ScrollRect>(); ;
        audioScrollRect.verticalNormalizedPosition = 1;

        // Updates navigation
        MenuNavigation menuNav = audioOpAutoScroll.GetComponentInChildren<MenuNavigation>();
        menuNav.UpdateTopBarNavigation();

        MenuOpenClose(audioMenu, true);
    }

    // Closes op menu and reopens main pause screen menu
    public void ExitOpMenu()
    {
        // Turns off auto scroll
        audioOpAutoScroll.isMenuOpen = false;
        gameOpAutoScroll.isMenuOpen = false;

        MenuOpenClose(opMenu, false);

        MenuOpenClose(gameMenu, true);

        MenuOpenClose(audioMenu, false);

        EventSystem.current.SetSelectedGameObject(prevScreenOpBtn);

        MenuOpenClose(pScreenBase, true);
    }
    
    // Opens or closes selected menus
    private void MenuOpenClose(CanvasGroup menu, bool isOpen)
    {
        menu.interactable = isOpen;
        menu.alpha = isOpen ? 1 : 0;
        menu.blocksRaycasts = isOpen;
    }
}
