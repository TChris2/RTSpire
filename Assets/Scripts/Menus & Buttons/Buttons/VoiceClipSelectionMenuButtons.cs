using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Button functions for the voice clip menu
public class VoiceClipSelectionMenuButtons : MonoBehaviour
{
    // SFW Clips
    [SerializeField]
    private AudioClip[] sfwClips; 
    // NSFW Clips
    [SerializeField]
    private AudioClip[] nsfwClips; 
    // Toad Clips
    [SerializeField]
    private AudioClip[] toadClips; 
    // Noise clips
    [SerializeField]
    private AudioClip[] noiseClips; 
    // The combined list of all selected clips
    public List<AudioClip> clipList = new List<AudioClip>(); 

    // Voice clip toggle buttons 1st is toggle at the top and 2nd is the toggle at the section itself
    [SerializeField]
    private Toggle AllToggleBtn;
    [SerializeField]
    private Toggle[] SFWToggleBtns;
    [SerializeField]
    private Toggle[] NSFWToggleBtns;
    [SerializeField]
    private Toggle[] ToadToggleBtns;
    [SerializeField]
    private Toggle[] NoiseToggleBtns;
    void Start()
    {
        LoadAllClipBtns();
    }

    public void LoadAllClipBtns()
    {
        // Clears the list
        clipList.Clear();

        // Loads the saved list of enabled SFW clips
        LoadClipSelectionStart("SFW", sfwClips);
        // Loads the saved list of enabled NSFW clips
        LoadClipSelectionStart("NSFW", nsfwClips);
        // Loads the saved list of enabled Toad clips
        LoadClipSelectionStart("Toad", toadClips);
        // Loads the saved list of enabled Toad clips
        LoadClipSelectionStart("Noise", noiseClips);
    }

    // Loads the saved list of enabled clips
    public void LoadClipSelectionStart(string clipType, AudioClip[] vClips)
    {   
        // Adds the enabled clips to the list
        for (int i = 0; i < vClips.Length; i++) 
        {   
            Toggle toggleBtn = GameObject.Find($"{clipType} Voice Clip {i} Toggle Button").GetComponent<Toggle>();
            
            // If enabled
            if (PlayerPrefs.GetFloat($"{clipType} Voice Clip {i} Toggle Button", 1) == 1) 
            {
                toggleBtn.isOn = true;
                clipList.Add(vClips[i]);
            }
            // If disabled
            else
                toggleBtn.isOn = false;
        }
    }

    // Saves selected clips to the list and playerprefs
    public void SaveClipSelection()
    {
        // Clears the list
        clipList.Clear();
        
        // Saves selected SFW buttons to prefs and adds them to the list
        ButtonPrefSaved("SFW", sfwClips);
        // Saves selected NSFW buttons to prefs and adds them to the list
        ButtonPrefSaved("NSFW", nsfwClips);
        // Saves selected Toad buttons to prefs and adds them to the list
        ButtonPrefSaved("Toad", toadClips);
        // Saves selected Noise buttons to prefs and adds them to the list
        ButtonPrefSaved("Noise", noiseClips);
    }

    // Saves selected buttons to prefs and adds them to the list
    private void ButtonPrefSaved(string clipType, AudioClip[] vClips)
    {
        // Saves selected buttons to prefs
        for (int i = 0; i < vClips.Length; i++) 
        {   
            Toggle toggleBtn = GameObject.Find($"{clipType} Voice Clip {i} Toggle Button").GetComponent<Toggle>();
            // If enabled
            if (toggleBtn.isOn) 
            {
                // Adds clip to the list
                clipList.Add(vClips[i]);
                PlayerPrefs.SetFloat($"{clipType} Voice Clip {i} Toggle Button", 1);
            }
            // If disabled
            else
                PlayerPrefs.SetFloat($"{clipType} Voice Clip {i} Toggle Button", 0);
        }
        
        // Saves prefs
        PlayerPrefs.Save();
    }
    
