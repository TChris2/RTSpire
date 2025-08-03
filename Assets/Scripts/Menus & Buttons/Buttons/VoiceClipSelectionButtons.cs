using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using TMPro;

// Button functions for the voice clip menu
public class VoiceClipSelectionButtons : MonoBehaviour
{
    [Header("Clips")]
    // SFW Clips
    [SerializeField]
    private List<VoiceClipInfo> sfwClips;
    // NSFW Clips
    [SerializeField]
    private List<VoiceClipInfo> nsfwClips;
    // Toad Clips
    [SerializeField]
    private List<VoiceClipInfo> toadClips;
    // Noise clips
    [SerializeField]
    private List<VoiceClipInfo> noiseClips;
    // The combined list of all selected clips
    public List<AudioClip> clipList = new List<AudioClip>();
    // Keeps track of total amount of clips
    public int clipTotal;
    // Toggle buttons 
    [Header("Toggles")]
    // 0) is toggle in the main menu 1) The toggle in the specific type menu
    [SerializeField]
    private Toggle AllToggleBtn;
    [SerializeField]
    private Toggle[] SFWToggleBtns = new Toggle[2];
    [SerializeField]
    private Toggle[] NSFWToggleBtns = new Toggle[2];
    [SerializeField]
    private Toggle[] ToadToggleBtns = new Toggle[2];
    [SerializeField]
    private Toggle[] NoiseToggleBtns = new Toggle[2];
    // YT audio toggle
    public bool isYtAudio;
    [SerializeField]
    private Toggle ytAudioToggle;
    [SerializeField]
    private TMPro.TMP_Text ytAudioToggleTxt;
    [Header("Clip Type Menu Stuff")]
    [SerializeField]
    private GameObject[] clipTypeMenuItems = new GameObject[2];
    private List<Toggle> clipTypeToggles = new List<Toggle>();

    void Awake()
    {
        // Loads clip information from file
        LoadFile();
    }

    void Start()
    {
        ytAudioToggle.isOn = PlayerPrefs.GetFloat("Youtube Audio Toggle", 0) != 0;
        YtAudioToggle(true);
    }

    // Loading & Saving Stuff
    //-------------------------------
    // Loads clip information from file
    public void LoadFile()
    {
        // Clears the list & total
        clipList.Clear();
        clipTotal = 0;

        string filePath = Path.Combine(Application.persistentDataPath, "VoiceClips.txt");
        //Debug.Log($"persistentDataPath {File.Exists(filePath)}");

        // If the file doesn't exist in persistentDataPath (i.e. start of a new/clean version of the game), 
        // it uses the starting version in the resources folder
        if (!File.Exists(filePath))
        {
            Debug.LogError("persistentDataPath verision does not exist, Loading Resources folder version");
            // Load the text file from Resources
            TextAsset textAsset = Resources.Load<TextAsset>("VoiceClips");

            if (textAsset == null)
            {
                Debug.LogError("Voice Clips txt file not found in Resources folder.");
                return;
            }

            File.WriteAllText(filePath, textAsset.text);
        }

        string[] lines = File.ReadAllLines(filePath);
        VoiceClipInfo vClip = null;

        // Goes through each line in the text file
        foreach (string line in lines)
        {
            // Removes empty space
            string trimmedLine = line.Trim();
            // Skips non data lines
            if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("_") ||
                trimmedLine.StartsWith("=-=-=-=-=-=-=-=-=")) continue;

            // Splits data lines into two parts
            string[] parts = trimmedLine.Split(':');

            if (parts.Length < 2) continue;

            string key = parts[0].Trim();
            string value = parts[1].Trim();

            // Gets the informations and adds it to a class
            switch (key)
            {
                case "File Name":
                    // Saves clip to lists
                    if (vClip != null) NewClip(vClip);
                    // Creates a blank entry
                    vClip = new VoiceClipInfo(value, "", null, false, false, false, false, false, false, false, false);
                    break;
                case "Clip Name":
                    if (vClip != null) vClip.clipName = value;
                    break;
                case "isEnabled":
                    if (vClip != null) { if (value == "True") vClip.isEnabled = true; else vClip.isEnabled = false; }
                    break;
                case "isSFW":
                    if (vClip != null) { if (value == "True") vClip.isSFW = true; else vClip.isSFW = false; }
                    break;
                case "isNSFW":
                    if (vClip != null) { if (value == "True") vClip.isNSFW = true; else vClip.isNSFW = false; }
                    break;
                case "isToad":
                    if (vClip != null) { if (value == "True") vClip.isToad = true; else vClip.isToad = false; }
                    break;
                case "isNoise":
                    if (vClip != null) { if (value == "True") vClip.isNoise = true; else vClip.isNoise = false; }
                    break;
                case "isChat":
                    if (vClip != null) { if (value == "True") vClip.isChat = true; else vClip.isChat = false; }
                    break;
                case "isBrainrot":
                    if (vClip != null) { if (value == "True") vClip.isBrainrot = true; else vClip.isBrainrot = false; }
                    break;
                case "isConcern":
                    if (vClip != null) { if (value == "True") vClip.isConcern = true; else vClip.isConcern = false; }
                    break;
            }
        }

