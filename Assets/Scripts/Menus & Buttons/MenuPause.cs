using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPause : MonoBehaviour
{
    public static bool isPaused;

    public void Pause()
    {
        if (!PlayerState.isDead && !PlayerState.isWin)
        {
            if (!isPaused)
            {
                Time.timeScale = 0;
                AudioListener.pause = true;
                isPaused = true;
                
                InputManager.player.Disable();
                InputManager.menu.Enable();
            }
            else
            {
                Time.timeScale = 1;
                AudioListener.pause = false;
                isPaused = false;

                InputManager.player.Enable();
                InputManager.menu.Disable();
            }
            Debug.Log(isPaused);
        }
    }
}
