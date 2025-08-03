using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

// Button functions for the tumble design menu
public class TumbleDesignSelectionButtons : MonoBehaviour
{
    [Header("Tumble Designs")]
    // List of tumbles with all their information
    [SerializeField]
    private List<TumbleDesignInfo> tumbleInfoList;
    // The combined list of all selected tumbles
    public List<Sprite> tumbleList = new List<Sprite>();
    // Keeps track of total amount of tumble designs
    public int tumbleTotal;
    // Temp holds sprites
    Sprite[] sprites;
    // Toggle buttons
    [Header("Toggles")]
    // 0) is toggle in the main menu 1) The toggle in the specific type menu
    [SerializeField]
    private Toggle AllToggleBtn;
    // YT tumble toggle
    public bool isYtTumble;
    [SerializeField]
    private Toggle ytTumbleToggle;
    [SerializeField]
    private TMPro.TMP_Text ytAudioToggleTxt;
    [Header("Sub Type Menu Stuff")]
    [SerializeField]
    private GameObject[] typeMenuItems = new GameObject[2];
    private List<Toggle> typeMenuToggles = new List<Toggle>();

    void Awake()
    {
        // Loads tumble design information from file
        LoadFile();
    }

    void Start()
    {
        ytTumbleToggle.isOn = PlayerPrefs.GetFloat("Youtube Tumble Toggle", 0) != 0;
        YtTumbleToggle(true);
    }

