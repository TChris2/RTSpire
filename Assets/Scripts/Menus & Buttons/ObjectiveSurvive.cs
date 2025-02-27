using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls win state of Survive Levels
public class ObjectiveSurvive : MonoBehaviour
{
    // Start time for lv
    [SerializeField]
    private float totalTime = 80;
    // Current time
    public float timer;
    // Text
    private TMPro.TMP_Text SurviveText;
    private TMPro.TMP_Text Timer;
    private bool counting;
    
    private PlayerState pState;

    void Start()
    {
        // Gets Start time for lv
        timer = totalTime;
        counting = false;

        // Gets text
        SurviveText = GameObject.Find("SurviveText").GetComponent<TMPro.TMP_Text>();
        Timer = GameObject.Find("Timer").GetComponent<TMPro.TMP_Text>();

        // Updates text
        SurviveText.text = $"{"Survive"}";
        Timer.text = $"{timer}";
        StartCoroutine(TimerStart());

        pState = GameObject.Find("Player Hitbox").GetComponent<PlayerState>();
    }

    private IEnumerator TimerStart()
    {
        // Enables the timer to start decreasing
        yield return new WaitForSeconds(1f);
        counting = true;
    }

    void FixedUpdate()
    {
        // Decreases timer
        if (counting)    
        {
            timer -= Time.deltaTime;
            int roundedTime = Mathf.RoundToInt(timer);
        
            if (roundedTime >= 0)
                Timer.text = $"{roundedTime}";
            else
                pState.isWin = true;
        }
    }
}
