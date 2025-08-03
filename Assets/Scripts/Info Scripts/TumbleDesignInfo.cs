using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stores tumble design info
[System.Serializable]
public class TumbleDesignInfo
{
    public int no;
    public string identity;
    public Sprite sprite;
    public bool isEnabled;
    // Used when user deciding whether enable or disable designs and store temp values here until the user saves them
    public bool isEnabledTemp;
    // Presets
    public bool isSFW;
    public bool isPun;
    public bool isPerson;
    public bool isObject;
    public bool isAnimal;

    public TumbleDesignInfo(int no, string identity, Sprite sprite, bool isEnabled, bool isSFW, bool isPun, 
        bool isPerson, bool isObject, bool isAnimal)
    {
        this.no = no;
        this.identity = identity;
        this.sprite = sprite;
        this.isEnabled = isEnabled;
        this.isEnabledTemp = isEnabled;
        this.isSFW = isSFW;
        this.isPun = isPun;
        this.isPerson = isPerson;
        this.isObject = isObject;
        this.isAnimal = isAnimal;
    }
}
