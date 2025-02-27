using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Reselects UI
public class UIReselector : MonoBehaviour
{
    private GameObject pMenuCurrent;

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null) 
            pMenuCurrent = EventSystem.current.currentSelectedGameObject;
        else
            EventSystem.current.SetSelectedGameObject(pMenuCurrent);
    }
}
