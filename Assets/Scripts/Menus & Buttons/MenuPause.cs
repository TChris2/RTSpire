using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MenuPause : MonoBehaviour
{
    private InputManager inputManager;
    private PlayerState pState;
    private bool isPaused;
    [SerializeField]
    private GameObject pMenuInitial;
    public GameObject menuCurrent;
    [Header("Menus")]
    // Full Pause Screen
    [SerializeField]
    private CanvasGroup pScreenTotal;
    // Pause Screen Base - Holds pause screen btns
    [SerializeField]
    private CanvasGroup pScreenBase;
    // Menus
    [SerializeField]
    private CanvasGroup ynScreen;
    // Option Menu
    [SerializeField]
    private CanvasGroup opMenu;
    // Sub Option Menu Menus
    [SerializeField]
    private CanvasGroup subOpMenu;
    [SerializeField]
    private CanvasGroup subOpYNScreen;
    
    void Start()
    {
        isPaused = false;
        inputManager = GetComponent<InputManager>();
        pState = GetComponentInChildren<PlayerState>();
    }

    public void Pause()
    {
        if (!pState.isDead && !pState.isWin)
        {
            // If the game wasn't already paused
            if (!isPaused)
            {
                Time.timeScale = 0;
                AudioListener.pause = true;
                isPaused = true;
                
                EventSystem.current.SetSelectedGameObject(pMenuInitial);

                pScreenTotal.interactable = true;
                pScreenTotal.alpha = 1;
                pScreenTotal.blocksRaycasts = true;

                StartCoroutine(UIReselect());

                inputManager.player.Disable();
                inputManager.menu.Enable();
            }
            // If the game was already paused
            else
            {
                Time.timeScale = 1;
                AudioListener.pause = false;
                isPaused = false;

                // Closes all menus
                CloseMenus();

                inputManager.player.Enable();
                inputManager.menu.Disable();
            }
        }
    }
    
    IEnumerator UIReselect ()
    {
        // Only active while pause menu is
        while (isPaused) 
        {
            if (EventSystem.current.currentSelectedGameObject != null) 
                menuCurrent = EventSystem.current.currentSelectedGameObject;
            else
                EventSystem.current.SetSelectedGameObject(menuCurrent);

            yield return null;
        }

        // Resets selected object
        menuCurrent = pMenuInitial;
    }

    void CloseMenus()
    {   
        // Disables the pause screen
        pScreenTotal.interactable = false;
        pScreenTotal.alpha = 0;
        pScreenTotal.blocksRaycasts = false;

        // Resets back to the pause screen
        pScreenBase.interactable = true;
        pScreenBase.alpha = 1;
        pScreenTotal.blocksRaycasts = true;

        // Closes sub menus if they are open
        ynScreen.interactable = false;
        ynScreen.alpha = 0;
        ynScreen.blocksRaycasts = false;

        // Closes option menus
        opMenu.interactable = false;
        opMenu.alpha = 0;
        opMenu.blocksRaycasts = false;

        // Closes sub option menus
        subOpMenu.interactable = false;
        subOpMenu.alpha = 0;
        subOpMenu.blocksRaycasts = false;

        subOpYNScreen.interactable = false;
        subOpYNScreen.alpha = 0;
        subOpYNScreen.blocksRaycasts = false;
    }
}
