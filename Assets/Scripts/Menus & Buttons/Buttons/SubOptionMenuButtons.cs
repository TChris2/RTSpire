using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Button functions for the sub option menu main buttons
public class SubOptionMenuButtons : MonoBehaviour
{
    [Header("Menus")]
    // Audio settings menu
    [SerializeField]
    private CanvasGroup audioMenu;
    // Voice clip menu
    public CanvasGroup clipMenu;
    // Game settings menu
    [SerializeField]
    private CanvasGroup gameMenu;
    // Tumble design menu
    [SerializeField]
    private CanvasGroup designMenu;
    // The main sub op menu
    public CanvasGroup subOpMenu;
    // The main op menu
    [SerializeField]
    private CanvasGroup opMenu;
    // For sub option yes or no screen
    public CanvasGroup ynScreen;
    private CanvasGroup subTypeMenu;
    // Scripts
    public VoiceClipSelectionButtons vClipMenuBtns;
    public TumbleDesignSelectionButtons tDesignMenuBtns;
    [HideInInspector]
    public ScrollRectAutoScroll vClipAutoScroll;
    [HideInInspector]
    public ScrollRectAutoScroll tDesignAutoScroll;
    [Header("Objects")]
    [SerializeField]
    private GameObject ynInitial;
    // Initial selected object for the clip menu
    public GameObject subOpMenuInitial;
    private GameObject subOpMenuPrev;
    // Sets as selected object when player goes back to the prev menu
    // Buttons of the previous menu - 1st is Game 2nd is Audio 
    [SerializeField]
    private GameObject[] prevOpMenuButton;
    [SerializeField]
    private TMPro.TMP_Text saveSelectTxt;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip saveClip;
    private bool isPressed;
    // Keeps tracks of which sub option menu was open
    public string subOpMenuOpen;
    public bool isSubTypeMenuOpen;

    void Start()
    {
        vClipAutoScroll = GameObject.Find("Voice Clip Menu").GetComponent<ScrollRectAutoScroll>();
        tDesignAutoScroll = GameObject.Find("Tumble Design Menu").GetComponent<ScrollRectAutoScroll>();
        subTypeMenu = GameObject.Find($"Type Menu").GetComponent<CanvasGroup>();
        vClipMenuBtns = FindObjectOfType<VoiceClipSelectionButtons>();
        tDesignMenuBtns = FindObjectOfType<TumbleDesignSelectionButtons>();

        audioSource = GetComponent<AudioSource>();
        audioSource.ignoreListenerPause = true;
        subOpMenu = GetComponent<CanvasGroup>();
        isSubTypeMenuOpen = false;
    }

    public void SaveSelection()
    {
        if (!isPressed)
        {
            isPressed = true;
            if (clipMenu.interactable)
                vClipMenuBtns.SaveClipSelection();
            if (designMenu.interactable)
                tDesignMenuBtns.SaveDesignSelection();

            StartCoroutine(UpdateTxt());
        }
    }

    public IEnumerator UpdateTxt()
    {
        audioSource.PlayOneShot(saveClip);
        float clipLength = saveClip.length;
        yield return new WaitForSecondsRealtime(.15f);
        saveSelectTxt.text = $"{"Selection Saved"}";
        yield return new WaitForSecondsRealtime(clipLength - 1.1f);

        saveSelectTxt.text = $"{"Save Selection"}";
        isPressed = false;
    }

    // Opens yes no screen for exitting sub option menu
    public void ExitSubOpMenu()
    {
        if (!isSubTypeMenuOpen)
        {
            // Closes currently open sub menu
            if (subOpMenuOpen == "Voice Clip")
                MenuOpenClose(clipMenu, false);
            if (subOpMenuOpen == "Tumble Design")
                MenuOpenClose(designMenu, false);

            subOpMenu.interactable = false;

            EventSystem.current.SetSelectedGameObject(ynInitial);

            MenuOpenClose(ynScreen, true);
        }
        // Closes all sub sub menus
        else
        {
            CloseSubTypeMenu();
        }
    }