    // Enables or Disables All Clips depending on if it is enabled or disabled
    public void AllToggle(bool isToggle) 
    {
        // isToggle is so that all preset button can use the same code all the all toggle
        if (AllToggleBtn.isOn || !isToggle)
        {
            SFWToggleBtns[0].isOn = AllToggleBtn.isOn;
            SFWToggleBtns[1].isOn = AllToggleBtn.isOn;
            EnableAll("SFW");

            NSFWToggleBtns[0].isOn = AllToggleBtn.isOn;
            NSFWToggleBtns[1].isOn = AllToggleBtn.isOn;
            EnableAll("NSFW");

            ToadToggleBtns[0].isOn = AllToggleBtn.isOn;
            ToadToggleBtns[1].isOn = AllToggleBtn.isOn;
            EnableAll("Toad");

            NoiseToggleBtns[0].isOn = AllToggleBtn.isOn;
            NoiseToggleBtns[1].isOn = AllToggleBtn.isOn;
            EnableAll("Noise");
        }
        else 
        {
            SFWToggleBtns[0].isOn = AllToggleBtn.isOn;
            SFWToggleBtns[1].isOn = AllToggleBtn.isOn;
            DisableAll("SFW");

            NSFWToggleBtns[0].isOn = AllToggleBtn.isOn;
            NSFWToggleBtns[1].isOn = AllToggleBtn.isOn;
            DisableAll("NSFW");

            ToadToggleBtns[0].isOn = AllToggleBtn.isOn;
            ToadToggleBtns[1].isOn = AllToggleBtn.isOn;
            DisableAll("Toad");

            NoiseToggleBtns[0].isOn = AllToggleBtn.isOn;
            NoiseToggleBtns[1].isOn = AllToggleBtn.isOn;
            DisableAll("Noise");
        }
    }

    // Enables or Disables All SFW Clips depending on if it is enabled or disabled
    public void SFWToggle(int tBtn) 
    {
        Toggle(tBtn, "SFW");
    }

    // Enables or Disables All NSFW Clips depending on if it is enabled or disabled
    public void NSFWToggle(int tBtn) 
    {
        Toggle(tBtn, "NSFW");
    }

    // Enables or Disables All Toad Clips depending on if it is enabled or disabled
    public void ToadToggle(int tBtn)
    {
        Toggle(tBtn, "Toad");
    }

    // Enables or Disables All Noise Clips depending on if it is enabled or disabled
    public void NoiseToggle(int tBtn)
    {
        Toggle(tBtn, "Noise");
    }

    // Enables or Disables All of the selected toggles clips depending on if it is enabled or disabled
    public void Toggle(int tBtn, string clipType) 
    {
        Toggle[] tempToggleBtns = GetToggle(clipType);

        // 0 = Toggle btn at the top
        if (tBtn == 0)
        {
            // Ensures both toggle btns have the same value
            tempToggleBtns[1].isOn = tempToggleBtns[0].isOn;
        }
        // 1 = Toggle btn at the section
        else {
            // Ensures both toggle btns have the same value
            tempToggleBtns[0].isOn = tempToggleBtns[1].isOn;
        }

        if (tempToggleBtns[tBtn].isOn)
            EnableAll(clipType);
        else 
            DisableAll(clipType);
    }

    // Get toggles for Toggle function
    Toggle[] GetToggle(string clipType)
    {
        // Gets the array length of that clipType
        switch(clipType)
        {
            case "SFW":
                return SFWToggleBtns;
            case "NSFW":
                return NSFWToggleBtns;
            case "Toad":
                return ToadToggleBtns;
            case "Noise":
                return NoiseToggleBtns;
            default:
                return null;
        }
    }

    // Gets array length of that group's toggle buttons
    int GetArrayLength(string clipType)
    {
        // Gets the array length of that clipType
        switch(clipType)
        {
            case "SFW":
                return sfwClips.Length;
            case "NSFW":
                return nsfwClips.Length;
            case "Toad":
                return toadClips.Length;
            case "Noise":
                return noiseClips.Length;
            default:
                return 0;
        }
    }

    // Enables all clips of that type
    private void EnableAll(string clipType)
    {
        int vClipLength = GetArrayLength(clipType);

        for (int i = 0; i < vClipLength; i++) 
        {   
            Toggle toggleBtn = GameObject.Find($"{clipType} Voice Clip {i} Toggle Button").GetComponent<Toggle>();
            
            toggleBtn.isOn = true;
        }
    }

    // Disables all clips of that type
    private void DisableAll(string clipType)
    {
        // Gets array length of that group's toggle buttons
        int vClipLength = GetArrayLength(clipType);
        
        for (int i = 0; i < vClipLength; i++) 
        {   
            Toggle toggleBtn = GameObject.Find($"{clipType} Voice Clip {i} Toggle Button").GetComponent<Toggle>();

            toggleBtn.isOn = false;
        }
    }

