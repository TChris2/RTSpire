using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            }
            else
            {
                Time.timeScale = 1;
                AudioListener.pause = false;
                isPaused = false;
            }
        }
    }
}
