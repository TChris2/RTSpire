using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls win state of Destroy Machine levels
public class ObjectiveDestroyMachine : MonoBehaviour
{
    // Sets how many machines the player has to destroy
    [SerializeField]
    private float TMachineTotal;
    public static float TMachineCounter;
    // Text
    private TMPro.TMP_Text DestroyText;
    private TMPro.TMP_Text DestroyCounter;
    
    void Start()
    {
        TMachineCounter = TMachineTotal;
        DestroyText = GameObject.Find("DestroyText").GetComponent<TMPro.TMP_Text>();
        DestroyCounter = GameObject.Find("DestroyCounter").GetComponent<TMPro.TMP_Text>();

        // Updates text
        DestroyText.text = $"{"Destroy Machines"}";
        DestroyCounter.text = $"{ObjectiveDestroyMachine.TMachineCounter}";
    }

    // Update is called once per frame
    void Update()
    {
        // If the player has destroyed all the machines
        if (TMachineCounter <= 0)
            PlayerState.isWin = true;
    }
}
