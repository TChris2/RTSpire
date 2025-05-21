using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Button functions for the sub option menu main buttons
public class SubOptionMenuButtons : MonoBehaviour
{
    // Content of audio menu
    [SerializeField]
    private CanvasGroup audioMenu;
    // Content of voice clip menu
    [SerializeField]
    private CanvasGroup clipMenu;
    // The main sub op menu
    private CanvasGroup subOpMenu;
    // The main op menu
    [SerializeField]
    private CanvasGroup opMenu;
    // Sets as selected object when player goes back to the base audio settings
    [SerializeField]
    // Buttons of the previous menu - 1st is Game 2nd is Audio 
    private GameObject[] prevOpMenuButton;
    [SerializeField]
    private VoiceClipSelectionButtons vClipMenuBtns;
    [SerializeField]
    private AudioClip saveClip;
    private AudioSource audioSource;
    [SerializeField]
    private TMPro.TMP_Text saveSelectTxt;
    private bool isPressed;
    // For sub option yes or no screen
    [SerializeField]
    private CanvasGroup ynScreen;
    [SerializeField]
    private GameObject ynInitial;
    // Initial selected object for the clip menu
    [SerializeField]
    private GameObject subOpMenuInitial;
    private GameObject subOpMenuPrev;
    // Keeps tracks of which sub option menu was open
    private ScrollRectAutoScroll vClipAutoScroll;
    public string subOpMenuOpen;
    public bool isSubTypeMenuOpen;
    private CanvasGroup subTypeMenu;

    void Start()
    {
        vClipAutoScroll = GameObject.Find("Voice Clip Main Menu").GetComponent<ScrollRectAutoScroll>();
        subTypeMenu = GameObject.Find($"Type Menu").GetComponent<CanvasGroup>();

        audioSource = GetComponent<AudioSource>();
        audioSource.ignoreListenerPause = true;
        subOpMenu = GetComponent<CanvasGroup>();
        isSubTypeMenuOpen = false;
    }

    public void SaveSelection()
    {
        if (!isPressed) {
            isPressed = true;
            if (clipMenu.interactable)
                vClipMenuBtns.SaveClipSelection();

            StartCoroutine(UpdateTxt());
        }
    }

    public IEnumerator UpdateTxt()
    {
        audioSource.PlayOneShot(saveClip);
        float clipLength = saveClip.length;
        yield return new WaitForSecondsRealtime(.15f);
        saveSelectTxt.text = $"{"Selection Saved"}";
        yield return new WaitForSecondsRealtime(clipLength-1.1f);

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
            {
                clipMenu.interactable = false;
                clipMenu.alpha = 0;
                clipMenu.blocksRaycasts = false;
            }
            if (subOpMenuOpen == "Tumble Mat")
            {

            }

            subOpMenu.interactable = false;

            EventSystem.current.SetSelectedGameObject(ynInitial);

            ynScreen.interactable = true;
            ynScreen.alpha = 1;
            ynScreen.blocksRaycasts = true;
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

        if (subOpMenuOpen == "Tumble Mat")
        {

        }

        // Updates navigation
        menuNav.UpdateTopBarNavigation();
            
        subTypeMenu.alpha = 0;
        subTypeMenu.interactable = false;
        subTypeMenu.blocksRaycasts = false;

        ScrollRectAutoScroll subTypeMenuAutoScroll = subTypeMenu.GetComponent<ScrollRectAutoScroll>();

        subTypeMenuAutoScroll.isMenuOpen = false;

        menuNav = subTypeMenu.GetComponentInChildren<MenuNavigation>();
        ClearTypeMenu(menuNav.gameObject);

        EventSystem.current.SetSelectedGameObject(subOpMenuPrev);

        if (subOpMenuOpen == "Voice Clip")
        {
            clipMenu.interactable = true;
            clipMenu.alpha = 1;
            clipMenu.blocksRaycasts = true;
        }
            
        isSubTypeMenuOpen = false;
    }

    public void No()
    {
        ynScreen.interactable = false;
        ynScreen.alpha = 0;
        ynScreen.blocksRaycasts = false;

        EventSystem.current.SetSelectedGameObject(subOpMenuInitial);

        subOpMenu.interactable = true;

        // Reopens the currently open sub menu
        if (subOpMenuOpen == "Voice Clip")
        {
            clipMenu.interactable = true;
            clipMenu.alpha = 1;
            clipMenu.blocksRaycasts = true;
        }
    }

    public void CloseSubOpMenu() 
    {
        ynScreen.interactable = false;
        ynScreen.alpha = 0;
        ynScreen.blocksRaycasts = false; 

        subOpMenu.interactable = false;
        subOpMenu.alpha = 0;
        subOpMenu.blocksRaycasts = false;

        // Reopens previous menu
        if (subOpMenuOpen == "Voice Clip")
        {
            vClipAutoScroll.isMenuOpen = false;
            
            EventSystem.current.SetSelectedGameObject(prevOpMenuButton[1]);

            audioMenu.interactable = true;
            audioMenu.alpha = 1;
            audioMenu.blocksRaycasts = true;
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
        }

        // Enables auto scroll
        subTypeMenuAutoScroll.isMenuOpen = true;
        // Gets all selectables for scroll
        subTypeMenuAutoScroll.Start();
        StartCoroutine(subTypeMenuAutoScroll.AutoScroll());
        // Resets scroll view
        subTypeMenuScrollRect.verticalNormalizedPosition = 1;

        // Updates navigation
        menuNav.UpdateTypeMenuNav(2);
        menuNav.UpdateTopBarNavigation();

        subOpMenuPrev = EventSystem.current.currentSelectedGameObject;

        // Sets the initially selected object for the menu
        EventSystem.current.SetSelectedGameObject(subOpMenuInitial);

        isSubTypeMenuOpen = true;

        clipMenu.interactable = false;
        clipMenu.alpha = 0;
        clipMenu.blocksRaycasts = false;

        // Enables the menu
        subTypeMenu.alpha = 1;
        subTypeMenu.interactable = true;
        subTypeMenu.blocksRaycasts = true;
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
}