    public void CloseSubTypeMenu()
    {
        MenuNavigation menuNav = null;

        if (subOpMenuOpen == "Voice Clip")
        {
            // Gets comp from previously opened menu
            menuNav = vClipAutoScroll.GetComponentInChildren<MenuNavigation>();
        }

        if (subOpMenuOpen == "Tumble Design")
        {
            // Gets comp from previously opened menu
            menuNav = tDesignAutoScroll.GetComponentInChildren<MenuNavigation>();
        }

        // Updates navigation
        menuNav.UpdateTopBarNavigation();

        MenuOpenClose(subTypeMenu, false);

        ScrollRectAutoScroll subTypeMenuAutoScroll = subTypeMenu.GetComponent<ScrollRectAutoScroll>();

        subTypeMenuAutoScroll.isMenuOpen = false;

        menuNav = subTypeMenu.GetComponentInChildren<MenuNavigation>();
        ClearTypeMenu(menuNav.gameObject);

        EventSystem.current.SetSelectedGameObject(subOpMenuPrev);

        if (subOpMenuOpen == "Voice Clip")
            MenuOpenClose(clipMenu, true);
        if (subOpMenuOpen == "Tumble Design")
            MenuOpenClose(designMenu, true);

        isSubTypeMenuOpen = false;
    }

    public void No()
    {
        MenuOpenClose(ynScreen, false);

        EventSystem.current.SetSelectedGameObject(subOpMenuInitial);

        subOpMenu.interactable = true;

        // Reopens the currently open sub menu
        if (subOpMenuOpen == "Voice Clip")
            MenuOpenClose(clipMenu, true);
        if (subOpMenuOpen == "Tumble Design")
            MenuOpenClose(designMenu, true);
    }

    public void CloseSubOpMenu()
    {
        // Closes sub option menu items
        MenuOpenClose(ynScreen, false);
        MenuOpenClose(subOpMenu, false);

        // Reopens previous menu
        if (subOpMenuOpen == "Voice Clip")
        {
            MenuOpenClose(clipMenu, false);

            vClipAutoScroll.isMenuOpen = false;

            EventSystem.current.SetSelectedGameObject(prevOpMenuButton[1]);

            MenuOpenClose(audioMenu, true);
            opMenu.interactable = true;
        }
        if (subOpMenuOpen == "Tumble Design")
        {
            MenuOpenClose(designMenu, false);

            tDesignAutoScroll.isMenuOpen = false;

            EventSystem.current.SetSelectedGameObject(prevOpMenuButton[0]);

            MenuOpenClose(gameMenu, true);
            opMenu.interactable = true;
        }
    }

    public void OpenSubTypeMenu(string subTypeMenuName)
    {
        MenuNavigation menuNav = subTypeMenu.GetComponentInChildren<MenuNavigation>();
        ScrollRectAutoScroll subTypeMenuAutoScroll = subTypeMenu.GetComponent<ScrollRectAutoScroll>();
        ScrollRect subTypeMenuScrollRect = subTypeMenu.GetComponent<ScrollRect>();

        if (subOpMenuOpen == "Voice Clip")
        {
            vClipMenuBtns.CreateClipTypeMenu(subTypeMenuName, menuNav.gameObject, GetComponent<AudioSource>());
            subTypeMenuAutoScroll.rowAmt = 2;
            // Closes sub option menu
            MenuOpenClose(clipMenu, false);
        }
        if (subOpMenuOpen == "Tumble Design")
        {
            tDesignMenuBtns.CreateSubDesignMenu(int.Parse(subTypeMenuName), menuNav.gameObject, GetComponent<AudioSource>());
            subTypeMenuAutoScroll.rowAmt = 5;
            // Closes sub option menu
            MenuOpenClose(designMenu, false);
        }

        // Enables auto scroll
        subTypeMenuAutoScroll.isMenuOpen = true;
        // Gets all selectables for scroll
        subTypeMenuAutoScroll.Start();
        StartCoroutine(subTypeMenuAutoScroll.AutoScroll());
        // Resets scroll view
        subTypeMenuScrollRect.verticalNormalizedPosition = 1;

        // Updates navigation
        menuNav.UpdateTypeMenuNav(subTypeMenuAutoScroll.rowAmt);
        menuNav.UpdateTopBarNavigation();

        subOpMenuPrev = EventSystem.current.currentSelectedGameObject;

        // Sets the initially selected object for the menu
        EventSystem.current.SetSelectedGameObject(subOpMenuInitial);

        isSubTypeMenuOpen = true;

        // Enables the sub type menu
        MenuOpenClose(subTypeMenu, true);
    }

    // Clears contents of type menu
    public void ClearTypeMenu(GameObject subTypeMenu)
    {
        Transform subTypeContent = subTypeMenu.transform;

        foreach (Transform child in subTypeContent)
        {
            Destroy(child.gameObject);
        }
    }
    
    // Opens or closes selected menus
    private void MenuOpenClose(CanvasGroup menu, bool isOpen)
    {
        menu.interactable = isOpen;
        menu.alpha = isOpen ? 1 : 0;
        menu.blocksRaycasts = isOpen;
    }
}
