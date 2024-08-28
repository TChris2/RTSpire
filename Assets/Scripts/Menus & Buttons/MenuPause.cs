using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MenuPause : MonoBehaviour
{
    public static bool isPaused;
    private CanvasGroup pScreenTotal;
    private CanvasGroup pScreenBase;
    private GameObject pMenuInitial;
    private GameObject pMenuCurrent;
    

    void Start()
    {
        pScreenTotal = GameObject.Find("Pause Screen").GetComponent<CanvasGroup>();
        pScreenBase = GameObject.Find("Pause Screen Base Layer").GetComponent<CanvasGroup>();
        pMenuInitial = GameObject.Find("Resume Button");
    }

    public void Pause()
    {
        if (!PlayerState.isDead && !PlayerState.isWin)
        {
            if (!isPaused)
            {
                Time.timeScale = 0;
                AudioListener.pause = true;
                isPaused = true;
                
                pScreenTotal.interactable = true;
                pScreenTotal.alpha = 1;

                EventSystem.current.SetSelectedGameObject(pMenuInitial);
                StartCoroutine(UIReselect());

                InputManager.player.Disable();
                InputManager.menu.Enable();
            }
            else
            {
                Time.timeScale = 1;
                AudioListener.pause = false;
                isPaused = false;

                pScreenTotal.interactable = false;
                pScreenTotal.alpha = 0;

                InputManager.player.Enable();
                InputManager.menu.Disable();
            }
        }
    }

    // Add thing to check which submenu it currently is in
    IEnumerator UIReselect ()
    {
        // Only active while pause menu is
        while (isPaused) 
        {
            if (EventSystem.current.currentSelectedGameObject != null) 
                pMenuCurrent = EventSystem.current.currentSelectedGameObject;
            else
                EventSystem.current.SetSelectedGameObject(pMenuCurrent);

            yield return null;
        }

        // Resets selected object
        pMenuCurrent = pMenuInitial;
    }
}
