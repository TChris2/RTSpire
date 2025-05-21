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
    // Auto Scrolls for each menu
    private ScrollRectAutoScroll audioOpAutoScroll;
    private ScrollRectAutoScroll gameOpAutoScroll;

    void Start()
    {
        opMenu = GetComponent<CanvasGroup>();
        gameOpAutoScroll = GameObject.Find("Game Options Scroll View").GetComponent<ScrollRectAutoScroll>();
        audioOpAutoScroll = GameObject.Find("Audio Options Scroll View").GetComponent<ScrollRectAutoScroll>();
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
        ScrollRect gameScrollRect = gameOpAutoScroll.GetComponent<ScrollRect>();
        gameScrollRect.verticalNormalizedPosition = 1;

        // Updates navigation
        MenuNavigation menuNav = gameOpAutoScroll.GetComponentInChildren<MenuNavigation>();
        menuNav.UpdateTopBarNavigation();

        gameMenu.interactable = true;
        gameMenu.alpha = 1;
        gameMenu.blocksRaycasts = true;
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
        ScrollRect audioScrollRect = audioOpAutoScroll.GetComponent<ScrollRect>();;
        audioScrollRect.verticalNormalizedPosition = 1;

        // Updates navigation
        MenuNavigation menuNav = audioOpAutoScroll.GetComponentInChildren<MenuNavigation>();
        menuNav.UpdateTopBarNavigation();

        audioMenu.interactable = true;
        audioMenu.alpha = 1;
        audioMenu.blocksRaycasts = true;
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
