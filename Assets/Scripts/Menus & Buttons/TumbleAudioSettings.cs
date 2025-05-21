using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Controls the different sliders that effect the enemy voice clips
public class TumbleAudioSettings : MonoBehaviour
{
    // Yap Rate
    public float yapRate;
    [SerializeField]
    private Slider yapSlider;
    [SerializeField]
    private TMPro.TMP_Text yapText;
    // Yap Delay
    public float minDelay;
    public float maxDelay;
    [SerializeField]
    private Slider minDelaySlider;
    [SerializeField]
    private Slider maxDelaySlider;
    [SerializeField]
    private TMPro.TMP_Text delayText;
    // Op menu
    [SerializeField]
    private CanvasGroup opMenu;
    // Main audio menu
    [SerializeField]
    private CanvasGroup audioMenu;
    // Clip menu stored in the sub op menu
    [SerializeField]
    private CanvasGroup clipMenu;
    // Sub op menu
    [SerializeField]
    private CanvasGroup subOpMenu;
    // Initial selected object for the sub op menu
    [SerializeField]
    private GameObject subMenuInitial;
    [SerializeField]
    private VoiceClipSelectionButtons vClipMenu;
    [SerializeField]
    private SubOptionMenuButtons SubOpMenuBtns;
    // Toggle for Enemy Voice Clip Play
    [SerializeField]
    private Toggle clipPlayToggle;
    // Txt for Enemy Voice Clip Play Toggle 
    [SerializeField]
    private TMPro.TMP_Text clipPlayToggleTxt;
    // For updating navigation for the sub menu when it is opened
    [SerializeField]
    private Button saveSubMenuBtn;
    [SerializeField]
    private Button exitSubMenuBtn;
    [SerializeField]
    private Selectable vClipMenuBtnNav;
    // Auto Scroll
    private ScrollRectAutoScroll vClipAutoScroll;

    void Start()
    {
        vClipAutoScroll = GameObject.Find("Voice Clip Main Menu").GetComponent<ScrollRectAutoScroll>();

        // Loads the previous value of the sliders
        yapSlider.value = PlayerPrefs.GetFloat("Tumble Yap Rate", 75);
        SetYapRate();
        maxDelaySlider.value = PlayerPrefs.GetFloat("Tumble Yap Delay Max", 50);
        SetMaxDelay();
        minDelaySlider.value = PlayerPrefs.GetFloat("Tumble Yap Delay Min", 20);
        SetMinDelay();

        // If enabled
        if (PlayerPrefs.GetFloat("Enemy Voice Clip Play Toggle", 1) == 1) 
            clipPlayToggle.isOn = true;
        // If disabled
        else
            clipPlayToggle.isOn = false;
    }

    // Updates the slider for the yap rate
    public void SetYapRate()
    {
        yapRate = yapSlider.value;
        yapText.text = $"{"An Enemy Voice Clip Has A " + yapRate + "% Chance of Playing"}";
    }

    // Updates the slider for the min delay for enemy voice clips to play
    public void SetMinDelay()
    {
        minDelay = minDelaySlider.value;

        // Ensures min delay does not exceed max delay
        if (minDelaySlider.value > maxDelay)
        {
            maxDelaySlider.value = minDelay;
            maxDelay = maxDelaySlider.value;
        }
        // Updates text
        UpdateDelayText();
    }

    // Updates the slider for the max delay for enemy voice clips to play
    public void SetMaxDelay()
    {
        maxDelay = maxDelaySlider.value;

        // Ensures max delay does not go below min delay
        if (minDelaySlider.value > maxDelay)
        {
            minDelaySlider.value = maxDelay;
            minDelay = minDelaySlider.value;
        }
        // Updates text
        UpdateDelayText();
    }

    // Updates delay text
    void UpdateDelayText()
    {
        // If they are the same value
        if (minDelay == maxDelay)
            delayText.text = $"{"An Enemy Has A Delay of " + minDelay + " Seconds\nBefore Playing A Voice Clip"}";
        // If they are not
        else
            delayText.text = $"{"An Enemy Has A Delay Between " + minDelay + "-" + maxDelay + " Seconds\nBefore Playing A Voice Clip"}";
    }

    // Enables or disables voice clips from playing
    public void VoiceClipPlayToggle() 
    {
        if (clipPlayToggle.isOn)
            clipPlayToggleTxt.text = $"{"Enemy Voice Clips Enabled"}";
        else
            clipPlayToggleTxt.text = $"{"Enemy Voice Clips Disabled"}";
    }
    
    // Opens the Enemy Voice Clip Menu
    public void ClipMenuOpen() 
    {
        // Tells the script which sub menu it is opening
        SubOpMenuBtns.subOpMenuOpen = "Voice Clip";
        
        // Resets all clip's temp enabled values before entering the menu
        vClipMenu.ReloadClips();

        // Disables the audio menu
        audioMenu.interactable = false;
        audioMenu.alpha = 0;
        audioMenu.blocksRaycasts = false;
        // Disables the option menu buttons whilst the sub menu is open
        opMenu.interactable = false;

        // Enables the sub option menu
        subOpMenu.interactable = true;
        subOpMenu.alpha = 1;
        subOpMenu.blocksRaycasts = true;

        // Enables auto scroll
        vClipAutoScroll.isMenuOpen = true;
        StartCoroutine(vClipAutoScroll.AutoScroll());
        // Resets scroll view
        ScrollRect vClipScrollRect = vClipAutoScroll.GetComponent<ScrollRect>();
        vClipScrollRect.verticalNormalizedPosition = 1;
        // Updates navigation
        MenuNavigation menuNav = vClipAutoScroll.GetComponentInChildren<MenuNavigation>();
        menuNav.UpdateTopBarNavigation();

        // Updates sub menu navigation to the open menu
        Navigation saveSubMenuBtnNav = saveSubMenuBtn.navigation;
        saveSubMenuBtnNav.selectOnDown = vClipMenuBtnNav;

        Navigation exitSubMenuBtnNav = exitSubMenuBtn.navigation;
        exitSubMenuBtnNav.selectOnDown = vClipMenuBtnNav;

        saveSubMenuBtn.navigation = saveSubMenuBtnNav;
        exitSubMenuBtn.navigation = exitSubMenuBtnNav;

        // Sets the initially selected object for the menu
        EventSystem.current.SetSelectedGameObject(subMenuInitial);

        // Enables the enemy voice clip menu
        clipMenu.interactable = true;
        clipMenu.alpha = 1;
        clipMenu.blocksRaycasts = true;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("Tumble Yap Rate", yapRate);
        PlayerPrefs.SetFloat("Tumble Yap Delay Min", minDelay);
        PlayerPrefs.SetFloat("Tumble Yap Delay Max", maxDelay);

        // If enabled
        if (clipPlayToggle.isOn) 
            PlayerPrefs.SetFloat("Enemy Voice Clip Play Toggle", 1);
        // If disabled
        else
            PlayerPrefs.SetFloat("Enemy Voice Clip Play Toggle", 0);
        PlayerPrefs.Save();
    }
}
