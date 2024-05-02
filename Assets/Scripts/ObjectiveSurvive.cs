using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls win state of Survive Levels
public class ObjectiveSurvive : MonoBehaviour
{
    [SerializeField]
    private float totalTime = 80;
    public static float timer;
    // Text
    private TMPro.TMP_Text SurviveText;
    private TMPro.TMP_Text Timer;
    private bool counting;
    void Start()
    {
        timer = totalTime;
        counting = false;

        SurviveText = GameObject.Find("SurviveText").GetComponent<TMPro.TMP_Text>();
        Timer = GameObject.Find("Timer").GetComponent<TMPro.TMP_Text>();

        // Updates text
        SurviveText.text = $"{"Survive"}";
        Timer.text = $"{timer}";
        StartCoroutine(TimerStart());
    }

    private IEnumerator TimerStart()
    {
        yield return new WaitForSeconds(1f);
        counting = true;
    }

    void FixedUpdate()
    {
        if (counting && !PlayerState.isDead)    
        {
            timer -= Time.deltaTime;
            int roundedTime = Mathf.RoundToInt(timer);
        
            if (roundedTime >= 0)
                Timer.text = $"{roundedTime}";
            else
                PlayerState.isWin = true;
        }
    }
}
