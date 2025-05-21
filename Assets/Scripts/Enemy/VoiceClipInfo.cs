using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VoiceClipInfo
{
    public string fileName;
    public string clipName;
    public AudioClip clip;
    public bool isEnabled;
    // Used when user deciding whether enable or disable clips and store temp values here until the user saves them
    public bool isEnabledTemp;
    // Clip Types
    public bool isSFW;
    public bool isNSFW;
    public bool isToad;
    public bool isNoise;
    // Presets
    public bool isYoutube;
    public bool isChat;
    public bool isBrainrot;
    public bool isConcern;

    public VoiceClipInfo(string fileName, string clipName, AudioClip clip, bool isEnabled, bool isSFW, bool isNSFW, 
        bool isToad, bool isNoise, bool isYoutube, bool isChat, bool isBrainrot, bool isConcern)
    {
        this.fileName = fileName;
        this.clipName = clipName;
        this.clip = clip;
        this.isEnabled = isEnabled;
        this.isEnabledTemp = isEnabled;
        this.isSFW = isSFW;
        this.isNSFW = isNSFW;
        this.isToad = isToad;
        this.isNoise = isNoise;
        this.isYoutube = isYoutube;
        this.isChat = isChat;
        this.isBrainrot = isBrainrot;
        this.isConcern = isConcern;
    }
}