    // Loading & Saving Stuff
    //-------------------------------
    // Loads tumble design information from file
    public void LoadFile()
    {
        sprites = Resources.LoadAll<Sprite>("Tumble Designs");

        // Clears the list & total
        tumbleList.Clear();
        tumbleTotal = 0;

        string filePath = Path.Combine(Application.persistentDataPath, "TumbleDesigns.txt");
        //Debug.Log($"persistentDataPath {File.Exists(filePath)}");

        // If the file doesn't exist in persistentDataPath (i.e. start of a new/clean version of the game), 
        // it uses the starting version in the resources folder
        if (!File.Exists(filePath))
        {
            Debug.LogError("persistentDataPath verision does not exist, Loading Resources folder version");
            // Load the text file from Resources
            TextAsset textAsset = Resources.Load<TextAsset>("TumbleDesigns");

            if (textAsset == null)
            {
                Debug.LogError("Tumble Designs txt file not found in Resources folder.");
                return;
            }

            File.WriteAllText(filePath, textAsset.text);
        }

        string[] lines = File.ReadAllLines(filePath);
        TumbleDesignInfo tDesign = null;

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
                case "No":
                    // Saves tumble to lists
                    if (tDesign != null) NewTumble(tDesign);
                    // Creates a blank entry
                    tDesign = new TumbleDesignInfo(int.Parse(value), "", null, false, false, false, false, false, false);
                    break;
                case "Identity":
                    if (tDesign != null) tDesign.identity = value;
                    break;
                case "isEnabled":
                    if (tDesign != null) { if (value == "True") tDesign.isEnabled = true; else tDesign.isEnabled = false; }
                    break;
                case "isSFW":
                    if (tDesign != null) { if (value == "True") tDesign.isSFW = true; else tDesign.isSFW = false; }
                    break;
                case "isPun":
                    if (tDesign != null) { if (value == "True") tDesign.isPun = true; else tDesign.isPun = false; }
                    break;
                case "isPerson":
                    if (tDesign != null) { if (value == "True") tDesign.isPerson = true; else tDesign.isPerson = false; }
                    break;
                case "isObject":
                    if (tDesign != null) { if (value == "True") tDesign.isObject = true; else tDesign.isObject = false; }
                    break;
                case "isAnimal":
                    if (tDesign != null) { if (value == "True") tDesign.isAnimal = true; else tDesign.isAnimal = false; }
                    break;
            }
        }

        // Saves tumble to lists
        if (tDesign != null)
            NewTumble(tDesign);
    }

    // Saves tumble to lists
    private void NewTumble(TumbleDesignInfo tDesign)
    {
        TumbleDesignInfo newTDesign = new TumbleDesignInfo(tDesign.no, tDesign.identity, tDesign.sprite,
        tDesign.isEnabled, tDesign.isSFW, tDesign.isPun, tDesign.isPerson, tDesign.isObject, tDesign.isAnimal);

        // Gets sprite
        newTDesign.sprite = sprites[newTDesign.no - 1];
        
        // Adds tumble to the info list
        tumbleInfoList.Add(newTDesign);

        // Adds it to the main list if it is enabled
        if (newTDesign.isEnabled)
        {
            tumbleList.Add(newTDesign.sprite);
        }

        // Counts tumble
        tumbleTotal += 1;
    }

    // Saves selected clips to the list and file
    public void SaveDesignSelection()
    {
        // Clears the list
        tumbleList.Clear();

        // Adds the enabled tumbles to the list
        foreach (TumbleDesignInfo tumble in tumbleInfoList)
        {
            // Makes it temp value the current value
            tumble.isEnabled = tumble.isEnabledTemp;

            // Adds it to the tumble list if it is enabled
            if (tumble.isEnabledTemp)
                tumbleList.Add(tumble.sprite);
        }

        // Saves tumble info to a file
        SaveToFile();

        // Resets enemy audio so changes can occur
        TumbleDesignReload();
    }
    
    // Reloads enemy designs
    public void TumbleDesignReload()
    {
        TumbleDesignSetter[] enemiesDesign = FindObjectsOfType<TumbleDesignSetter>();
        if (enemiesDesign != null)
        {
            foreach (TumbleDesignSetter enemy in enemiesDesign)
                enemy.RandomTumbleDesign();
        }
    }

    // Saves tumble info to a file
    public void SaveToFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "TumbleDesigns.txt");
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            // Writes the information to the file in the same format it was read
            foreach (TumbleDesignInfo tumble in tumbleInfoList)
            {
                writer.WriteLine($"No: {tumble.no}");
                writer.WriteLine($"Identity: {tumble.identity}");
                writer.WriteLine($"isEnabled: {tumble.isEnabled}");
                writer.WriteLine($"isSFW: {tumble.isSFW}");
                writer.WriteLine($"isPun: {tumble.isPun}");
                writer.WriteLine($"isPerson: {tumble.isPerson}");
                writer.WriteLine($"isObject: {tumble.isObject}");
                writer.WriteLine($"isAnimal: {tumble.isAnimal}");
                writer.WriteLine();
            }
        }

        Debug.Log("Tumble designs saved to file");
    }

    // Resets all tumble's temp enabled values
    public void ReloadEnables()
    {
        foreach (TumbleDesignInfo tumble in tumbleInfoList)
            tumble.isEnabled = tumble.isEnabledTemp;
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

        EnableDisableAll(0, tumbleInfoList.Count, isEnabled);
    }

    // Enables or disables all clips of that type
    private void EnableDisableAll(int loopStart, int loopEnd, bool isEnabled)
    {
        for (int i = loopStart; i < loopEnd; i++)
            tumbleInfoList[i].isEnabledTemp = isEnabled;
    }

    // Enables or disables all clips & toggles of that type
    private void EnableDisableAllBtns(int loopStart, int loopEnd, bool isEnabled)
    {
        for (int i = loopStart; i < loopEnd; i++)
            typeMenuToggles[i].isOn = isEnabled;
    }

    // Presets
    //-------------------------------
    // Gets the preset type from the button and applies preset to each group of toggle buttons
    public void Preset(string presetType)
    {
        // Disables all toggle button
        AllToggleBtn.isOn = false;

        // Applies preset to all designs
        foreach (TumbleDesignInfo tumble in tumbleInfoList)
        {
            // Gets the value of the preset for the toggle button
            bool isPreset = GetPresetValue(presetType, tumble);

            // Checks to see if the preset enabled or disabled and enables/disables the toggle
            if (isPreset)
                tumble.isEnabledTemp = true;
            else
                tumble.isEnabledTemp = false;
        }
    }

    // Finds the preset value the preset button is applying and returns it 
    private bool GetPresetValue(string presetType, TumbleDesignInfo tumble)
    {
        switch (presetType)
        {
            case "SFW":
                return tumble.isSFW;
            case "Pun":
                return tumble.isPun;
            case "Person":
                return tumble.isPerson;
            case "Object":
                return tumble.isObject;
            case "Animal":
                return tumble.isAnimal;
            default:
                return true;
        }
    }

    // Creates sub design menu
    public void CreateSubDesignMenu(int subDesignMenu, GameObject subTypeMenu, AudioSource audioSource)
    {
        typeMenuToggles.Clear();

        Transform subTypeContent = subTypeMenu.transform;

        int loopStart = subDesignMenu - 100;
        int loopEnd = (subDesignMenu == 999) ? 750 : subDesignMenu;

        // Header toggle
        GameObject item = Instantiate(typeMenuItems[0], subTypeContent);
        TMPro.TMP_Text txt = item.GetComponentInChildren<TMPro.TMP_Text>();
        txt.text = $"{loopStart+1}-{subDesignMenu} -";
        Toggle toggle = item.GetComponentInChildren<Toggle>();
        // Enables or Disables all of the toggles in the subtype menu
        toggle.onValueChanged.AddListener((bool isOn) =>
        {
            ///////////////////////////////////////////////////
            ///////////////////////////////////////////////////
            // Make sure to update if statement to get the value for loop end when you add all the the tumbles to game
            ///////////////////////////////////////////////////
            ///////////////////////////////////////////////////
            EnableDisableAllBtns(0, (subDesignMenu == 999) ? 50 : 100, !isOn);
        });

        Sprite[] sprites = Resources.LoadAll<Sprite>($"Tumble Designs/{subDesignMenu}");

        // Main items of subtype menu
        for (int i = loopStart; i < loopEnd;)
        {
            item = Instantiate(typeMenuItems[1], subTypeContent);

            // Toggle
            Toggle[] toggles = item.GetComponentsInChildren<Toggle>();

            for (int l = 0; l < 5; l++)
            {
                int index = i;
                // Gives it a name
                item.name = $"{subDesignMenu}_{(index + 101) % subDesignMenu}";
                
                Image image = toggles[l].GetComponentInChildren<Image>();
                image.sprite = sprites[(index + 100) % subDesignMenu];
                toggles[l].isOn = !tumbleInfoList[index].isEnabledTemp;

                toggles[l].onValueChanged.AddListener((bool isOn) =>
                {
                    tumbleInfoList[index].isEnabledTemp = !isOn;
                    Debug.Log(tumbleInfoList[index].no);
                });
                typeMenuToggles.Add(toggles[l]);
                
                i++;
            }
        }
    }

    // Enable instead of "removing" clips from the pool it instead "replaces" those clips with Youtube Clips
    public void YtTumbleToggle(bool isStart)
    {
        if (ytTumbleToggle.isOn)
            ytAudioToggleTxt.text = "- Disabled Designs <style=\"Active\">Are</style> Replaced With <sprite name=\"youtube\">";
        else
            ytAudioToggleTxt.text = "- Disabled Designs <style=\"InActive\">Are Not</style> Replaced With <sprite name=\"youtube\">";
        isYtTumble = ytTumbleToggle.isOn;

        // Reloads enemy designs so changes can occur, does not occur at start
        if (!isStart)
            TumbleDesignReload();
    }
    
    private void OnDisable()
    {
        // Saves prefs
        PlayerPrefs.SetFloat("Youtube Tumble Toggle", isYtTumble ? 1 : 0);

        PlayerPrefs.Save();
    }
}
