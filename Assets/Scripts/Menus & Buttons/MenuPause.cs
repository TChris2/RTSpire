using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MenuPause : MonoBehaviour
{
    public static bool isPaused;
    private Animator PauseScreenAni;
    [SerializeField]
    private GameObject pMenuInitial;
    

    void Start()
    {
        PauseScreenAni = GameObject.Find("Pause Screen").GetComponent<Animator>();
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
                
                PauseScreenAni.Play("Pause");

                EventSystem.current.SetSelectedGameObject(pMenuInitial);

                InputManager.player.Disable();
                InputManager.menu.Enable();
            }
            else
            {
                Time.timeScale = 1;
                AudioListener.pause = false;
                isPaused = false;

                PauseScreenAni.Play("Unpause");

                InputManager.player.Enable();
                InputManager.menu.Disable();
            }
        }
    }
}
