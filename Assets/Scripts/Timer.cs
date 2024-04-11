using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    TMPro.TMP_Text currentTimetext;
    public static float timer = 0;

    void Awake()
    {
        timer = 0;
        currentTimetext = GetComponent<TMPro.TMP_Text>();
    } 

    void Update()
    {
        timer += Time.deltaTime;

        UpdateTimerText();
    }

    void UpdateTimerText()
    {

        //updates the time on text
        currentTimetext.text = $"{timer}s";
    }
}
