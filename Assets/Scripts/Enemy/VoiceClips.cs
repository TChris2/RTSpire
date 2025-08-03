using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Plays a random sfx
public class VoiceClips : MonoBehaviour
{
    // Gets list of voice clips
    private VoiceClipSelectionButtons vcMenu;
    // Gets voice clip variables
    private TumbleAudioSettings tumbleAudio;
    public AudioSource audioSource;
    bool audioPlay = true;
    // YT clips played if user has enabled the yt audio op
    [SerializeField]
    private AudioClip[] ytClips;

    void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Start()
    {
        // Gets voice clip variables
        tumbleAudio = GameObject.Find("Audio Options Menu").GetComponent<TumbleAudioSettings>();
        // Gets list of voice clips
        vcMenu = GameObject.Find("Voice Clip Menu").GetComponent<VoiceClipSelectionButtons>();

        if (tumbleAudio.isClipPlay && tumbleAudio.yapRate != 0 && (vcMenu.clipList.Count != 0 || vcMenu.isYtAudio))
            StartCoroutine(RandomClipStart());
    }

    void Update()
    {
        // If a sfx isn't already playing and the player has not won or died
        if (tumbleAudio.isClipPlay && tumbleAudio.yapRate != 0 && (vcMenu.clipList.Count != 0 || vcMenu.isYtAudio) && audioPlay == true)
            StartCoroutine(RandomClip());
    }

    // Plays a random sfx
    IEnumerator RandomClip()
    {
        // Set to false to prevent multiple sfxs from playing at the same time
        audioPlay = false;

        // Sets a random delay to prevent sfxs from all playing at the same time
        yield return new WaitForSeconds(Random.Range(tumbleAudio.minDelay, tumbleAudio.maxDelay + 1));

        // Checks to make sure any of the conditions have changed during the delay
        if (tumbleAudio.isClipPlay && tumbleAudio.yapRate != 0 && (vcMenu.clipList.Count != 0 || vcMenu.isYtAudio))
        {
            float clipLength = ChooseClip();            
            yield return new WaitForSeconds(clipLength);

            audioPlay = true;
        }
    }

    // Plays a random clip when the enemy is spawned in
    IEnumerator RandomClipStart()
    {
        // Set to false to prevent multiple sfxs from playing at the same time
        audioPlay = false;

        float clipLength = ChooseClip();
        yield return new WaitForSeconds(clipLength);

        // Sets a random delay to prevent sfxs from all playing at the same time
        yield return new WaitForSeconds(Random.Range(tumbleAudio.minDelay, tumbleAudio.maxDelay + 1));
        audioPlay = true;
    }

    // Gets a clip and plays it
    float ChooseClip()
    {
        AudioClip randomVC;
        // Rolls a random num to see if a clip plays
        int randomNum = Random.Range(1, 101);

        // Plays a clip if num is at or below yap rate or if yap rate is set to 100%
        if (tumbleAudio.yapRate == 100 || randomNum <= tumbleAudio.yapRate)
        {
            // If Youtube Audio Mode is enabled
            if (vcMenu.isYtAudio)
            {
                // Instead of picking a random num from the selected cliplist total it instead picks a random num from the total cliplist
                randomNum = Random.Range(0, vcMenu.clipTotal);
                // If the random num is above the cliplist count it instead picks a Youtube clip
                if (randomNum >= vcMenu.clipList.Count || vcMenu.clipList.Count == 0)
                {
                    randomNum = Random.Range(0, ytClips.Length);
                    randomVC = ytClips[randomNum];
                }
                // If below it uses a regular clip
                else
                    randomVC = vcMenu.clipList[randomNum];
            }
            // Picks a clip from the selected clip list
            else
            {
                randomNum = Random.Range(0, vcMenu.clipList.Count);
                randomVC = vcMenu.clipList[randomNum];
            }

            // Plays clip
            audioSource.PlayOneShot(randomVC);

            // Returns clip length
            return randomVC.length;
        }
        // If random num is above yap rate no clip is played and 0 is returned
        else
            return 0;
    }
}
