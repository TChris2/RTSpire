using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Controls the different volume sliders
public class VolumeSettings : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;
    // Master Volume
    [SerializeField]
    private Slider mastSlider;
    private float mastVol;
    // Music Volume
    [SerializeField]
    private Slider musicSlider;
    private float musicVol;
    // SFX Volume
    [SerializeField]
    private Slider sfxSlider;
    private float sfxVol;
    // Tumble Volume
    [SerializeField]
    private Slider tSlider;
    private float tVol;

    void Start()
    {
        // Loads the previous value of the sliders
        mastSlider.value = PlayerPrefs.GetFloat("Master Volume", 1);
        SetMasterVol();
        musicSlider.value = PlayerPrefs.GetFloat("Music Volume", 1);
        SetMusicVol();
        sfxSlider.value = PlayerPrefs.GetFloat("SFX Volume", 1);
        SetSFXVol();
        tSlider.value = PlayerPrefs.GetFloat("Tumble Volume", 1);
        SetTumbleVol();
    }

    // Updates the mixer for the master volume
    public void SetMasterVol()
    {
        mastVol = mastSlider.value;
        mixer.SetFloat("Master Volume", Mathf.Log10(mastVol)*20);
    }

    // Updates the mixer for the music volume
    public void SetMusicVol()
    {
        musicVol = musicSlider.value;
        mixer.SetFloat("Music Volume", Mathf.Log10(musicVol)*20);
    }

    // Updates the mixer for the sfx volume
    public void SetSFXVol()
    {
        sfxVol = sfxSlider.value;
        mixer.SetFloat("SFX Volume", Mathf.Log10(sfxVol)*20);
    }

    // Updates the mixer for the tumble enemies volume
    public void SetTumbleVol()
    {
        tVol = tSlider.value;
        mixer.SetFloat("Tumble Volume", Mathf.Log10(tVol)*20);
    }

    // Saves the value of the sliders
    private void OnDisable()
    {
        PlayerPrefs.SetFloat("Master Volume", mastVol);
        PlayerPrefs.SetFloat("Music Volume", musicVol);
        PlayerPrefs.SetFloat("SFX Volume", sfxVol);
        PlayerPrefs.SetFloat("Tumble Volume", tVol);
        PlayerPrefs.Save();
    }
}
