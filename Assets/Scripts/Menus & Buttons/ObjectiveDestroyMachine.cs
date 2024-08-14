using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls win state of Destroy Machine levels
public class ObjectiveDestroyMachine : MonoBehaviour
{
    // Sets how many machines the player has to destroy
    [SerializeField]
    private float TMachineTotal;
    // Keeps track of the remaining machines the player has to destroy
    public static float TMachineCounter;
    // Text
    private TMPro.TMP_Text DestroyText;
    private TMPro.TMP_Text DestroyCounter;
    
    void Start()
    {
        // Get total machines for the level
        TMachineCounter = TMachineTotal;
        DestroyText = GameObject.Find("DestroyText").GetComponent<TMPro.TMP_Text>();
        DestroyCounter = GameObject.Find("DestroyCounter").GetComponent<TMPro.TMP_Text>();

        // Updates text with amt of machines
        DestroyText.text = $"{"Destroy Machines"}";
        DestroyCounter.text = $"{TMachineCounter}";
    }
}
