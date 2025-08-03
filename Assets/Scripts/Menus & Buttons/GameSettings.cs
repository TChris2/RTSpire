using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Handles game setting's options
public class GameSettings : MonoBehaviour
{
    [Header("Sliders & Toggles")]
    [SerializeField]
    private Toggle fsToggle;
    private PlayerLook pLook;
    [SerializeField]
    private Slider camSensSlider;
    [SerializeField]
    private TMPro.TMP_Text camSensText;
    [SerializeField]
    private Slider zoomSensSlider;
    [SerializeField]
    private TMPro.TMP_Text zoomSensText;
    [SerializeField]
    private Toggle camInvertToggle;
    [SerializeField]
    private TMPro.TMP_Text camInvertToggleTxt;
    [Header("Menus")]
    // Op menu
    [SerializeField]
    private CanvasGroup opMenu;
    // Main audio op menu
    [SerializeField]
    private CanvasGroup gameMenu;
    // Clip menu stored in the sub op menu
    [SerializeField]
    private CanvasGroup designMenu;
    [Header("Scripts")]
    [SerializeField]
    private SubOptionMenuButtons SubOpMenuBtns;
    private Camera cam;

    void Start()
    {
        // Get components
        SubOpMenuBtns.tDesignAutoScroll = GameObject.Find("Tumble Design Menu").GetComponent<ScrollRectAutoScroll>();
        pLook = GameObject.Find("Player").GetComponent<PlayerLook>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();

        // Loads the previous values
        fsToggle.isOn = PlayerPrefs.GetFloat("Full Screen", 0) != 0;
        ToggleFullscreen(true);
        camSensSlider.value = PlayerPrefs.GetFloat("Camera Rotation Sensitivity", 0);
        SetCameraSensRate();
        zoomSensSlider.value = PlayerPrefs.GetFloat("Camera Zoom Sensitivity", 0);
        SetZoomSensRate();
        camInvertToggle.isOn = PlayerPrefs.GetFloat("Camera Controls Inverted", 1) != 0;
        CameraInvertToggle();
    }

    public void ToggleFullscreen(bool isStart)
    {
        int width;
        int height;

        if (fsToggle.isOn)
        {
            if (!isStart)
            {
                PlayerPrefs.SetInt("Window Size X", Screen.width);
                PlayerPrefs.SetInt("Window Size Y", Screen.height);
            }

            width = Display.main.systemWidth;
            height = Display.main.systemHeight;
        }
        else
        {
            width = PlayerPrefs.GetInt("Window Size X", Screen.width);
            height = PlayerPrefs.GetInt("Window Size Y", Screen.height);
        }

        Screen.SetResolution(width, height, fsToggle.isOn);
    }

    // Updates the slider for camera rotation sensitivity
    public void SetCameraSensRate()
    {
        pLook.rotateSens = camSensSlider.value + 100;
        camSensText.text = $"- <style=\"Highlight\">{camSensSlider.value}</style>";
    }

    // Updates the slider for the camera zoom sensitivity
    public void SetZoomSensRate()
    {
        pLook.zoomSens = zoomSensSlider.value + 40;
        zoomSensText.text = $"- <style=\"Highlight\">{zoomSensSlider.value}</style>";
    }

    // Enables inverted camera controls
    public void CameraInvertToggle()
    {
        // Absolutes the value to prevent potential issues
        pLook.rotateSens = Mathf.Abs(pLook.rotateSens);

        if (camInvertToggle.isOn)
        {
            pLook.rotateSens *= -1;
            camInvertToggleTxt.text = "- Controls <style=\"Active\">Are</style> Inverted";
        }
        else
            camInvertToggleTxt.text = "- Controls <style=\"InActive\">Are Not</style> Inverted";
    }

    // Opens the Enemy Tumble Design Menu
    public void DesignMenuOpen()
    {
        // Tells the script which sub menu it is opening
        SubOpMenuBtns.subOpMenuOpen = "Tumble Design";

        // Resets all temp enabled values before entering the menu
        SubOpMenuBtns.tDesignMenuBtns.ReloadEnables();

        // Disables the audio menu
        MenuOpenClose(gameMenu, false);

        // Disables the option menu buttons whilst the sub menu is open
        opMenu.interactable = false;

        // Enables the sub option menu
        MenuOpenClose(SubOpMenuBtns.subOpMenu, true);

        // Enables auto scroll
        SubOpMenuBtns.tDesignAutoScroll.isMenuOpen = true;
        StartCoroutine(SubOpMenuBtns.tDesignAutoScroll.AutoScroll());
        // Resets scroll view
        ScrollRect vClipScrollRect = SubOpMenuBtns.tDesignAutoScroll.GetComponent<ScrollRect>();
        vClipScrollRect.verticalNormalizedPosition = 1;
        // Updates navigation
        MenuNavigation menuNav = SubOpMenuBtns.tDesignAutoScroll.GetComponentInChildren<MenuNavigation>();
        menuNav.UpdateTopBarNavigation();

        // Sets the initially selected object for the menu
        EventSystem.current.SetSelectedGameObject(SubOpMenuBtns.subOpMenuInitial);

        // Enables the enemy voice clip menu
        MenuOpenClose(designMenu, true);
    }

    // Opens or closes selected menus
    private void MenuOpenClose(CanvasGroup menu, bool isOpen)
    {
        menu.interactable = isOpen;
        menu.alpha = isOpen ? 1 : 0;
        menu.blocksRaycasts = isOpen;
    }
    
    private void OnDisable()
    {
        // Saves prefs
        PlayerPrefs.SetFloat("Full Screen", Screen.fullScreen ? 1 : 0);
        PlayerPrefs.SetFloat("Camera Rotation Sensitivity", camSensSlider.value);
        PlayerPrefs.SetFloat("Camera Zoom Sensitivity", zoomSensSlider.value);
        PlayerPrefs.SetFloat("Camera Controls Inverted", camInvertToggle.isOn ? 1 : 0);

        PlayerPrefs.Save();
    }
}
