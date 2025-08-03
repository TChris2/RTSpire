using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MenuPause : MonoBehaviour
{
    private bool isPaused;
    [SerializeField]
    private GameObject pMenuInitial;
    public GameObject menuCurrent;
    [Header("Menus")]
    // Full Pause Screen
    [SerializeField]
    private CanvasGroup pScreenTotal;
    // Sub Option Menu Menus
    [SerializeField]
    private CanvasGroup subOpMenu;
    [SerializeField]
    private CanvasGroup subOpYNScreen;
    // Scripts
    private InputManager inputManager;
    private PlayerState pState;
    private SubOptionMenuButtons subOpMenuBtns;
    private PauseMenuBaseButtons pMenuBaseBtns;
    private OptionsMenuButtons opMenuBtns;


    void Start()
    {
        isPaused = false;
        inputManager = GetComponent<InputManager>();
        pState = GetComponentInChildren<PlayerState>();
        subOpMenuBtns = FindObjectOfType<SubOptionMenuButtons>();
        pMenuBaseBtns = FindObjectOfType<PauseMenuBaseButtons>();
        opMenuBtns = FindObjectOfType<OptionsMenuButtons>();
    }

    // Pauses the game
    public void Pause()
    {
        // Stops player from pausing the game if they had died or won
        if (!pState.isDead && !pState.isWin)
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
    }

    // Resumes the game if the player is not in a sub menu
    public void Resume()
    {
        // Closes sub menu player is in currently
        if (!pMenuBaseBtns.pScreenBase.interactable)
            CloseMenus();
        // Resumes the game
        else
        {
            // Disables the pause screen
            pScreenTotal.interactable = false;
            pScreenTotal.alpha = 0;
            pScreenTotal.blocksRaycasts = false;

            inputManager.player.Enable();
            inputManager.menu.Disable();

            Time.timeScale = 1;
            AudioListener.pause = false;
        }
    }

    // Closes sub menu player is in currently
    void CloseMenus()
    {
        // Closes sub type menu of sub option menu
        if (subOpMenuBtns.isSubTypeMenuOpen)
        {
            // Debug.Log("Closing Sub Type Menu");
            subOpMenuBtns.CloseSubTypeMenu();
        }

        // Closes sub option menu yes no screen
        else if (subOpMenuBtns.ynScreen.interactable)
        {
            // Debug.Log("Closing Sub Option Menu Yes No Screen");
            subOpMenuBtns.No();
        }

        // Closes sub option menus
        else if (subOpMenuBtns.subOpMenu.interactable)
        {
            // Debug.Log("Closing Sub Option Menu");
            subOpMenuBtns.CloseSubOpMenu();
        }

        // Closes option menu
        else if (opMenuBtns.opMenu.interactable)
        {
            // Debug.Log("Closing Option Menu");
            opMenuBtns.ExitOpMenu();
        }

        // Closes pause menu yes no screen
        else if (pMenuBaseBtns.ynScreen.interactable)
        {
            // Debug.Log("Closing Pause Screen Yes No Screen");
            pMenuBaseBtns.No();
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
}
