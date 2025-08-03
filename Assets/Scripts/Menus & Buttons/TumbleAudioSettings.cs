using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Controls the different sliders that effect the enemy voice clips
public class TumbleAudioSettings : MonoBehaviour
{
    [Header("Sliders & Toggles")]
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
    // Toggle for Enemy Voice Clip Play
    [SerializeField]
    private Toggle clipPlayToggle;
    // Txt for Enemy Voice Clip Play Toggle 
    [SerializeField]
    private TMPro.TMP_Text clipPlayToggleTxt;
    public bool isClipPlay;
    [Header("Menus")]
    // Op menu
    [SerializeField]
    private CanvasGroup opMenu;
    // Main audio op menu
    [SerializeField]
    private CanvasGroup audioMenu;
    // Sub op menu
    [SerializeField]
    private CanvasGroup subOpMenu;
    [Header("Scripts")]
    [SerializeField]
    private SubOptionMenuButtons SubOpMenuBtns;

    void Start()
    {
        // Get components
        SubOpMenuBtns.vClipAutoScroll = GameObject.Find("Voice Clip Menu").GetComponent<ScrollRectAutoScroll>();

        // Loads the previous value of the sliders
        yapSlider.value = PlayerPrefs.GetFloat("Tumble Yap Rate", 75);
        SetYapRate();
        maxDelaySlider.value = PlayerPrefs.GetFloat("Tumble Yap Delay Max", 50);
        SetMaxDelay();
        minDelaySlider.value = PlayerPrefs.GetFloat("Tumble Yap Delay Min", 20);
        SetMinDelay();
        clipPlayToggle.isOn = PlayerPrefs.GetFloat("Enemy Voice Clip Play Toggle", 1) == 1 ? true : false;
        VoiceClipPlayToggle(true);
    }

    // Updates the slider for the yap rate
    public void SetYapRate()
    {
        yapRate = yapSlider.value;
        yapText.text = $"- An Enemy Has A <style=\"Highlight\">{yapRate}%</style> Chance of Playing A Voice Clip";
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
            delayText.text = $"Enemies Have A Delay of <style=\"Min\">{minDelay}</style> Seconds\nBefore Playing A Voice Clip";
        // If they are not
        else
            delayText.text = $"Enemies Have A Delay Between <style=\"Min\">{minDelay}</style>-<style=\"Max\">{maxDelay}</style> Seconds\nBefore Playing A Voice Clip";
    }

    // Enables or disables voice clips from playing
    public void VoiceClipPlayToggle(bool isStart)
    {
        if (clipPlayToggle.isOn)
            clipPlayToggleTxt.text = $"{"- Enemies <style=\"Active\">Will</style> Randomly Play Voice Clips"}";
        else
            clipPlayToggleTxt.text = $"{"- Enemies <style=\"InActive\">Will Not</style> Randomly Play Voice Clips"}";

        isClipPlay = clipPlayToggle.isOn;

        // Resets enemy audio so changes can occur, does not occur at start
        if (!isStart)
            SubOpMenuBtns.vClipMenuBtns.EnemyAudioReset();
    }

    // Opens the Enemy Voice Clip Menu
    public void ClipMenuOpen()
    {
        // Tells the script which sub menu it is opening
        SubOpMenuBtns.subOpMenuOpen = "Voice Clip";

        // Resets all clip's temp enabled values before entering the menu
        SubOpMenuBtns.vClipMenuBtns.ReloadEnables();

        // Disables the audio menu
        MenuOpenClose(audioMenu, false);

        // Disables the option menu buttons whilst the sub menu is open
        opMenu.interactable = false;

        // Enables the sub option menu
        MenuOpenClose(SubOpMenuBtns.subOpMenu, true);

        // Enables auto scroll
        SubOpMenuBtns.vClipAutoScroll.isMenuOpen = true;
        StartCoroutine(SubOpMenuBtns.vClipAutoScroll.AutoScroll());
        // Resets scroll view
        ScrollRect vClipScrollRect = SubOpMenuBtns.vClipAutoScroll.GetComponent<ScrollRect>();
        vClipScrollRect.verticalNormalizedPosition = 1;
        // Updates navigation
        MenuNavigation menuNav = SubOpMenuBtns.vClipAutoScroll.GetComponentInChildren<MenuNavigation>();
        menuNav.UpdateTopBarNavigation();

        // Sets the initially selected object for the menu
        EventSystem.current.SetSelectedGameObject(SubOpMenuBtns.subOpMenuInitial);

        // Enables the enemy voice clip menu
        MenuOpenClose(SubOpMenuBtns.clipMenu, true);
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
        PlayerPrefs.SetFloat("Tumble Yap Rate", yapRate);
        PlayerPrefs.SetFloat("Tumble Yap Delay Min", minDelay);
        PlayerPrefs.SetFloat("Tumble Yap Delay Max", maxDelay);
        PlayerPrefs.SetFloat("Enemy Voice Clip Play Toggle", isClipPlay ? 1 : 0);

        PlayerPrefs.Save();
    }
}