        // Saves clip to lists
        if (vClip != null)
            NewClip(vClip);
    }

    // Saves clip to lists
    private void NewClip(VoiceClipInfo vClip)
    {
        VoiceClipInfo newVClip = new VoiceClipInfo(vClip.fileName, vClip.clipName, vClip.clip,
        vClip.isEnabled, vClip.isSFW, vClip.isNSFW, vClip.isToad, vClip.isNoise, vClip.isChat,
        vClip.isBrainrot, vClip.isConcern);

        // Adds the clip to its specific clip type list
        if (newVClip.isSFW)
        {
            newVClip.clip = Resources.Load<AudioClip>($"RT Voice Clips/SFW/{newVClip.fileName}");
            sfwClips.Add(newVClip);
        }
        if (newVClip.isNSFW)
        {
            newVClip.clip = Resources.Load<AudioClip>($"RT Voice Clips/NSFW/{newVClip.fileName}");
            nsfwClips.Add(newVClip);
        }
        if (newVClip.isToad)
        {
            newVClip.clip = Resources.Load<AudioClip>($"RT Voice Clips/Toad/{newVClip.fileName}");
            toadClips.Add(newVClip);
        }
        if (newVClip.isNoise)
        {
            newVClip.clip = Resources.Load<AudioClip>($"RT Voice Clips/Noises/{newVClip.fileName}");
            noiseClips.Add(newVClip);
        }

        // Adds it to the list if it is enabled
        if (newVClip.isEnabled)
        {
            clipList.Add(newVClip.clip);
        }

        // Counts clip
        clipTotal += 1;
    }

    // Saves selected clips to the list and file
    public void SaveClipSelection()
    {
        // Clears the list
        clipList.Clear();

        // Adds the enabled clips to the clip list
        AddToList(sfwClips);
        AddToList(nsfwClips);
        AddToList(toadClips);
        AddToList(noiseClips);

        // Saves clip info to a file
        SaveToFile();

        // Resets enemy audio so changes can occur
        EnemyAudioReset();
    }
    
    // Resets enemy audio so changes can occur
    public void EnemyAudioReset()
    {
        VoiceClips[] enemiesVC = FindObjectsOfType<VoiceClips>();
        if (enemiesVC != null)
        {
            foreach (VoiceClips enemy in enemiesVC)
                if (enemy.audioSource.isPlaying)
                    enemy.audioSource.Stop();
        }
    }

    // Adds clips to the list if selected
    private void AddToList(List<VoiceClipInfo> vClips)
    {
        foreach (VoiceClipInfo clip in vClips)
        {
            // Makes it temp value the current value
            clip.isEnabled = clip.isEnabledTemp;

            // Adds it to the clip list if it is enabled
            if (clip.isEnabledTemp)
                clipList.Add(clip.clip);
        }
    }

    // Saves clip info to a file
    public void SaveToFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "VoiceClips.txt");
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            WriteToFile(writer, sfwClips);
            WriteToFile(writer, nsfwClips);
            WriteToFile(writer, toadClips);
            WriteToFile(writer, noiseClips);
        }

        Debug.Log("Voice clips saved to file");
    }

    // Write the clip information to the file in the same format it was read
    void WriteToFile(StreamWriter writer, List<VoiceClipInfo> vClips)
    {
        foreach (VoiceClipInfo clip in vClips)
        {
            writer.WriteLine($"File Name: {clip.fileName}");
            writer.WriteLine($"Clip Name: {clip.clipName}");
            writer.WriteLine($"isEnabled: {clip.isEnabled}");
            writer.WriteLine($"isSFW: {clip.isSFW}");
            writer.WriteLine($"isNSFW: {clip.isNSFW}");
            writer.WriteLine($"isToad: {clip.isToad}");
            writer.WriteLine($"isNoise: {clip.isNoise}");
            writer.WriteLine($"isChat: {clip.isChat}");
            writer.WriteLine($"isBrainrot: {clip.isBrainrot}");
            writer.WriteLine($"isConcern: {clip.isConcern}");
            writer.WriteLine();
        }
    }

    // Resets all clip's temp enabled values
    public void ReloadEnables()
    {
        ReloadClipType(sfwClips);
        ReloadClipType(nsfwClips);
        ReloadClipType(toadClips);
        ReloadClipType(noiseClips);
    }

    // Resets clip type's temp enabled values
    public void ReloadClipType(List<VoiceClipInfo> vClips)
    {
        foreach (VoiceClipInfo clip in vClips)
            clip.isEnabled = clip.isEnabledTemp;
    }

    // Toggles
    //-------------------------------
    // Enables or Disables All Clips depending on if it is enabled or disabled
    public void AllToggle(bool isToggle)
    {
        // isToggle is so that all preset button can use the same code all the all toggle
        if (!isToggle)
        {
            AllToggleBtn.isOn = true;
        }

        bool isEnabled = AllToggleBtn.isOn;

        SFWToggleBtns[0].isOn = AllToggleBtn.isOn;
        EnableDisableAll(sfwClips, isEnabled);

        NSFWToggleBtns[0].isOn = AllToggleBtn.isOn;
        EnableDisableAll(nsfwClips, isEnabled);

        ToadToggleBtns[0].isOn = AllToggleBtn.isOn;
        EnableDisableAll(toadClips, isEnabled);

        NoiseToggleBtns[0].isOn = AllToggleBtn.isOn;
        EnableDisableAll(noiseClips, isEnabled);
    }

    // Enables or Disables All Clips of that type depending on if it is enabled or disabled
    // SFW
    public void SFWToggle(int tBtn) { Toggle(tBtn, "SFW", false); }
    // NSFW
    public void NSFWToggle(int tBtn) { Toggle(tBtn, "NSFW", false); }
    // Toad
    public void ToadToggle(int tBtn) { Toggle(tBtn, "Toad", false); }
    // Noise
    public void NoiseToggle(int tBtn) { Toggle(tBtn, "Noise", false); }

    // Enables or Disables All of the selected toggles clips depending on if it is enabled or disabled
    public void Toggle(int tBtn, string clipType, bool inTypeMenu)
    {
        Toggle[] tempToggleBtns = GetToggles(clipType);
        List<VoiceClipInfo> vClips = GetClipType(clipType);

        // Ensures when toggle button in specific menu is enabled it does the same in the main menu
        if (tBtn == 1)
        {
            // Ensures both toggle btns have the same value
            tempToggleBtns[0].isOn = tempToggleBtns[1].isOn;
        }

        if (inTypeMenu)
            EnableDisableAllBtns(vClips, tempToggleBtns[0].isOn);
        else
            EnableDisableAll(vClips, tempToggleBtns[0].isOn);
    }

    // Enables or disables all clips of that type
    private void EnableDisableAll(List<VoiceClipInfo> vClips, bool isEnabled)
    {
        foreach (VoiceClipInfo clip in vClips)
            clip.isEnabledTemp = isEnabled;
    }

    // Enables or disables all clips & toggles of that type
    private void EnableDisableAllBtns(List<VoiceClipInfo> vClips, bool isEnabled)
    {
        for (int i = 0; i < vClips.Count; i++)
            clipTypeToggles[i].isOn = isEnabled;
    }

    // Get toggles for Toggle function
    Toggle[] GetToggles(string clipType)
    {
        // Gets the array length of that clipType
        switch (clipType)
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

    // Presets
    //-------------------------------
    // Gets the preset type from the button and applies preset to each group of toggle buttons
    public void Preset(string presetType)
    {
        // Enables that preset's type toggle buttons & disables the others
        ToggleEnableDisable(presetType);
        // Applies preset to all clips
        ApplyPreset(sfwClips, presetType);
        ApplyPreset(nsfwClips, presetType);
        ApplyPreset(toadClips, presetType);
        ApplyPreset(noiseClips, presetType);
    }

    // Enables that preset's type toggle header button & disables the others
    private void ToggleEnableDisable(string presetType)
    {
        // Intially disables all of the toggle buttons
        SFWToggleBtns[0].isOn = false;
        NSFWToggleBtns[0].isOn = false;
        ToadToggleBtns[0].isOn = false;
        NoiseToggleBtns[0].isOn = false;
        AllToggleBtn.isOn = false;

        // Renables the specific preset button
        switch (presetType)
        {
            case "SFW":
                SFWToggleBtns[0].isOn = true;
                break;
            case "NSFW":
                NSFWToggleBtns[0].isOn = true;
                break;
            case "Toad":
                ToadToggleBtns[0].isOn = true;
                break;
            case "Noise":
                NoiseToggleBtns[0].isOn = true;
                break;
        }
    }

    // Applies preset to all clips
    private void ApplyPreset(List<VoiceClipInfo> vClips, string presetType)
    {
        foreach (VoiceClipInfo clip in vClips)
        {
            // Gets the value of the preset for the toggle button
            bool isPreset = GetPresetValue(presetType, clip);

            // Checks to see if the preset enabled or disabled and enables/disables the toggle
            if (isPreset)
                clip.isEnabledTemp = true;
            else
                clip.isEnabledTemp = false;
        }
    }

    // Finds the preset value the preset button is applying and returns it 
    private bool GetPresetValue(string presetType, VoiceClipInfo clip)
    {
        switch (presetType)
        {
            case "SFW":
                return clip.isSFW;
            case "NSFW":
                return clip.isNSFW;
            case "Toad":
                return clip.isToad;
            case "Noise":
                return clip.isNoise;
            case "Chat":
                return clip.isChat;
            case "Brainrot":
                return clip.isBrainrot;
            case "Concern":
                return clip.isConcern;
            default:
                return true;
        }
    }

    // Get clip type
    public List<VoiceClipInfo> GetClipType(string clipType)
    {
        // Gets the array length of that clipType
        switch (clipType)
        {
            case "SFW":
                return sfwClips;
            case "NSFW":
                return nsfwClips;
            case "Toad":
                return toadClips;
            case "Noise":
                return noiseClips;
            default:
                return null;
        }
    }

    public void CreateClipTypeMenu(string subTypeMenuName, GameObject subTypeMenu, AudioSource audioSource)
    {
        clipTypeToggles.Clear();

        Transform subTypeContent = subTypeMenu.transform;

        // Gets clip type of menu being created
        List<VoiceClipInfo> vClips = GetClipType(subTypeMenuName);

        // Header toggler
        GameObject item = Instantiate(clipTypeMenuItems[0], subTypeContent);
        TMPro.TMP_Text txt = item.GetComponentInChildren<TMPro.TMP_Text>();
        txt.text = $"{subTypeMenuName} -";
        Toggle toggle = item.GetComponentInChildren<Toggle>();
        toggle.onValueChanged.AddListener((bool isOn) => { Toggle(1, subTypeMenuName, true); });

        Toggle[] tempToggleBtns = GetToggles(subTypeMenuName);
        tempToggleBtns[1] = toggle;

        // Main items of subtype menu
        foreach (VoiceClipInfo clip in vClips)
        {
            item = Instantiate(clipTypeMenuItems[1], subTypeContent);

            // Gives it a name
            item.name = $"{subTypeMenuName} Voice Clip {clip.fileName}";

            // Toggle
            toggle = item.GetComponentInChildren<Toggle>();
            toggle.isOn = clip.isEnabledTemp;
            toggle.onValueChanged.AddListener((bool isOn) =>
            {
                clip.isEnabledTemp = isOn;
            });
            clipTypeToggles.Add(toggle);

            // Button
            Button btn = item.GetComponentInChildren<Button>();
            txt = btn.GetComponentInChildren<TMPro.TMP_Text>();
            txt.text = clip.clipName;
            if (clip.clipName.Contains("<sprite"))
            {
                txt.alignment = TextAlignmentOptions.MidlineLeft;
                txt.fontSizeMax = 100;
            }
            
            // Plays associated audio clip when button is pressed
                btn.onClick.AddListener(() =>
            {
                audioSource.clip = clip.clip;
                audioSource.Play();
            });
        }
    }

    // Enable instead of "removing" clips from the pool it instead "replaces" those clips with Youtube Clips
    public void YtAudioToggle(bool isStart)
    {
        if (ytAudioToggle.isOn)
            ytAudioToggleTxt.text = "- Disabled Clips <style=\"Active\">Are</style> Replaced With <sprite name=\"youtube\">";
        else
            ytAudioToggleTxt.text = "- Disabled Clips <style=\"InActive\">Are Not</style> Replaced With <sprite name=\"youtube\">";
        isYtAudio = ytAudioToggle.isOn;

        // Resets enemy audio so changes can occur, does not occur at start
        if (!isStart)
            EnemyAudioReset();
    }
    
    private void OnDisable()
    {
        // Saves prefs
        PlayerPrefs.SetFloat("Youtube Audio Toggle", isYtAudio ? 1 : 0);

        PlayerPrefs.Save();
    }
}