    // Gets the preset type from the button and applies preset to each group of toggle buttons
    public void Preset(string presetType)
    {
        ToggleEnableDisable(presetType);
        ApplyPreset("SFW", presetType);
        ApplyPreset("NSFW", presetType);
        ApplyPreset("Toad", presetType);
        ApplyPreset("Noise", presetType);
    }

    // Applies preset to all toggle buttons
    private void ApplyPreset(string clipType, string presetType)
    {
        // Gets array length of that group's toggle buttons
        int vClipLength = GetArrayLength(clipType);

        for (int i = 0; i < vClipLength; i++) 
        {   
            // Gets toggle button
            GameObject toggleBtn = GameObject.Find($"{clipType} Voice Clip {i} Toggle Button");
            // Gets the preset script attached to the button
            VoiceClipPreset vcPreset = toggleBtn.GetComponent<VoiceClipPreset>();
            // Gets the toggle comp
            Toggle toggle = toggleBtn.GetComponent<Toggle>();

            // Gets the value of the preset for the toggle button
            bool isPreset = GetPresetValue(presetType, vcPreset);

            // Checks to see if the preset enabled or disabled and enables/disables the toggle
            if (isPreset)
                toggle.isOn = true;
            else
                toggle.isOn = false;
        }
    }

    // Finds the preset value the preset button is applying and returns it 
    private bool GetPresetValue(string presetType, VoiceClipPreset vcPreset)
    {
        switch(presetType)
        {
            case "SFW":
                return vcPreset.isSFW;
            case "NSFW":
                return vcPreset.isNSFW;
            case "Toad":
                return vcPreset.isToad;
            case "Noise":
                return vcPreset.isNoise;
            case "Youtube":
                return vcPreset.isYoutube;
            case "Chat":
                return vcPreset.isChat;
            case "Brainrot":
                return vcPreset.isBrainrot;
            case "Concern":
                return vcPreset.isConcern;
            default:
                return true;
        }
    }

    // Enables that preset's type toggle header buttons & disables the others
    private void ToggleEnableDisable(string presetType)
    {
        switch(presetType)
        {
            case "SFW":
                // Enables that preset's type toggle header button
                SFWToggleBtns[0].isOn = true;
                SFWToggleBtns[1].isOn = true;

                // Disables the other toggle header buttons
                NSFWToggleBtns[0].isOn = false;
                NSFWToggleBtns[1].isOn = false;

                ToadToggleBtns[0].isOn = false;
                ToadToggleBtns[1].isOn = false;

                NoiseToggleBtns[0].isOn = false;
                NoiseToggleBtns[1].isOn = false;

                break;
            case "NSFW":
                // Enables that preset's type toggle header button
                NSFWToggleBtns[0].isOn = true;
                NSFWToggleBtns[1].isOn = true;

                // Disables the other toggle header buttons
                SFWToggleBtns[0].isOn = false;
                SFWToggleBtns[1].isOn = false;

                ToadToggleBtns[0].isOn = false;
                ToadToggleBtns[1].isOn = false;

                NoiseToggleBtns[0].isOn = false;
                NoiseToggleBtns[1].isOn = false;

                break;
            case "Toad":
                // Enables that preset's type toggle header button
                ToadToggleBtns[0].isOn = true;
                ToadToggleBtns[1].isOn = true;

                // Disables the other toggle header buttons
                SFWToggleBtns[0].isOn = false;
                SFWToggleBtns[1].isOn = false;

                NSFWToggleBtns[0].isOn = false;
                NSFWToggleBtns[1].isOn = false;

                NoiseToggleBtns[0].isOn = false;
                NoiseToggleBtns[1].isOn = false;
                break;
            case "Noise":
                // Enables that preset's type toggle header button
                NoiseToggleBtns[0].isOn = true;
                NoiseToggleBtns[1].isOn = true;

                // Disables the other toggle header buttons
                SFWToggleBtns[0].isOn = false;
                SFWToggleBtns[1].isOn = false;

                NSFWToggleBtns[0].isOn = false;
                NSFWToggleBtns[1].isOn = false;

                ToadToggleBtns[0].isOn = false;
                ToadToggleBtns[1].isOn = false;

                break;
        }
    }
}
