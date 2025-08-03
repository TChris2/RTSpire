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
    public float TMachineCounter;
    // Text
    private TMPro.TMP_Text DestroyText;
    private TMPro.TMP_Text DestroyCounter;
    private PlayerState pState;

    void Start()
    {
        // Get total machines for the level
        TMachineCounter = TMachineTotal;
        DestroyText = GameObject.Find("DestroyText").GetComponent<TMPro.TMP_Text>();
        DestroyCounter = GameObject.Find("DestroyCounter").GetComponent<TMPro.TMP_Text>();

        // Updates text with amt of machines
        DestroyText.text = $"{"Destroy Machines"}";
        DestroyCounter.text = $"{TMachineCounter}";

        // Gets player comps
        pState = GameObject.Find("Player Hitbox").GetComponent<PlayerState>();
    }

    // Checks to see if the player has destroyed all the machines
    public void WinCheck()
    {
        if (TMachineCounter <= 0)
            Invoke("Win", .9f);
    }

    // If the player has destroyed all machines
    void Win()
    {
        StartCoroutine(pState.PlayerWin());
    }
}
