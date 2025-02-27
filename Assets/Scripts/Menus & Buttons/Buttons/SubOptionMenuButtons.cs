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
    private VoiceClipSelectionMenuButtons vcMenuBtns;
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
    // Keeps tracks of which sub option menu was open
    [SerializeField]
    private ScrollRectAutoScroll vClipAutoScroll;

    public string subOpMenuOpen;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.ignoreListenerPause = true;
        subOpMenu = GetComponent<CanvasGroup>();
    }

    public void SaveSelection()
    {
        if (!isPressed) {
            isPressed = true;
            if (clipMenu.interactable)
                vcMenuBtns.SaveClipSelection();

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

    public void ExitSubOpMenu() 
    {
        // Closes currently open sub menu
        if (subOpMenuOpen == "Voice Clip")
        {
            clipMenu.interactable = false;
            clipMenu.alpha = 0;
            clipMenu.blocksRaycasts = false;
        }

        subOpMenu.interactable = false;

        EventSystem.current.SetSelectedGameObject(ynInitial);

        ynScreen.interactable = true;
        ynScreen.alpha = 1;
        ynScreen.blocksRaycasts = true; 
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
}
